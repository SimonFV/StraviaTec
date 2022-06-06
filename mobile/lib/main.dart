import 'package:flutter/material.dart';
import 'package:mobile/database/database_manager.dart';
import 'package:mobile/screens/login/login.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  //creates the database
  await DatabaseManager.instance.database;

  //Run the interface
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({Key? key}) : super(key: key);

  // Root widget of application.
  @override
  Widget build(BuildContext context) {
    return const MaterialApp(
      title: 'StraviaTec',
      debugShowCheckedModeBanner: false,
      home: Login(),
    );
  }
}
