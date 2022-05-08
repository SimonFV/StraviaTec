CREATE DATABASE StraviaTecDB;
GO

USE StraviaTecDB;
GO

CREATE TABLE "USER"
(
	"User"		VARCHAR(15)		NOT NULL,
	FirstName	VARCHAR(15)		NOT NULL,
	LastName1	VARCHAR(15)		NOT NULL,
	LastName2	VARCHAR(15)		NOT NULL,
	BirthDate	DATE			NOT NULL,
	"Password"	VARCHAR(20)		NOT NULL,
	Picture		VARCHAR(255),
	Nationality	VARCHAR(15),
	PRIMARY KEY("User")
);

CREATE TABLE FRIENDS
(
	"User"		VARCHAR(15)		NOT NULL,
	FriendUser	VARCHAR(15)		NOT NULL,
	PRIMARY KEY("User", FriendUser)
);

CREATE TABLE GROUPS
(
	AdminUser	VARCHAR(15)		NOT NULL,
	"Name"		VARCHAR(15)		NOT NULL,
	PRIMARY KEY("AdminUser", "Name")
);

CREATE TABLE ACTIVITY
(
	Id			INT				NOT NULL,
	UserId		VARCHAR(15)		NOT NULL,
	Distance	INT,
	Duration	TIME			NOT NULL,
	"Route"		VARCHAR(255),
	Altitude	INT,
	"Start"		DATETIME		NOT NULL,
	"Type"		VARCHAR(15)		NOT NULL,
	PRIMARY KEY(Id)
);

CREATE TABLE COMMENT
(
	"User"		VARCHAR(15)		NOT NULL,
	ActivityId	INT				NOT NULL,
	PostTime	DATETIME		NOT NULL,
	Body		TEXT			NOT NULL,
	PRIMARY KEY("User", ActivityId, PostTime)
);


CREATE TABLE ACTIVITY_TYPE
(
	"Name" 		VARCHAR(15)		NOT NULL,
	PRIMARY KEY("Name")
);
