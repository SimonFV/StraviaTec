import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:mobile/database/database_manager.dart';
import 'package:mobile/database/activity.dart';
import 'package:mobile/database/user.dart';
import 'package:path_provider/path_provider.dart';
import '../run/run.dart';
import 'package:dio/dio.dart';

class Activities extends StatefulWidget {
  final User activeUser;
  final String token;

  const Activities({Key? key, required this.activeUser, required this.token})
      : super(key: key);

  @override
  _ActivitiesState createState() => _ActivitiesState();
}

class _ActivitiesState extends State<Activities> {
  final textController = TextEditingController();
  String errorMessage = "";
  bool synchronizing = false;
  var dio = Dio();

  Future<void> syncActivities() async {
    setState(() {
      errorMessage = "";
      synchronizing = true;
    });
    var activitiesToAdd = await DatabaseManager.instance.getNewActivities();

    Directory documentsDirectory = await getApplicationDocumentsDirectory();
    String path = documentsDirectory.path;

    for (var element in activitiesToAdd) {
      String id = (await DatabaseManager.instance
              .getActivityId(element.userId, element.start))
          .toString();
      String tempPath = path + "/" + id + ".gpx";
      var file = await MultipartFile.fromFile(tempPath, filename: id + ".gpx");
      var formData = FormData.fromMap({
        "UserId": element.userId,
        "Distance": element.distance.toString(),
        "Duration": "0001-01-01 " + element.duration,
        "Start": element.start,
        "Type": element.type,
        "Roc": 'Roc',
        "RocName": 'RocName',
        "Route": file
      });

      try {
        try {
          await dio.post("http://10.0.2.2:5000/User/addActivity",
              data: formData);
        } on DioError catch (e) {
          if (e.response!.statusCode! >= 400) {
            setState(() {
              errorMessage = e.response!.data;
              synchronizing = false;
            });
            return;
          }
        }
      } catch (err) {
        setState(() {
          errorMessage = "Could not connect to server.";
          synchronizing = false;
        });
        return;
      }
    }

    await DatabaseManager.instance.syncOldActivities();
    await DatabaseManager.instance.deleteFilesOfSyncedActivities();

    setState(() {
      synchronizing = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
            backgroundColor: const Color.fromARGB(193, 38, 240, 172),
            actions: <Widget>[
              Padding(
                  padding: const EdgeInsets.only(right: 20.0),
                  child: GestureDetector(
                    onTap: () async {
                      await syncActivities();
                    },
                    child: const Icon(
                      Icons.sync,
                      size: 26.0,
                    ),
                  )),
            ],
            title: Text(widget.activeUser.user == ""
                ? "Offline"
                : widget.activeUser.user)),
        body: AnnotatedRegion<SystemUiOverlayStyle>(
            value: SystemUiOverlayStyle.light,
            child: Stack(children: <Widget>[
              Container(
                  height: double.infinity,
                  width: double.infinity,
                  padding: const EdgeInsets.fromLTRB(10, 0, 10, 10),
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
                  child: SingleChildScrollView(
                      child: Column(
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: <Widget>[
                        const SizedBox(height: 20),
                        const Text('My Activities',
                            style: TextStyle(
                                color: Color.fromARGB(255, 0, 115, 161),
                                fontSize: 30,
                                fontWeight: FontWeight.bold)),
                        const SizedBox(height: 20),
                        SizedBox(
                          height: 300,
                          width: double.infinity,
                          child: synchronizing
                              ? const CircularProgressIndicator()
                              : FutureBuilder<List<Activity>>(
                                  future:
                                      DatabaseManager.instance.getActivities(),
                                  builder: (BuildContext context,
                                      AsyncSnapshot<List<Activity>> snapshot) {
                                    if (!snapshot.hasData) {
                                      return const Center(
                                          child: Text("Loading..."));
                                    }
                                    return ListView.builder(
                                      itemCount: snapshot.data!.toList().length,
                                      itemBuilder: (context, index) {
                                        return Card(
                                            child: Padding(
                                                padding:
                                                    const EdgeInsets.all(3.0),
                                                child: Column(
                                                  children: [
                                                    Row(
                                                      mainAxisAlignment:
                                                          MainAxisAlignment
                                                              .center,
                                                      children: [
                                                        Text(
                                                          snapshot.data![index]
                                                              .type,
                                                          style: const TextStyle(
                                                              fontSize: 18.0,
                                                              color: Color
                                                                  .fromARGB(
                                                                      255,
                                                                      0,
                                                                      99,
                                                                      99),
                                                              fontWeight:
                                                                  FontWeight
                                                                      .bold),
                                                        ),
                                                        const SizedBox(
                                                            width: 10),
                                                        Text(
                                                          snapshot.data![index]
                                                              .start,
                                                          style: const TextStyle(
                                                              fontSize: 16.0,
                                                              color: Color
                                                                  .fromARGB(
                                                                      255,
                                                                      87,
                                                                      0,
                                                                      99),
                                                              fontWeight:
                                                                  FontWeight
                                                                      .bold),
                                                        ),
                                                        const SizedBox(
                                                            width: 10),
                                                        snapshot.data![index]
                                                                    .status ==
                                                                "synced"
                                                            ? const Icon(
                                                                Icons.done,
                                                                color: Colors
                                                                    .green)
                                                            : const Icon(
                                                                Icons.sync,
                                                                color: Colors
                                                                    .black12)
                                                      ],
                                                    ),
                                                    const SizedBox(height: 6),
                                                    Text(
                                                      "Distance: " +
                                                          snapshot.data![index]
                                                              .distance
                                                              .toString() +
                                                          " km " +
                                                          "\t\t\t\tDuration: " +
                                                          snapshot.data![index]
                                                              .duration,
                                                      textAlign: TextAlign.left,
                                                    ),
                                                  ],
                                                )));
                                      },
                                    );
                                  }),
                        ),
                        const SizedBox(height: 15),
                        Text(errorMessage,
                            style: const TextStyle(
                                color: Color.fromARGB(207, 255, 73, 73),
                                fontSize: 14)),
                        const SizedBox(height: 15),
                        Container(
                            height: 60,
                            width: double.infinity,
                            decoration: BoxDecoration(
                                borderRadius: BorderRadius.circular(100),
                                gradient: const LinearGradient(colors: [
                                  Colors.blueAccent,
                                  Color.fromARGB(255, 86, 223, 156)
                                ])),
                            child: MaterialButton(
                                onPressed: () {
                                  Navigator.of(context)
                                      .push(MaterialPageRoute(
                                          builder: (context) => Run(
                                                activeUser: widget.activeUser,
                                              )))
                                      .then((_) => setState(() {}));
                                },
                                child: const Text('NEW ACTIVITY',
                                    style: TextStyle(
                                        fontSize: 25,
                                        color: Colors.white,
                                        fontWeight: FontWeight.bold))))
                      ])))
            ])));
  }
}
