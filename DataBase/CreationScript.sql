CREATE DATABASE StraviaTecDB;
GO

USE StraviaTecDB;
GO

CREATE TABLE "USER"
(
	"User"		NVARCHAR(15)	NOT NULL,
	FirstName	NVARCHAR(15)	NOT NULL,
	LastName1	NVARCHAR(15)	NOT NULL,
	LastName2	NVARCHAR(15)	NOT NULL,
	BirthDate	DATE			NOT	NULL,
	"Password"	BINARY(64)		NOT NULL,
	Picture		NVARCHAR(255),
	Nationality	NVARCHAR(15),
	PRIMARY KEY("User")
);

CREATE TABLE FRIENDS
(
	"User"		NVARCHAR(15)	NOT NULL,
	FriendUser	NVARCHAR(15)	NOT NULL,
	PRIMARY KEY("User", FriendUser)
);

CREATE TABLE GROUPS
(
	Id			INT				NOT NULL	IDENTITY(1,1),
	AdminUser	NVARCHAR(15)	NOT NULL,
	"Name"		NVARCHAR(15)	NOT NULL,
	PRIMARY KEY(Id)
);

CREATE TABLE GROUP_USERS
(
	GroupId		INT				NOT NULL,
	"User"		NVARCHAR(15)	NOT NULL,
	PRIMARY KEY(GroupId, "User")
);

CREATE TABLE ACTIVITY
(
	Id			INT				NOT NULL	IDENTITY(1,1),
	UserId		NVARCHAR(15)	NOT NULL,
	Distance	DECIMAL(9,3)	NOT NULL,
	Duration	TIME			NOT NULL,
	"Route"		NVARCHAR(255),
	"Start"		DATETIME		NOT NULL,
	"Type"		NVARCHAR(15)	NOT NULL,
	PRIMARY KEY(Id)
);

CREATE TABLE ACTIVITY_TYPE
(
	"Name" 		NVARCHAR(15)	NOT NULL,
	PRIMARY KEY("Name")
);

CREATE Table CHALLENGE
(
    Id  		INT 			NOT NULL	IDENTITY(1,1),
	UserAdmin	NVARCHAR(15)	NOT NULL,
    "Name" 		NVARCHAR(15) 	NOT NULL,
    Class 		VARCHAR(15), 
    Privacy 	BIT,
    StartDate 	DATE 			NOT NULL,
    EndDate 	DATE 			NOT NULL,
	Activity_Type	NVARCHAR(15),
	Objective	DECIMAL(9,3)
    PRIMARY KEY(Id)
);

CREATE TABLE CHALLENGE_PARTICIPANTS
(
    "User" 		NVARCHAR(15) 	NOT NULL,
    ChallengeId INT 			NOT NULL,
    PRIMARY KEY("User", ChallengeId)
);

CREATE TABLE CHALLENGE_ACTIVITIES
(
    ChallengeId INT 			NOT NULL,
	ActivityId	INT				NOT NULL,
    PRIMARY KEY(ChallengeId, ActivityId)
);

CREATE TABLE CHALLENGE_VISIBILITY
(
    GroupId		INT			NOT NULL,
    ChallengeId INT 		NOT NULL,
    PRIMARY KEY(GroupId	, ChallengeID)
);

CREATE TABLE CHALLENGE_SPONSORS
(
    ChallengeId INT 			NOT NULL,
    TradeName 	NVARCHAR(15) 	NOT NULL,
    PRIMARY KEY(ChallengeId,TradeName)
);

CREATE TABLE SPONSOR
(
    TradeName 		NVARCHAR(15) 	NOT NULL,
    Representative 	NVARCHAR(15) 	NOT NULL,
    ReprPhone 		INT 			NOT NULL,
    Logo 			NVARCHAR(255),
    PRIMARY KEY(TradeName)
);

CREATE TABLE CATEGORY
(
    "Name"			NVARCHAR(15) 	NOT NULL,
    "Description" 	NVARCHAR(70) 	NOT NULL,
    PRIMARY KEY("Name")
);

