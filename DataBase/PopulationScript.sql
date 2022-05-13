USE StraviaTecDB;
GO

/*
--User registration
EXEC Register @User = 'sfv',
			@FirstName = 'Simon',
			@LastName1 = 'Fallas',
			@LastName2 = 'Villalobos',
			@BirthDate = '1998-02-15',
			@Password = 'Simon-12345',
			@Picture = 'profilePic',
			@Nationality = 'CostaRican';

--User login
EXEC LoginUser @User = 'sfv',
				@Password = 'Simon-12345';

--Default Activity Types
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Running');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Swimming');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Cycling');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Hiking');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Kayaking');
INSERT INTO ACTIVITY_TYPE("Name") VALUES('Walking');
*/
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
/*
INSERT INTO GROUPS(AdminUser, "Name") VALUES('sfv','Group 1');						
INSERT INTO GROUPS(AdminUser, "Name") VALUES('sfv','Group 2');
INSERT INTO GROUPS(AdminUser, "Name") VALUES('sfv','Group 3');
*/

	



