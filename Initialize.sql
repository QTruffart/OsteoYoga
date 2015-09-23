declare @idContact int
declare @idProfil1 int
declare @idProfil2 int
declare @idOffice1 int
declare @idOffice2 int

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
VALUES (30, 5, 4, 15, @idOffice1, @idContact),
	   (45, 10, 3, 45, @idOffice2, @idContact)