CREATE TABLE RACE (
	Id				INT				NOT NULL	IDENTITY(1,1),
	UserAdmin		NVARCHAR(15)	NOT NULL,
	"Name"			NVARCHAR(15) 	NOT NULL,
	"Route"			NVARCHAR(225) 	NOT NULL,
	"Cost"			DECIMAL(9,3) 	NOT NULL,
	Privacy			BIT 			NOT NULL,
	StartDate		DATETIME		NOT NULL,
	Category		NVARCHAR(1000) 	NOT NULL,
	"Type"			NVARCHAR(15) 	NOT NULL,
	PRIMARY KEY(Id)
);

CREATE TABLE RACE_VISIBILITY
(
    GroupId			INT 			NOT NULL,
    RaceId	 		INT 			NOT NULL,
    PRIMARY KEY(GroupId, RaceId)
);

CREATE TABLE RACE_SPONSORS
(
    RaceId			INT			 	NOT NULL,
    TradeName	 	NVARCHAR(15) 	NOT NULL,
    PRIMARY KEY(RaceId, TradeName)
);

CREATE TABLE BANK_ACCOUNTS
(
    RaceId			INT			 	NOT NULL,
    BankAccount 	NVARCHAR(15) 	NOT NULL,
    PRIMARY KEY(BankAccount, RaceId)
);

CREATE Table RACE_PARTICIPANTS
(
    "User"  		NVARCHAR(15)	NOT NULL,
    RaceId 			INT 			NOT NULL,
    Payment 		NVARCHAR(255),
    "Status" 		NVARCHAR(15)	NOT NULL,
    ActivityId 		INT,
	CategoryName	NVARCHAR(15) 	NOT NULL,
    PRIMARY KEY(RaceId, "User")
);

CREATE Table RACE_CATEGORY
(
    Raceid			INT 			NOT NULL,
    CategoryName	NVARCHAR(15) 	NOT NULL,
    PRIMARY KEY(RaceId, CategoryName)
);
GO


----------------------------------------------
--				FOREIGN KEYS				--
----------------------------------------------


ALTER TABLE FRIENDS
ADD CONSTRAINT FK_FRIENDS_USER FOREIGN KEY ("User")
REFERENCES "USER"("User");

ALTER TABLE FRIENDS
ADD CONSTRAINT FK_FRIENDS_USERFRIEND FOREIGN KEY (FriendUser)
REFERENCES "USER"("User");

ALTER TABLE GROUPS
ADD CONSTRAINT FK_GROUP_ADMINUSER FOREIGN KEY (AdminUser)
REFERENCES "USER"("User");

ALTER TABLE GROUP_USERS
ADD CONSTRAINT FK_GROUPUSERS_GROUPID FOREIGN KEY (GroupId)
REFERENCES GROUPS(Id);

ALTER TABLE GROUP_USERS
ADD CONSTRAINT FK_GROUPUSERS_USER FOREIGN KEY ("User")
REFERENCES "USER"("User");

ALTER TABLE ACTIVITY
ADD CONSTRAINT FK_ACTIVITY_USER FOREIGN KEY (UserId)
REFERENCES "USER"("User");

ALTER TABLE ACTIVITY
ADD CONSTRAINT FK_ACTIVITY_TYPE FOREIGN KEY ("Type")
REFERENCES ACTIVITY_TYPE("Name");

ALTER TABLE CHALLENGE_VISIBILITY
ADD CONSTRAINT FK_CHALLENGE_VIS_GROUPID FOREIGN KEY(GroupId)
REFERENCES GROUPS(Id);

ALTER TABLE CHALLENGE_VISIBILITY
ADD CONSTRAINT FK_CHALLENGE_VIS_CHALLENGEID FOREIGN KEY(ChallengeId)
REFERENCES CHALLENGE(Id);

ALTER TABLE CHALLENGE
ADD CONSTRAINT FK_CHALLENGE_USERADMIN FOREIGN KEY (UserAdmin)
REFERENCES "USER"("User");

