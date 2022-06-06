class ActivityType {
  String name;

  ActivityType({required this.name});

  factory ActivityType.fromMap(Map<String, dynamic> json) =>
      ActivityType(name: json['name']);

  Map<String, dynamic> toMap() {
    return {
      'name': name,
    };
  }
}
