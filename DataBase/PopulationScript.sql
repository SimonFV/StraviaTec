USE StraviaTecDB;
GO


--User registration
EXEC Register @User = 'src', @FirstName = 'Sergio', @LastName1 = 'Ríos', @LastName2 = 'Campos',
			@BirthDate = '2000-12-24', @Password = '12345', @Picture = 'Files\Profiles\src\srcPic.jpg', @Nationality = 'Costa Rica';
GO
EXEC Register @User = 'sfv', @FirstName = 'Simón', @LastName1 = 'Fallas', @LastName2 = 'Villalobos',
			@BirthDate = '1998-02-15', @Password = '12345', @Picture = 'Files\Profiles\sfv\sfvPic.jpg', @Nationality = 'Costa Rica';
GO
EXEC Register @User = 'goq', @FirstName = 'Gretchell', @LastName1 = 'Ochoa', @LastName2 = 'Quintero',
			@BirthDate = '2000-12-24', @Password = '12345', @Picture = 'Files\Profiles\goq\goqPic.jpg', @Nationality = 'Costa Rica';
GO

INSERT INTO GROUPS(AdminUser, "Name") VALUES('sfv','Group Simon');						
INSERT INTO GROUPS(AdminUser, "Name") VALUES('src','Group Sergio');

INSERT INTO FRIENDS("User", FriendUser) VALUES('sfv','src');
INSERT INTO FRIENDS("User", FriendUser) VALUES('sfv','goq');


INSERT INTO CATEGORY("Name", Description) VALUES('Junior','Under 15 years.');
INSERT INTO CATEGORY("Name", Description) VALUES('Sub-23','From 15 to 23 years.');
INSERT INTO CATEGORY("Name", Description) VALUES('Open','From 24 to 30 years.');
INSERT INTO CATEGORY("Name", Description) VALUES('Elite','All ages.');
INSERT INTO CATEGORY("Name", Description) VALUES('Master A','From 31 to 40 years.');
INSERT INTO CATEGORY("Name", Description) VALUES('Master B','From 41 to 50 years.');
INSERT INTO CATEGORY("Name", Description) VALUES('Master C','More than 50 years.');

--Default Activity Types
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Running');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Swimming');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Cycling');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Hiking');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Kayaking');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Walking');
GO

--Challenge registration

EXEC RegisterChallenge
	@User= 'sfv',
	@Name='Reto',
	@Class='Master',
	@Privacy=1,
	@Groups='Group Simon',
	@StartDate='2022-06-15',
	@EndDate= '2022-06-20',
	@Activity_Type = 'Running',
	@Objective= 10;

GO
-- Race registration

EXEC RegisterRace
	@Admin= 'sfv',
	@Name='Carrera',
	@Route='',
	@Cost=1000,
	@Privacy=1,
	@Groups='Group Simon',
	@StartDate='2022-06-15',
	@Category='Junior,Open',
	@Type = 'Running'

GO
EXEC Register_in_Race
	@User = 'src',
	@RaceName = 'Carrera',
	@Category = 'Junior'

GO
--Adding activities

EXEC AddActivity @UserId = 'sfv', @Distance = 40.51, @Duration = '02:00:00', @Route = 'Files\Routes\Activities\1\sfvRoute.gpx',
	@Altitude = 10.1, @Start = '2022-05-21 14:10:00', @Type = 'Running', @RoC = '', @RoCName = '';
GO

EXEC AddActivity @UserId = 'src', @Distance = 40.51, @Duration = '02:00:00', @Route = 'Files\Routes\Activities\2\srcRoute.gpx',
	@Altitude = 10.1, @Start = '2022-05-21 14:10:00', @Type = 'Running', @RoC = '', @RoCName = '';
GO

EXEC AddActivity @UserId = 'goq', @Distance = 40.51, @Duration = '02:00:00', @Route = 'Files\Routes\Activities\3\goqRoute.gpx',
	@Altitude = 10.1, @Start = '2022-05-21 14:10:00', @Type = 'Running', @RoC = '', @RoCName = '';
GO

EXEC AddActivity @UserId = 'src', @Distance = 40.51, @Duration = '02:00:00', @Route = 'Files\Routes\Activities\1\sfvRoute.gpx',
	@Altitude = 10.1, @Start = '2022-05-21 19:10:00', @Type = 'Running', @RoC = 'Race', @RoCName = 'Carrera';
GO