ALTER TABLE CHALLENGE
ADD CONSTRAINT FK_CHALLENGE_ACTIVITY_TYPE FOREIGN KEY (Activity_type)
REFERENCES ACTIVITY_TYPE("Name");

ALTER TABLE CHALLENGE_PARTICIPANTS
ADD CONSTRAINT FK_CHALLENGE_PART_USER FOREIGN KEY ("User")
REFERENCES "USER"("User");

ALTER TABLE CHALLENGE_PARTICIPANTS
ADD CONSTRAINT FK_CHALLENGE_PART_CHALLENGEID FOREIGN KEY (ChallengeId)
REFERENCES CHALLENGE(Id);

ALTER TABLE CHALLENGE_ACTIVITIES
ADD CONSTRAINT FK_CHA_ACT_CHALLENGEID FOREIGN KEY (ChallengeId)
REFERENCES CHALLENGE(Id);

ALTER TABLE CHALLENGE_ACTIVITIES
ADD CONSTRAINT FK_CHA_ACT_ACTIVITYID FOREIGN KEY (ActivityId)
REFERENCES ACTIVITY(Id);

ALTER TABLE CHALLENGE_SPONSORS
ADD CONSTRAINT FK_CHALLENGE_SPON_CHALLENGEID FOREIGN KEY(ChallengeId)
REFERENCES CHALLENGE(Id);

ALTER TABLE CHALLENGE_SPONSORS
ADD CONSTRAINT FK_CHALLENGE_SPON_TRADE FOREIGN KEY(TradeName)
REFERENCES SPONSOR(TradeName);

ALTER TABLE RACE_VISIBILITY
ADD CONSTRAINT FK_RACE_GROUP_ID FOREIGN KEY(GroupId)
REFERENCES GROUPS(Id);

ALTER TABLE RACE_VISIBILITY
ADD CONSTRAINT FK_RACE_VIS_ID FOREIGN KEY(RaceId)
REFERENCES RACE(Id);

ALTER TABLE RACE
ADD CONSTRAINT FK_USER_RACE_ADMIN FOREIGN KEY(UserAdmin)
REFERENCES "USER"("User");

ALTER TABLE RACE
ADD CONSTRAINT FK_TYPE_NAME FOREIGN KEY("Type")
REFERENCES ACTIVITY_TYPE("Name");

ALTER TABLE RACE_PARTICIPANTS
ADD CONSTRAINT FK_USER_PARTI_NAME FOREIGN KEY("User")
REFERENCES "USER"("User");

ALTER TABLE RACE_PARTICIPANTS
ADD CONSTRAINT FK_RACE_PARTI_ID FOREIGN KEY(RaceId)
REFERENCES RACE(Id);

ALTER TABLE RACE_PARTICIPANTS
ADD CONSTRAINT FK_ACTIVITY_ID FOREIGN KEY(ActivityId)
REFERENCES ACTIVITY(Id);

ALTER TABLE RACE_SPONSORS
ADD CONSTRAINT FK_RACE_SPON_ID FOREIGN KEY(RaceId)
REFERENCES RACE(Id);

ALTER TABLE RACE_SPONSORS
ADD CONSTRAINT FK_TRADE_RACE_NAME FOREIGN KEY(TradeName)
REFERENCES SPONSOR(TradeName);

ALTER TABLE BANK_ACCOUNTS
ADD CONSTRAINT FK_RACE_BANK_ID FOREIGN KEY(RaceId)
REFERENCES RACE(Id);

ALTER TABLE RACE_CATEGORY
ADD CONSTRAINT FK_RACE_CATEGORY_ID FOREIGN KEY(RaceId)
REFERENCES RACE(Id);

ALTER TABLE RACE_CATEGORY
ADD CONSTRAINT FK_RACE_CATEGORY_NAME FOREIGN KEY(CategoryName)
REFERENCES CATEGORY("Name");
GO



----------------------------------------------
--			 STORED PROCEDURES				--
----------------------------------------------


