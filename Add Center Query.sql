﻿INSERT INTO [DPIDB]..[Centers]
(
	[Id]
      ,[Name]
      ,[IPAddress]
      ,[Port]
      ,[OwnerId]
      ,[DateAdded]
      ,[LastUpdatedBy]
      ,[LastUpdateDate]
      ,[IsDeleted]
)
VALUES(
	NEWID(),
	N'مرکز شماره 1',
	'0.0.0.0',
	'1100',
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	0
),
(
	NEWID(),
	N'مرکز شماره 2',
	'0.0.0.0',
	'1110',
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	0
),
(
	NEWID(),
	N'مرکز شماره 3',
	'0.0.0.0',
	'1120',
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	0
),
(
	NEWID(),
	N'مرکز شماره 4',
	'0.0.0.0',
	'1130',
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	0
),
(
	NEWID(),
	N'مرکز شماره 5',
	'0.0.0.0',
	'1140',
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	0
),
(
	NEWID(),
	N'مرکز شماره 6',
	'0.0.0.0',
	'1150',
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	0
),
(
	NEWID(),
	N'مرکز شماره 7',
	'0.0.0.0',
	'1160',
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	0
),
(
	NEWID(),
	N'مرکز شماره 8',
	'0.0.0.0',
	'1170',
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	0
),
(
	NEWID(),
	N'مرکز شماره 9',
	'0.0.0.0',
	'1180',
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	0
),
(
	NEWID(),
	N'مرکز شماره 10',
	'0.0.0.0',
	'1190',
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	'00000000-0000-0000-0000-000000000000',
	GETDATE(),
	0
)
/*
SELECT TOP (1000) [Id]
      ,[Name]
      ,[IPAddress]
      ,[Port]
      ,[OwnerId]
      ,[DateAdded]
      ,[LastUpdatedBy]
      ,[LastUpdateDate]
      ,[IsDeleted]
  FROM [DPIDB].[dbo].[Centers]
*/