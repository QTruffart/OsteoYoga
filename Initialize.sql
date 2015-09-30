declare @idContact int
declare @idProfil1 int
declare @idProfil2 int
declare @idOffice1 int
declare @idOffice2 int
declare @idpratictionerOffice1 int
declare @idpratictionerOffice2 int

declare @idLundi	int
declare @idMardi	int
declare @idMercredi int
declare @idJeudi	int
declare @idVendredi int
declare @idSamedi	int

delete from DefaultWorkDaysPO
delete from Duration
delete from PratictionerOffice
delete from DefaultWorkDay
delete from ContactProfile
delete from office
delete from Profile
delete from contact

INSERT INTO [dbo].[Contact]
			([ClassType]
			,[FullName]
			,[Mail]
			,[NetworkId]
			,[NetworkType]
			,[Phone]
			,[History])
		VALUES
			('Pratictioner'
			,'Truffart Nicolas'
			,'quentin.truffart@gmail.com'
			,null
			,null
			,'+33(0)6 95 01 44 21'
			,null)
set @idContact = (select SCOPE_IDENTITY())
	
INSERT INTO [dbo].[Profile]
			([Name])
		VALUES
			('Patient')	   
set @idProfil1 = (select SCOPE_IDENTITY())

INSERT INTO [dbo].[Profile]
			([Name])
		VALUES
			('Administrator')	
set @idProfil2 = (select SCOPE_IDENTITY())

INSERT INTO [dbo].[Office]
           ([Name],[Adress])
     VALUES ('Bayonnes', '210 Rue du Jardin Public 33300 Bordeaux')	 
set @idOffice1 = (select SCOPE_IDENTITY())
	 
INSERT INTO [dbo].[Office]
           ([Name],[Adress])
     VALUES ('Rion des Landes', '461 Avenue de Verdun 33700 Mérignac')
set @idOffice2 = (select SCOPE_IDENTITY())


INSERT INTO [dbo].[ContactProfile]
           ([Contact_id]
           ,[Profile_id])
     VALUES (@idContact, @idProfil1),
			(@idContact, @idProfil2)


INSERT INTO [dbo].[PratictionerOffice]
           ([Reminder]
           ,[DateWaiting]
           ,[MinInterval]
           ,[MaxInterval]
           ,[Office_id]
           ,[Pratictioner_id])
VALUES (30, 5, 4, 15, @idOffice1, @idContact)
	   
set @idpratictionerOffice1 = (select SCOPE_IDENTITY())

INSERT INTO [dbo].[Duration]
           ([Value], [PratictionerOffice_id])
     VALUES (45, @idpratictionerOffice1),
		 (30, @idpratictionerOffice1),
		 (60, @idpratictionerOffice1)


INSERT INTO [dbo].[PratictionerOffice]
           ([Reminder]
           ,[DateWaiting]
           ,[MinInterval]
           ,[MaxInterval]
           ,[Office_id]
           ,[Pratictioner_id])
VALUES (30, 5, 4, 15, @idOffice2, @idContact)
	   
set @idpratictionerOffice2 = (select SCOPE_IDENTITY())

INSERT INTO [dbo].[Duration]
           ([Value], [PratictionerOffice_id])
     VALUES (45, @idpratictionerOffice2),		 
			(60, @idpratictionerOffice2)
			
INSERT INTO [dbo].[DefaultWorkDay] ([DayOfTheWeek])
     VALUES ('Monday')
set @idLundi = (select SCOPE_IDENTITY())
	 
INSERT INTO [dbo].[DefaultWorkDay] ([DayOfTheWeek])
     VALUES ('Tuesday')
set @idMardi = (select SCOPE_IDENTITY())

INSERT INTO [dbo].[DefaultWorkDay] ([DayOfTheWeek])
     VALUES ('Wednesday')
set @idMercredi = (select SCOPE_IDENTITY())

INSERT INTO [dbo].[DefaultWorkDay] ([DayOfTheWeek])
     VALUES ('Thursday')
set @idJeudi = (select SCOPE_IDENTITY())

INSERT INTO [dbo].[DefaultWorkDay] ([DayOfTheWeek])
     VALUES ('Friday')
set @idVendredi = (select SCOPE_IDENTITY())

INSERT INTO [dbo].[DefaultWorkDay] ([DayOfTheWeek])
     VALUES ('Saturday')
set @idSamedi = (select SCOPE_IDENTITY())

INSERT INTO [dbo].[DefaultWorkDay] ([DayOfTheWeek])
     VALUES ('Sunday')
set @idLundi = (select SCOPE_IDENTITY())


INSERT INTO [dbo].[DefaultWorkDaysPO]
           ([BeginTime]
           ,[EndTime]
           ,[PratictionerOffice_id]
           ,[DefaultWorkDay_id]
           )
     VALUES
           ('01/01/2001 08:30'
           ,'01/01/2001 20:00'
           ,@idpratictionerOffice1
           ,@idLundi )

		   
INSERT INTO [dbo].[DefaultWorkDaysPO]
           ([BeginTime]
           ,[EndTime]
           ,[PratictionerOffice_id]
           ,[DefaultWorkDay_id]
           )
     VALUES
           ('01/01/2001 09:00'
           ,'01/01/2001 19:00'
           ,@idpratictionerOffice1
           ,@idMercredi )

		   
INSERT INTO [dbo].[DefaultWorkDaysPO]
           ([BeginTime]
           ,[EndTime]
           ,[PratictionerOffice_id]
           ,[DefaultWorkDay_id]
           )
     VALUES
           ('01/01/2001 08:00'
           ,'01/01/2001 12:00'
           ,@idpratictionerOffice1
           ,@idJeudi )

		   
INSERT INTO [dbo].[DefaultWorkDaysPO]
           ([BeginTime]
           ,[EndTime]
           ,[PratictionerOffice_id]
           ,[DefaultWorkDay_id]
           )
     VALUES
           ('01/01/2001 09:30'
           ,'01/01/2001 20:00'
           ,@idpratictionerOffice2
           ,@idMardi )

		   
		   
INSERT INTO [dbo].[DefaultWorkDaysPO]
           ([BeginTime]
           ,[EndTime]
           ,[PratictionerOffice_id]
           ,[DefaultWorkDay_id]
           )
     VALUES
           ('01/01/2001 08:00'
           ,'01/01/2001 18:00'
           ,@idpratictionerOffice2
           ,@idVendredi )

		   
		   
INSERT INTO [dbo].[DefaultWorkDaysPO]
           ([BeginTime]
           ,[EndTime]
           ,[PratictionerOffice_id]
           ,[DefaultWorkDay_id]
           )
     VALUES
           ('01/01/2001 14:00'
           ,'01/01/2001 21:00'
           ,@idpratictionerOffice2
           ,@idSamedi )