/*
	Users registration. Checks if the User already exists and saves the Password with hash.
*/
CREATE PROCEDURE Register
	@User NVARCHAR(15), @FirstName NVARCHAR(15), @LastName1 NVARCHAR(15), @LastName2 NVARCHAR(15),
	@BirthDate DATE, @Password NVARCHAR(30), @Picture NVARCHAR(255), @Nationality VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
     
    IF EXISTS(SELECT "User" FROM "USER" WHERE "User" = @User)
    BEGIN
        SELECT -1  --User already exists
    END
    ELSE
    BEGIN
        INSERT INTO "USER"
                ("User", FirstName, LastName1, LastName2, BirthDate,
				"Password", Picture, Nationality)
        VALUES (
				@User, @FirstName, @LastName1, @LastName2, @BirthDate,
				HASHBYTES('SHA2_512', @Password), @Picture, @Nationality)
        SELECT 0 --User registered
    END
END;
GO

CREATE PROCEDURE LoginUser
	@User NVARCHAR(15),
	@Password NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
     
    IF NOT EXISTS(SELECT "User" FROM "USER" WHERE "User" = @User)
    BEGIN
        SELECT -1  --User not found
    END
    ELSE IF NOT EXISTS(SELECT "User" FROM "USER" WHERE "User" = @User AND "Password" = HASHBYTES('SHA2_512', @Password))
    BEGIN
        SELECT -2 --Incorrect password
    END
	ELSE
	BEGIN
		SELECT 0 --User logged in
	END
END;
GO

CREATE PROCEDURE UpdateUser
	@User NVARCHAR(15), @FirstName NVARCHAR(15), @LastName1 NVARCHAR(15), @LastName2 NVARCHAR(15),
	@BirthDate DATE, @Password NVARCHAR(30), @NewPassword NVARCHAR(30), @Picture NVARCHAR(255), @Nationality VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
     
    IF NOT EXISTS(SELECT "User" FROM "USER" WHERE "User" = @User)
    BEGIN
        SELECT -1  --User not found
    END
    ELSE IF NOT EXISTS(SELECT "User" FROM "USER" WHERE "User" = @User AND "Password" = HASHBYTES('SHA2_512', @Password))
    BEGIN
        SELECT -2 --Incorrect password
    END
    ELSE
    BEGIN
        UPDATE "USER"
        SET		FirstName = @FirstName, 
				LastName1 = @LastName1, 
				LastName2 = @LastName2, 
				BirthDate = @BirthDate,
				"Password" = HASHBYTES('SHA2_512', @NewPassword),
				Picture = @Picture, 
				Nationality = @Nationality
		WHERE	"User" = @User;

        SELECT 0 --User updated
    END
END;
GO

CREATE PROCEDURE FriendsLatestActivities
	@User NVARCHAR(15)
AS
BEGIN
    SELECT * FROM LATEST_ACTIVITY 
	WHERE EXISTS(
				SELECT FriendUser FROM FRIENDS 
				WHERE LATEST_ACTIVITY."User" = FRIENDS.FriendUser AND FRIENDS."User" = @User)
END;
GO

CREATE PROCEDURE FriendsAvailable
	@User NVARCHAR(15)
AS
BEGIN
    SELECT "User", FirstName, LastName1, LastName2, Nationality
	FROM "USER" 
	WHERE "USER"."User" NOT IN(
		SELECT FRIENDS.FriendUser FROM FRIENDS 
		WHERE "User" = @User)
		AND "USER"."User" != @User;
END;
GO

CREATE PROCEDURE getFriends
	@User NVARCHAR(15)
AS
BEGIN
    SELECT "User", FirstName, LastName1, BirthDate,Picture
	FROM "USER" 
	WHERE "USER"."User" IN(
		SELECT FRIENDS.FriendUser FROM FRIENDS 
		WHERE "User" = @User)
		AND "USER"."User" != @User;
END;
GO

CREATE PROCEDURE GroupsAvailable
	@User NVARCHAR(15)
