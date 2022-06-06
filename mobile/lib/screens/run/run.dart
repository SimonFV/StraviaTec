import 'dart:io';
import 'dart:math';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:gpx/gpx.dart';
import 'package:location/location.dart';
import 'package:mobile/database/activity_type.dart';
import 'dart:async';
import 'dart:typed_data';
import 'package:mobile/database/user.dart';
import 'package:path_provider/path_provider.dart';
import 'package:stop_watch_timer/stop_watch_timer.dart';
import '../../database/activity.dart';
import 'package:intl/intl.dart';
import '../../database/database_manager.dart';

class Run extends StatefulWidget {
  final User activeUser;

  const Run({Key? key, required this.activeUser}) : super(key: key);

  @override
  _RunState createState() => _RunState();
}

class _RunState extends State<Run> {
  StreamSubscription? _locationSubscription;
  final Location _locationTracker = Location();
  Marker? marker;
  Circle? circle;
  final Completer<GoogleMapController> _controller = Completer();
  var gpx = Gpx();

  static const CameraPosition initialLocation = CameraPosition(
    target: LatLng(37.42796133580664, -122.085749655962),
    zoom: 14.4746,
  );

  final StopWatchTimer _stopWatchTimer = StopWatchTimer();
  bool recording = false;
  bool positionReady = false;
  Activity newActivity = Activity(
      userId: "",
      route: "default",
      distance: 0,
      duration: "00:00:00",
      start: "0001-01-01 00:00:00",
      type: "Running",
      status: "new");

  @override
  void initState() {
    super.initState();
  }

  @override
  void dispose() async {
    if (_locationSubscription != null) {
      _locationSubscription!.cancel();
    }
    super.dispose();
    await _stopWatchTimer.dispose(); // Need to call dispose function.
  }

  Future<Uint8List> getMarker() async {
    ByteData byteData =
        await DefaultAssetBundle.of(context).load("assets/arrow.png");
    return byteData.buffer.asUint8List();
  }

  void updateMarkerAndCircle(LocationData newLocalData, Uint8List imageData) {
    LatLng latlng = LatLng(newLocalData.latitude!, newLocalData.longitude!);
    setState(() {
      marker = Marker(
          markerId: const MarkerId("home"),
          position: latlng,
          rotation: newLocalData.heading!,
          draggable: false,
          zIndex: 2,
          flat: true,
          anchor: const Offset(0.5, 0.5),
          icon: BitmapDescriptor.fromBytes(imageData));
      circle = Circle(
          circleId: const CircleId("car"),
          radius: newLocalData.accuracy!,
          zIndex: 1,
          strokeColor: Colors.blue,
          center: latlng,
          fillColor: Colors.blue.withAlpha(70));
    });
  }

  void getCurrentLocation() async {
    try {
      Uint8List imageData = await getMarker();
      var location = await _locationTracker.getLocation();

      updateMarkerAndCircle(location, imageData);

      if (_locationSubscription != null) {
        _locationSubscription!.cancel();
      }

      _locationSubscription =
          _locationTracker.onLocationChanged.listen((newLocalData) async {
        final GoogleMapController controller = await _controller.future;
        controller.animateCamera(CameraUpdate.newCameraPosition(CameraPosition(
            bearing: 0,
            target: LatLng(newLocalData.latitude!, newLocalData.longitude!),
            tilt: 0,
            zoom: 16.00)));
        updateMarkerAndCircle(newLocalData, imageData);
        if (gpx.wpts.isEmpty) {
          gpx.wpts.add(
              Wpt(lat: newLocalData.latitude!, lon: newLocalData.longitude!));
        }
        if (recording &&
            gpx.wpts.last !=
                Wpt(
                    lat: newLocalData.latitude!,
                    lon: newLocalData.longitude!)) {
          setState(() {
            newActivity.distance += num.parse((acos(
                        sin(gpx.wpts.last.lat! * pi / 180) *
                                sin(newLocalData.latitude! * pi / 180) +
                            cos(gpx.wpts.last.lat! * pi / 180) *
                                cos(newLocalData.latitude! * pi / 180) *
                                cos((newLocalData.longitude! -
                                        gpx.wpts.last.lon!) *
                                    pi /
                                    180)) *
                    6371)
                .toStringAsFixed(3));
          });
          gpx.wpts.add(
              Wpt(lat: newLocalData.latitude!, lon: newLocalData.longitude!));
        }
      });
    } on PlatformException catch (e) {
      if (e.code == 'PERMISSION_DENIED') {
        debugPrint("Permission Denied");
      }
    }
  }

