class Activity {
  String userId;
  String route;
  double distance;
  String duration;
  String start;
  String type;
  String status;

  Activity(
      {required this.userId,
      required this.route,
      required this.distance,
      required this.duration,
      required this.start,
      required this.type,
      required this.status});

  factory Activity.fromMap(Map<String, dynamic> json) => Activity(
      userId: json['userId'],
      route: json['route'],
      distance: json['distance'],
      duration: json['duration'],
      start: json['start'],
      type: json['type'],
      status: json['status']);

  Map<String, dynamic> toMap() {
    return {
      'userId': userId,
      'route': route,
      'distance': distance,
      'duration': duration,
      'start': start,
      'type': type,
      'status': status,
    };
  }
}