AS
BEGIN
    SELECT * FROM GROUPS
	WHERE Id NOT IN(
		SELECT GROUP_USERS.GroupId FROM GROUP_USERS 
		WHERE GROUP_USERS."User" = @User)
		AND AdminUser != @User;
END;
GO

CREATE PROCEDURE getGroups
	@User NVARCHAR(15)
AS
BEGIN
    SELECT * FROM GROUPS
	WHERE Id IN(
		SELECT GROUP_USERS.GroupId FROM GROUP_USERS 
		WHERE GROUP_USERS."User" = @User)
		AND AdminUser != @User;
END;
GO

CREATE PROCEDURE AddActivity
	@UserId	NVARCHAR(15), @Distance DECIMAL(9,3), @Duration TIME, @Route NVARCHAR(255),
	@Start DATETIME, @Type	NVARCHAR(15), @RoC NVARCHAR(15), @RoCName NVARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
     
    IF EXISTS(SELECT UserId FROM ACTIVITY WHERE UserId = @UserId AND 
												"Start" < @Start + CAST(@Duration AS datetime) AND
												"Start" + CAST(Duration AS datetime) > @Start)
    BEGIN
        SELECT -1  --An Activity at that time already in the database
    END
	ELSE IF GETDATE() < @Start + CAST(@Duration AS datetime)
	BEGIN
        SELECT -2  --The time and duration of the activity doesn't match with the current time
    END
    ELSE
	IF (@RoC =  'Race')
		BEGIN
			IF EXISTS(Select Id FROM RACE, RACE_PARTICIPANTS WHERE RACE."Name" = @RoCName AND RACE_PARTICIPANTS."User" = @UserId AND RACE_PARTICIPANTS.RaceId = RACE.Id)
				BEGIN
					INSERT INTO ACTIVITY
							(UserId, Distance, Duration, "Route", 
							"Start", "Type")
					VALUES (@UserId, @Distance, @Duration, @Route, 
							@Start, @Type);
					UPDATE RACE_PARTICIPANTS
					SET ActivityId = (SELECT Id FROM ACTIVITY WHERE UserId = @UserId And "Start" = @Start)
					SELECT 0 --Activity added
				END
			ELSE
				BEGIN
					SELECT -3  -- Race not exists or User isn't registed
				END
		END
	ELSE IF (@RoC = 'Challenge')
		BEGIN
			IF EXISTS(Select Id FROM CHALLENGE, CHALLENGE_PARTICIPANTS WHERE CHALLENGE."Name" = @RoCName AND CHALLENGE_PARTICIPANTS."User" = @UserId AND CHALLENGE_PARTICIPANTS.ChallengeId = CHALLENGE.Id)
				BEGIN
					INSERT INTO ACTIVITY
							(UserId, Distance, Duration, "Route", 
							"Start", "Type")
					VALUES (@UserId, @Distance, @Duration, @Route, 
							@Start, @Type);
					SELECT 0 --Activity added
				END
			ELSE
				BEGIN
					SELECT -4  -- Challenge not exists or User isn't registed
				END
		END
	ELSE
			INSERT INTO ACTIVITY
					(UserId, Distance, Duration, "Route", 
					"Start", "Type")
			VALUES (@UserId, @Distance, @Duration, @Route, 
					@Start, @Type);
			SELECT 0 --Activity added
END;
GO

CREATE PROCEDURE ChallengeGroups
	@Groups varchar(1000),
	@ChallengeName NVARCHAR(15)
AS
	DECLARE @Position INT
	DECLARE @Group varchar (1000)
	SET @Groups=@Groups+','
	WHILE PATINDEX('%,%', @Groups)<>0
		BEGIN
			SELECT @Position=PATINDEX('%,%', @Groups)
			SELECT @Group = left(@Groups, @Position - 1)

			INSERT INTO CHALLENGE_VISIBILITY(GroupId,ChallengeId)
			VALUES (
				(SELECT Id FROM GROUPS WHERE "Name" = @Group),
				(SELECT Id FROM CHALLENGE WHERE "Name" = @ChallengeName))
			SELECT @Groups=STUFF(@Groups, 1, @Position, '')
		END
