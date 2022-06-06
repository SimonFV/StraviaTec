import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:mobile/database/database_manager.dart';
import 'package:mobile/database/user.dart';
import 'package:mobile/screens/activities/activities.dart';
import 'package:http/http.dart';
import 'package:intl/intl.dart';
import '../../database/activity.dart';

class Login extends StatefulWidget {
  const Login({Key? key}) : super(key: key);

  @override
  _LoginState createState() => _LoginState();
}

class _LoginState extends State<Login> {
  bool _obscurePassword = true;
  String errorMessage = "";
  String currentToken = "";
  final userName = TextEditingController();
  final userPassword = TextEditingController();

  @override
  void dispose() {
    // Clean up the controller when the widget is disposed.
    userName.dispose();
    userPassword.dispose();
    super.dispose();
  }

  void loginPost() async {
    dynamic jsonBody;
    if (userName.text.isEmpty || userPassword.text.isEmpty) {
      setState(() {
        errorMessage = "Empty field.";
      });
      return;
    }
    try {
      final response = await post(
          Uri.parse("http://10.0.2.2:5000/Authentication/login"),
          headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
          },
          body: jsonEncode(
              {"user": userName.text, "password": userPassword.text}));
      if (response.statusCode >= 400) {
        setState(() {
          errorMessage = response.body;
        });
        return;
      }
      jsonBody = jsonDecode(response.body);
      currentToken = jsonBody['token'];
    } catch (err) {
      setState(() {
        errorMessage = "Could not connect to server.";
      });
      return;
    }

    setState(() {
      errorMessage = "";
    });

    if (!(await DatabaseManager.instance.userExists(userName.text))) {
      await DatabaseManager.instance.addUser(User(user: userName.text));
    }

    //requests the activities for sincronization
    try {
      final response = await get(
          Uri.parse("http://10.0.2.2:5000/User/userActivities/sfv"),
          headers: {
            "Content-Type": "application/json",
            "Accept": "application/json",
            "Authentication": "Bearer " + currentToken
          });
      if (response.statusCode >= 400) {
        setState(() {
          errorMessage = response.body;
        });
        return;
      }
      jsonBody = jsonDecode(response.body);
    } catch (err) {
      setState(() {
        errorMessage = "Could not connect to server.";
      });
      return;
    }
    await DatabaseManager.instance.deleteFilesOfSyncedActivities();
    await DatabaseManager.instance.unSyncActivities();

    jsonBody.forEach((activity) async {
      if ((await DatabaseManager.instance.activityExists(
          activity["userId"], activity["start"].replaceAll("T", " ")))) {
        await DatabaseManager.instance.syncActivity(
            activity["userId"], activity["start"].replaceAll("T", " "));
      } else {
        await DatabaseManager.instance.addActivity(Activity(
            userId: activity["userId"],
            route: "default",
            distance: activity["distance"],
            duration: DateFormat('HH:mm:ss').format(DateTime(
                0,
                1,
                1,
                activity["duration"]["hours"],
                activity["duration"]["minutes"],
                activity["duration"]["seconds"],
                0,
                0)),
            start: activity["start"].replaceAll("T", " "),
            type: activity["type"],
            status: "synced"));
      }
    });

    await DatabaseManager.instance.deleteNotSyncedActivities();

    Navigator.of(context).push(MaterialPageRoute(
        builder: (context) => Activities(
            activeUser: User(user: userName.text), token: currentToken)));
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        body: AnnotatedRegion<SystemUiOverlayStyle>(
            value: SystemUiOverlayStyle.light,
            child: Stack(children: <Widget>[
              Container(
                  height: double.infinity,
                  width: double.infinity,
                  padding: const EdgeInsets.all(30),
                  decoration: const BoxDecoration(
                      gradient: LinearGradient(
                          begin: Alignment.topCenter,
                          end: Alignment.bottomCenter,
                          colors: [
                        Color.fromARGB(244, 192, 255, 224),
                        Color.fromARGB(170, 202, 240, 255),
                        Color.fromARGB(170, 202, 240, 255),
                        Color.fromARGB(170, 79, 155, 255),
                      ])),
                  child: SingleChildScrollView(
                      child: Column(
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: <Widget>[
                        const SizedBox(height: 25),
                        const Text('StraviaTEC',
                            style: TextStyle(
                                color: Color.fromARGB(255, 0, 158, 137),
                                fontSize: 40,
                                fontWeight: FontWeight.bold)),
                        const SizedBox(height: 25),
                        TextFormField(
                            controller: userName,
                            keyboardType: TextInputType.text,
                            decoration: const InputDecoration(
                                labelText: "User",
                                border: OutlineInputBorder(),
                                prefixIcon: Icon(Icons.person))),
                        const SizedBox(height: 15),
                        TextFormField(
                            controller: userPassword,
                            keyboardType: TextInputType.visiblePassword,
                            obscureText: _obscurePassword,
                            decoration: InputDecoration(
                                labelText: "Password",
                                border: const OutlineInputBorder(),
                                prefixIcon: const Icon(Icons.lock),
                                suffixIcon: IconButton(
                                  onPressed: () {
                                    setState(() {
                                      _obscurePassword = !_obscurePassword;
                                    });
                                  },
                                  icon: _obscurePassword
                                      ? const Icon(Icons.visibility)
                                      : const Icon(Icons.visibility_off),
                                ))),
                        const SizedBox(height: 25),
                        Container(
                            height: 60,
                            width: double.infinity,
                            decoration: BoxDecoration(
                                borderRadius: BorderRadius.circular(100),
                                gradient: const LinearGradient(colors: [
                                  Colors.blueAccent,
                                  Colors.greenAccent
                                ])),
                            child: MaterialButton(
                                onPressed: () {
                                  setState(() {
                                    errorMessage = "";
                                  });
                                  loginPost();
                                },
                                child: const Text('LOGIN',
                                    style: TextStyle(
                                        fontSize: 25,
                                        color: Colors.white,
                                        fontWeight: FontWeight.bold)))),
                        const SizedBox(height: 15),
                        Text(errorMessage,
                            style: const TextStyle(
                                color: Color.fromARGB(207, 255, 73, 73),
                                fontSize: 14)),
                        const SizedBox(height: 20),
                        Container(
                            height: 40,
                            width: double.infinity,
                            decoration: BoxDecoration(
                                borderRadius: BorderRadius.circular(100),
                                gradient: const LinearGradient(colors: [
                                  Color.fromARGB(255, 124, 172, 255),
                                  Color.fromARGB(255, 122, 240, 183)
                                ])),
                            child: MaterialButton(
                                onPressed: () async {
                                  setState(() {
                                    errorMessage = "";
                                  });
                                  if (userName.text.isEmpty ||
                                      !(await DatabaseManager.instance
                                          .userExists(userName.text))) {
                                    setState(() {
                                      errorMessage =
                                          "Offline mode can only be used with\nan already logged in user.";
                                    });
                                    return;
                                  }
                                  Navigator.of(context).push(MaterialPageRoute(
                                      builder: (context) => Activities(
                                          activeUser: User(user: userName.text),
                                          token: "")));
                                },
                                child: const Text('OFFLINE MODE',
                                    style: TextStyle(
                                        fontSize: 18,
                                        color: Colors.white,
                                        fontWeight: FontWeight.bold))))
                      ])))
            ])));
  }
}
