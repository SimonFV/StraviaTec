USE StraviaTecDB;
GO

/*
--User registration
EXEC Register @User = 'andres', @FirstName = 'andres', @LastName1 = 'perico', @LastName2 = 'perico',
			@BirthDate = '2000-12-24', @Password = '12345', @Picture = 'profilePic3', @Nationality = 'German';
GO
EXEC Register @User = 'sfv', @FirstName = 'sfv', @LastName1 = 'sfv', @LastName2 = 'sfv',
			@BirthDate = '2000-12-24', @Password = '12345', @Picture = 'profilePic5', @Nationality = 'French';
GO

--User login
EXEC LoginUser @User = 'sfv', @Password = 'Simon-12345';



--Challenge registration


EXEC RegisterChallenge 
	@Id=0,
	@User= 'sfv',
	@Name='Reto ',
	@Class='Master',
	@Privacy=1,
	@Groups='Group 1,Group 2,Group 3',
	@StartDate='2022-06-15',
	@EndDate= '2022-06-20',
	@Activity_Type = 'Running';


INSERT INTO GROUPS(AdminUser, "Name") VALUES('sfv','Group 1');						
INSERT INTO GROUPS(AdminUser, "Name") VALUES('sfv','Group 2');
INSERT INTO GROUPS(AdminUser, "Name") VALUES('sfv','Group 3');

--Default Activity Types
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Running');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Swimming');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Cycling');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Hiking');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Kayaking');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Walking');
GO


EXEC AddActivity @UserId	= 'asd', @Distance = 34.34, @Duration = '05:45:23', @Route = 'routeasd',
	@Altitude = 14.1, @Start = '2022-11-13 03:50:00', @Type = 'Hiking';
GO

*/