GO

CREATE PROCEDURE RegisterChallenge
	@User NVARCHAR(15),
	@Name NVARCHAR(15),
	@Class NVARCHAR(15),
	@Privacy BIT,
	@Groups VARCHAR(1000),
	@StartDate DATE,
	@EndDate DATE,
	@Activity_Type NVARCHAR(15),
	@Objective	DECIMAL(9,3)
AS
BEGIN 
	SET NOCOUNT ON;
	IF EXISTS(SELECT "Name" FROM CHALLENGE WHERE "Name" = @Name)
		BEGIN
			SELECT -1  --Challenge already exists
		END
	ELSE IF NOT EXISTS(SELECT "Name" FROM ACTIVITY_TYPE WHERE "Name"=@Activity_Type)
		BEGIN
			SELECT -2 --Not Type Found
		END

	ELSE
	BEGIN
		INSERT INTO CHALLENGE
				(UserAdmin,
				"Name",
				Class,
				Privacy,
				StartDate,
				EndDate,
				Activity_Type,
				Objective
				)
		VALUES(@User,
				@Name,
				@Class,
				@Privacy,
				@StartDate,
				@EndDate,
				@Activity_Type,
				@Objective
				)
		SELECT 0 --Challenge registered
	END

	IF (@Privacy=1)
		BEGIN
			EXEC ChallengeGroups @Groups=@Groups,
								 @Challengename=@Name;
		END
END;
GO

CREATE PROCEDURE RaceGroups
	@Groups varchar(1000),
	@RaceName NVARCHAR(15)
AS
	DECLARE @Position INT
	DECLARE @Group varchar (1000)
	SET @Groups=@Groups+','
	WHILE PATINDEX('%,%', @Groups)<>0
		BEGIN
			SELECT @Position=PATINDEX('%,%', @Groups)
			SELECT @Group = left(@Groups, @Position - 1)

			INSERT INTO RACE_VISIBILITY(GroupId,RaceId)
			VALUES (
				(SELECT Id FROM GROUPS WHERE "Name" = @Group),
				(SELECT Id FROM RACE WHERE "Name" = @RaceName))
			SELECT @Groups=STUFF(@Groups, 1, @Position, '')
		END;
GO

CREATE PROCEDURE RaceCategories
	@Categories varchar(1000),
	@RaceName NVARCHAR(15)
AS
	DECLARE @Position INT
	DECLARE @Category varchar (1000)
	SET @Categories=@Categories+','
	WHILE PATINDEX('%,%', @Categories)<>0
		BEGIN
			SELECT @Position=PATINDEX('%,%', @Categories)
			SELECT @Category = left(@Categories, @Position - 1)

			INSERT INTO RACE_CATEGORY(CategoryName,RaceId)
			VALUES (
				(SELECT "Name" FROM CATEGORY WHERE "Name" = @Category),
				(SELECT Id FROM RACE WHERE "Name" = @RaceName))
			SELECT @Categories=STUFF(@Categories, 1, @Position, '')
		END;
GO

CREATE PROCEDURE RegisterRace
	@Admin NVARCHAR(15),
	@Name NVARCHAR(15),
	@Route NVARCHAR(225),
	@Cost DECIMAL(9,3),
	@Privacy BIT,
	@Groups VARCHAR(1000),
	@StartDate DATE,
	@Category NVARCHAR(1000),
	@Type NVARCHAR(15)
