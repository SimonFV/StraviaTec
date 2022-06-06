class User {
  String user;

  User({required this.user});

  factory User.fromMap(Map<String, dynamic> json) => User(user: json['user']);

  Map<String, dynamic> toMap() {
    return {'user': user};
  }
}
