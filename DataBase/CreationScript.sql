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
	Id			INT				NOT NULL	IDENTITY(1,1),
	UserId		VARCHAR(15)		NOT NULL,
	Distance	INT,
	Duration	TIME			NOT NULL,
	"Route"		VARCHAR(255),
	Altitude	INT,
	"Start"		DATETIME		NOT NULL,
	"Type"		VARCHAR(15)		NOT NULL,
	PRIMARY KEY(Id)
);


CREATE TABLE ACTIVITY_TYPE
(
	"Name" 		VARCHAR(15)		NOT NULL,
	PRIMARY KEY("Name")
);

CREATE Table CHALLENGE
(
    Id  		INT 			NOT NULL	IDENTITY(1,1),
    "Name" 		VARCHAR(15) 	NOT NULL,
    Class 		VARCHAR(15) , 
    Privacy 	VARCHAR(15),
    StartDate 	DATE 			NOT NULL,
    EndDate 	DATE 			NOT NULL,
    PRIMARY KEY(Id)
);

CREATE TABLE CHALLENGE_PARTICIPANTS
(
    "User" 		VARCHAR(15) 	NOT NULL,
    ChallengeId INT 			NOT NULL,
    ActivityId 	INT 			NOT NULL,
    PRIMARY KEY("User", ChallengeId,ActivityId)
);

CREATE TABLE CHALLENGE_VISIBILITY
(
    "Admin"  	VARCHAR(15) 	NOT NULL,
    "Group" 	VARCHAR(15),
    ChallengeId INT 			NOT NULL,
    PRIMARY KEY("Admin","Group", ChallengeID)
);

CREATE TABLE CHALLENGE_SPONSORS
(
    ChallengeId INT 			NOT NULL,
    TradeName 	VARCHAR(15) 	NOT NULL,
    PRIMARY KEY(ChallengeId,TradeName)
);

CREATE TABLE SPONSOR
(
    TradeName 		VARCHAR(15) 	NOT NULL,
    Representative 	VARCHAR(15) 	NOT NULL,
    ReprPhone 		INT 			NOT NULL,
    Logo 			VARCHAR(255),
    PRIMARY KEY(TradeName)
);