AS
BEGIN 
	SET NOCOUNT ON;
	IF EXISTS(SELECT "Name" FROM RACE WHERE "Name" = @Name)
		BEGIN
			SELECT -1  -- Race already exists
		END
	ELSE IF NOT EXISTS(SELECT "Name" FROM ACTIVITY_TYPE WHERE "Name"=@Type)
		BEGIN
			SELECT -2 -- Not Type Found
		END

	ELSE
	BEGIN
		INSERT INTO Race
				(UserAdmin,
				"Name",
				"Route",
				"Cost",
				Privacy,
				StartDate,
				Category,
				"Type"
				)
		VALUES(@Admin,
				@Name,
				@Route,
				@Cost,
				@Privacy,
				@StartDate,
				@Category,
				@Type
				)
		SELECT 0 -- Race registered
	END

	BEGIN
		EXEC RaceCategories @Categories=@Category,
							@Racename=@Name;
	END

	IF (@Privacy=1)
		BEGIN
			EXEC RaceGroups @Groups=@Groups,
						    @Racename=@Name;
		END
END;
GO

CREATE PROCEDURE Register_in_Race
	@User NVARCHAR(15),
	@RaceName NVARCHAR(25),
	@Category NVARCHAR(15)
AS
BEGIN 
	SET NOCOUNT ON;
	IF EXISTS(SELECT "USER".FirstName FROM "USER", RACE_PARTICIPANTS WHERE RACE_PARTICIPANTS."User" = @User)
		BEGIN
			SELECT -1  -- Is already register
		END
	
	ELSE
	BEGIN
		INSERT INTO RACE_PARTICIPANTS
				("User",
				"RaceId",
				"Status",
				CategoryName
				)
		VALUES(@User,
				(SELECT Id FROM RACE WHERE "Name" = @RaceName),
				'Unpaid',
				@Category
				)
		SELECT 0 -- Race registered
	END

END;
GO

CREATE PROCEDURE PARTICIPANTS_IN_RACE
	@RaceName NVARCHAR(25)
AS
BEGIN
    SET NOCOUNT ON;
     
    IF NOT EXISTS(SELECT Id FROM RACE WHERE "Name" = @RaceName)
    BEGIN
        SELECT -1  -- Race not found
    END
    
	ELSE
	BEGIN
		Select FirstName, LastName, Age, CategoryName FROM PARTICIPANTS_PER_RACE, RACE WHERE RACE."Name" = @RaceName AND RACE.Id = PARTICIPANTS_PER_RACE.RaceId ORDER BY CategoryName;
	END
END;
GO

CREATE PROCEDURE RECORD_IN_RACE
	@RaceName NVARCHAR(25)
AS
BEGIN
    SET NOCOUNT ON;
     
    IF NOT EXISTS(SELECT Id FROM RACE WHERE "Name" = @RaceName)
    BEGIN
        SELECT -1  -- Race not found
    END
    
	ELSE
	BEGIN
		Select FirstName, LastName, Age, Duration, CategoryName FROM RECORD_PER_RACE, RACE WHERE RACE."Name" = @RaceName AND RACE.Id = RECORD_PER_RACE.RaceId ORDER BY CategoryName;
	END
END;
GO

----------------------------------------------
--					VIEWS					--
----------------------------------------------


CREATE VIEW LATEST_ACTIVITY AS
SELECT "User", FirstName, LastName1, LastName2, Id, "Type", "Start", "Route", Distance
FROM "USER", ACTIVITY 
WHERE "User" = UserId AND "Start" = (SELECT MAX("Start") FROM ACTIVITY WHERE UserId = "User");
GO

CREATE VIEW PARTICIPANTS_PER_RACE AS
SELECT "USER".FirstName, CONCAT("USER".LastName1, ' ', "USER".LastName2) AS LastName, (0 + Convert(Char(8),GETDATE(),112) - Convert(Char(8),"USER".BirthDate,112)) / 10000 AS Age, RACE_PARTICIPANTS.RaceId, RACE_PARTICIPANTS.CategoryName
FROM "USER", RACE_PARTICIPANTS
WHERE "USER"."User" = RACE_PARTICIPANTS."User";
GO

