USE StraviaTecDB;
GO

/*
--User registration
EXEC Register @User = 'asd',
			@FirstName = 'asd1',
			@LastName1 = 'asd2',
			@LastName2 = 'asd3',
			@BirthDate = '1998-02-21',
			@Password = 'Simon-12345',
			@Picture = 'profilePic2',
			@Nationality = 'CostaRicana';
GO


--User login
EXEC LoginUser @User = 'sfv',
				@Password = 'Simon-12345';



--Challenge registration
EXEC RegisterChallenge 
	@Id=0,
	@User= 'sfv',
	@Name='Reto ',
	@Class='Master',
	@Privacy=1,
	@GroupName='Group 1',
	@StartDate='2022-06-15',
	@EndDate= '2022-06-20',
	@Activity_Type = 'Running';

--INSERT INTO GROUPS(AdminUser, "Name") VALUES('sfv','Group 1');						


--Default Activity Types
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Running');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Swimming');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Cycling');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Hiking');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Kayaking');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Walking');
GO


EXEC AddActivity
	@UserId	= 'asd',
	@Distance = 34.34,
	@Duration = '05:45:23',
	@Route = 'routeasd',
	@Altitude = 14.1,
	@Start = '2022-11-13 03:50:00',
	@Type = 'Hiking';
GO

SELECT * FROM MyFriendsStartPage;
GO
*/