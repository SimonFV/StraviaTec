import 'package:mobile/database/user.dart';
import 'package:sqflite/sqflite.dart';
import 'dart:io';
import 'package:path/path.dart';
import 'package:path_provider/path_provider.dart';
import 'package:mobile/database/activity.dart';

import 'activity_type.dart';

class DatabaseManager {
  DatabaseManager._privateConstructor();
  static final DatabaseManager instance = DatabaseManager._privateConstructor();

  static Database? _database;
  Future<Database> get database async => _database ?? await _initDatabase();

  Future<Database> _initDatabase() async {
    Directory documentsDirectory = await getApplicationDocumentsDirectory();
    String path = join(documentsDirectory.path, 'mobileapp.db');
    return await openDatabase(
      path,
      version: 1,
      onCreate: _onCreate,
    );
  }

  Future _onCreate(Database db, int version) async {
    await db.execute('''CREATE TABLE USER(
        user TEXT PRIMARY KEY);
    ''');
    await db.execute('''CREATE TABLE ACTIVITY_TYPE(
	      name TEXT PRIMARY KEY);
    ''');
    await db.execute('''INSERT INTO ACTIVITY_TYPE(name) 
                        VALUES('Running');''');
    await db.execute('''INSERT INTO ACTIVITY_TYPE(name) 
                        VALUES('Swimming');''');
    await db.execute('''INSERT INTO ACTIVITY_TYPE(name) 
                        VALUES('Cycling');''');
    await db.execute('''INSERT INTO ACTIVITY_TYPE(name) 
                        VALUES('Hiking');''');
    await db.execute('''INSERT INTO ACTIVITY_TYPE(name) 
                        VALUES('Kayaking');''');
    await db.execute('''INSERT INTO ACTIVITY_TYPE(name) 
                        VALUES('Walking');''');
    await db.execute('''CREATE TABLE ACTIVITY(
        id INTEGER PRIMARY KEY,
        userId TEXT,
        route TEXT,
        distance DECIMAL(9,3),
        duration TIME,
	      start DATETIME,
	      type TEXT,
        status TEXT,
        FOREIGN KEY(userId) REFERENCES USER(user));
    ''');
  }

  Future<List<Activity>> getActivities() async {
    Database db = await instance.database;
    var activities = await db.query('ACTIVITY', orderBy: 'id');
    List<Activity> activityList = activities.isEmpty
        ? []
        : activities.map((c) => Activity.fromMap(c)).toList();
    return activityList;
  }

  Future<List<Activity>> getNewActivities() async {
    Database db = await instance.database;
    var activities =
        await db.query('ACTIVITY', where: "status = 'new'", orderBy: 'id');
    List<Activity> activityList = activities.isEmpty
        ? []
        : activities.map((c) => Activity.fromMap(c)).toList();
    return activityList;
  }

  Future<List<ActivityType>> getTypes() async {
    Database db = await instance.database;
    var types = await db.query('ACTIVITY_TYPE');
    List<ActivityType> typeList =
        types.isEmpty ? [] : types.map((c) => ActivityType.fromMap(c)).toList();
    return typeList;
  }

  Future<int> addActivity(Activity activity) async {
    Database db = await instance.database;
    return await db.insert('ACTIVITY', activity.toMap());
  }

  Future<int> addUser(User user) async {
    Database db = await instance.database;
    return await db.insert('USER', user.toMap());
  }

  Future<int> unSyncActivities() async {
    Database db = await instance.database;
    return await db.update('ACTIVITY', {"status": "notsynced"},
        where: "status = 'synced'");
  }

  Future<int> syncActivity(String userId, String start) async {
    Database db = await instance.database;
    return await db.update('ACTIVITY', {"status": "synced"},
        where: "userId = '" + userId + "' AND " + "start = '" + start + "'");
  }

  Future<int> deleteNotSyncedActivities() async {
    Database db = await instance.database;
    return await db.delete('ACTIVITY', where: "status = 'notsynced'");
  }

  Future<int> syncOldActivities() async {
    Database db = await instance.database;
    return await db.update('ACTIVITY', {"status": "synced"},
        where: "status = 'new'");
  }

  Future<bool> isTableEmpty() async {
    Database db = await instance.database;
    int? count = Sqflite.firstIntValue(
        await db.rawQuery('SELECT COUNT(*) FROM ACTIVITY'));
    return count == 0;
  }

  Future<bool> userExists(String user) async {
    Database db = await instance.database;
    var result =
        await db.rawQuery("SELECT user FROM USER WHERE user = '" + user + "';");
    return result.isNotEmpty;
  }

  Future<bool> activityExists(String userId, String start) async {
    Database db = await instance.database;
    var result = await db.rawQuery("SELECT id FROM ACTIVITY WHERE userId = '" +
        userId +
        "' AND " +
        "start = '" +
        start +
        "';");
    return result.isNotEmpty;
  }

  Future<int> getActivityId(String userId, String start) async {
    Database db = await instance.database;
    int? count = Sqflite.firstIntValue(await db.rawQuery(
        "SELECT id FROM ACTIVITY WHERE userId = '" +
            userId +
            "' AND " +
            "start = '" +
            start +
            "';"));
    return count!;
  }
}