CREATE VIEW RECORD_PER_RACE AS
SELECT "USER".FirstName, CONCAT("USER".LastName1, ' ', "USER".LastName2) AS LastName, (0 + Convert(Char(8),GETDATE(),112) - Convert(Char(8),"USER".BirthDate,112)) / 10000 AS Age, RACE_PARTICIPANTS.RaceId, ACTIVITY.Duration, RACE_PARTICIPANTS.CategoryName
FROM "USER", RACE_PARTICIPANTS, ACTIVITY
WHERE "USER"."User" = RACE_PARTICIPANTS."User" AND ACTIVITY.Id = RACE_PARTICIPANTS.ActivityId;
GO

----------------------------------------------
--					TRIGGERS				--
----------------------------------------------

-- TRIGGERS FOR DELETION

CREATE TRIGGER UserDeletedTrigger
    ON "USER"
    INSTEAD OF DELETE
AS
BEGIN
    DELETE FROM ACTIVITY
    WHERE UserId IN(SELECT deleted."User" FROM deleted)

	DELETE FROM GROUP_USERS
    WHERE "User" IN(SELECT deleted."User" FROM deleted)

	DELETE FROM FRIENDS
    WHERE "User" IN(SELECT deleted."User" FROM deleted) OR FriendUser IN(SELECT deleted."User" FROM deleted)

	DELETE FROM CHALLENGE_PARTICIPANTS
    WHERE "User" IN(SELECT deleted."User" FROM deleted)

	DELETE FROM GROUPS
	WHERE AdminUser IN(SELECT deleted."User" FROM deleted)

	DELETE FROM RACE
    WHERE UserAdmin IN(SELECT deleted."User" FROM deleted)

	DELETE FROM CHALLENGE
    WHERE UserAdmin IN(SELECT deleted."User" FROM deleted)

	DELETE FROM "USER"
    WHERE "User" = (SELECT deleted."User" FROM deleted)
END;
GO

CREATE TRIGGER ActivityDeletedTrigger
    ON ACTIVITY
    INSTEAD OF DELETE
AS
BEGIN
	DELETE FROM CHALLENGE_ACTIVITIES
    WHERE ActivityId IN(SELECT deleted.Id FROM deleted)

	DELETE FROM RACE_PARTICIPANTS
    WHERE ActivityId IN(SELECT deleted.Id FROM deleted)

	DELETE FROM ACTIVITY
    WHERE Id = (SELECT deleted.Id FROM deleted)
END;
GO

CREATE TRIGGER GroupDeletedTrigger
    ON GROUPS
    INSTEAD OF DELETE
AS
BEGIN
	DELETE FROM GROUP_USERS
    WHERE GroupId IN(SELECT deleted.Id FROM deleted)

	DELETE FROM CHALLENGE_VISIBILITY
    WHERE GroupId IN(SELECT deleted.Id FROM deleted)

	DELETE FROM RACE_VISIBILITY
    WHERE GroupId IN(SELECT deleted.Id FROM deleted)

	DELETE FROM GROUPS
    WHERE Id = (SELECT deleted.Id FROM deleted)
END;
GO

CREATE TRIGGER RaceDeletedTrigger
    ON RACE
    INSTEAD OF DELETE
AS
BEGIN
	DELETE FROM RACE_PARTICIPANTS
    WHERE RaceId IN(SELECT deleted.Id FROM deleted)

	DELETE FROM RACE_VISIBILITY
    WHERE RaceId IN(SELECT deleted.Id FROM deleted)

	DELETE FROM RACE
    WHERE Id = (SELECT deleted.Id FROM deleted)
END;
GO

CREATE TRIGGER ChallengeDeletedTrigger
    ON CHALLENGE
    INSTEAD OF DELETE
AS
BEGIN
	DELETE FROM CHALLENGE_PARTICIPANTS
    WHERE ChallengeId IN(SELECT deleted.Id FROM deleted)

	DELETE FROM CHALLENGE_ACTIVITIES
    WHERE ChallengeId IN(SELECT deleted.Id FROM deleted)

	DELETE FROM CHALLENGE_VISIBILITY
    WHERE ChallengeId IN(SELECT deleted.Id FROM deleted)

	DELETE FROM CHALLENGE
    WHERE Id = (SELECT deleted.Id FROM deleted)
END;
GO