  void saveActivity(String gpxString) async {
    if (newActivity.distance == 0.0) {
      newActivity.distance = 0.001;
    }
    newActivity.userId = widget.activeUser.user;
    await DatabaseManager.instance.addActivity(newActivity);

    int id = await DatabaseManager.instance
        .getActivityId(widget.activeUser.user, newActivity.start);
    Directory documentsDirectory = await getApplicationDocumentsDirectory();
    String path = documentsDirectory.path + "/" + id.toString() + ".gpx";
    var file = File(path);
    file.writeAsString(gpxString);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
            backgroundColor: const Color.fromARGB(193, 38, 240, 172),
            title: Text(widget.activeUser.user == ""
                ? "Offline"
                : widget.activeUser.user)),
        body: AnnotatedRegion<SystemUiOverlayStyle>(
            value: SystemUiOverlayStyle.light,
            child: Container(
                height: double.infinity,
                width: double.infinity,
                padding: const EdgeInsets.fromLTRB(5, 0, 5, 5),
                decoration: const BoxDecoration(
                    gradient: LinearGradient(
                        begin: Alignment.topCenter,
                        end: Alignment.bottomCenter,
                        colors: [
                      Color.fromARGB(244, 192, 255, 224),
                      Color.fromARGB(170, 202, 240, 255),
                      Color.fromARGB(170, 202, 240, 255),
                      Color.fromARGB(170, 79, 200, 255),
                    ])),
                child: Column(
                  children: [
                    SizedBox(
                        height: 220,
                        width: double.infinity,
                        child: GoogleMap(
                          mapType: MapType.normal,
                          initialCameraPosition: initialLocation,
                          markers: Set.of((marker != null) ? [marker!] : []),
                          circles: Set.of((circle != null) ? [circle!] : []),
                          onMapCreated: (GoogleMapController controller) {
                            gpx.creator = "dart-gpx library";
                            gpx.wpts = [];
                            _controller.complete(controller);
                          },
                        )),
                    ElevatedButton.icon(
                      onPressed: () {
                        setState(() {
                          positionReady = true;
                        });
                        getCurrentLocation();
                      },
                      icon: positionReady
                          ? const Icon(Icons.my_location_outlined)
                          : const Icon(Icons.location_searching),
                      label: positionReady
                          ? const Text('Ready')
                          : const Text('Locate'),
                    ),
                    StreamBuilder<int>(
                      stream: _stopWatchTimer.rawTime,
                      initialData: 0,
                      builder: (context, snap) {
                        final value = snap.data;
                        final displayTime =
                            StopWatchTimer.getDisplayTime(value!);
                        return Column(
                          children: <Widget>[
                            Padding(
                              padding: const EdgeInsets.all(8),
                              child: Text(
                                displayTime,
                                style: const TextStyle(
                                    fontSize: 30,
                                    fontFamily: 'Helvetica',
                                    fontWeight: FontWeight.bold),
                              ),
                            ),
                            Row(
                              mainAxisAlignment: MainAxisAlignment.center,
                              children: [
                                ElevatedButton.icon(
                                    onPressed: positionReady && !recording
                                        ? () {
                                            newActivity.start = DateFormat(
                                                    'yyyy-MM-dd HH:mm:ss')
                                                .format(DateTime.now().subtract(
                                                    const Duration(hours: 6)));
                                            _stopWatchTimer.onExecute
                                                .add(StopWatchExecute.start);
                                            setState(() {
                                              recording = true;
                                            });
                                          }
                                        : null,
                                    style: ButtonStyle(
                                      backgroundColor:
                                          MaterialStateProperty.all<Color>(
                                              Colors.green),
                                    ),
                                    icon: const Icon(Icons.play_arrow),
                                    label: const Text('Start')),
                                const SizedBox(
                                  width: 5.0, // height you want
                                ),
                                ElevatedButton.icon(
                                    onPressed: () {
                                      _stopWatchTimer.onExecute
                                          .add(StopWatchExecute.stop);
                                      setState(() {
                                        recording = false;
                                      });
                                    },
                                    icon: const Icon(Icons.stop),
                                    label: const Text('Stop')),
                                const SizedBox(
                                  width: 5.0, // height you want
                                ),
                                ElevatedButton.icon(
                                    onPressed: () {
                                      _stopWatchTimer.onExecute
                                          .add(StopWatchExecute.reset);
                                      setState(() {
                                        recording = false;
                                        newActivity.distance = 0.0;
                                      });
                                      gpx.wpts.clear();
                                    },
                                    style: ButtonStyle(
                                      backgroundColor:
                                          MaterialStateProperty.all<Color>(
                                              Colors.pink),
                                    ),
                                    icon: const Icon(Icons.restore_outlined),
                                    label: const Text('Reset')),
                              ],
                            ),
                          ],
                        );
                      },
                    ),
                    const SizedBox(
                      height: 5.0, // height you want
                    ),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        FutureBuilder<List<ActivityType>>(
                            future: DatabaseManager.instance.getTypes(),
                            builder: (BuildContext context,
                                AsyncSnapshot<List<ActivityType>> snapshot) {
                              if (!snapshot.hasData) {
                                return const Center(child: Text("Loading..."));
                              }
                              final items = snapshot.data!
                                  .map((e) => e.toMap()['name'])
                                  .toList()
                                  .cast<String>();
                              return DropdownButton<String>(
                                value: newActivity.type,
                                icon: const Icon(Icons.arrow_downward),
                                elevation: 16,
                                items: items.map<DropdownMenuItem<String>>(
                                    (String value) {
                                  return DropdownMenuItem<String>(
                                    value: value,
                                    child: Text(value),
                                  );
                                }).toList(),
                                onChanged: (String? newValue) {
                                  setState(() {
                                    newActivity.type = newValue!;
                                  });
                                },
                              );
                            }),
                        const SizedBox(
                          width: 40.0, // height you want
                        ),
                        Text("Distance: " +
                            newActivity.distance.toString() +
                            " km"),
                      ],
                    ),
                    const SizedBox(
                      height: 5.0, // height you want
                    ),
                    ElevatedButton.icon(
                        onPressed: _stopWatchTimer.rawTime.value != 0
                            ? () {
                                newActivity.duration =
                                    StopWatchTimer.getDisplayTime(
                                        _stopWatchTimer.rawTime.value,
                                        milliSecond: false);
                                _stopWatchTimer.onExecute
                                    .add(StopWatchExecute.reset);
                                var gpxString =
                                    GpxWriter().asString(gpx, pretty: true);
                                saveActivity(gpxString);
                                Navigator.pop(context);
                              }
                            : null,
                        style: ButtonStyle(
                          backgroundColor: MaterialStateProperty.all<Color>(
                              const Color.fromARGB(255, 148, 133, 0)),
                        ),
                        icon: const Icon(Icons.save),
                        label: const Text('Save')),
                    const SizedBox(
                      height: 10.0, // height you want
                    ),
                  ],
                ))));
  }
}
