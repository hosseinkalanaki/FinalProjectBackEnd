;with sourceCenter AS(
	SELECT 
		*
	FROM [DPIDB]..[Centers]
)
INSERT INTO [DPIDB].[dbo].[Modules]
([Id]
      ,[Minimum]
      ,[Maximum]
	  ,[ModuleNumber]
	  ,[IsTemp]
      ,[CenterId]
      ,[OwnerId]
      ,[DateAdded]
      ,[LastUpdatedBy]
      ,[LastUpdateDate]
      ,[IsDeleted]
 )
 SELECT 
	NEWID(),
	20,
	60,
	3,
	0,
	s.[Id],
	s.[OwnerId],
	s.[DateAdded],
	s.[LastUpdatedBy],
	s.[LastUpdateDate],
	s.[IsDeleted]
from sourceCenter s
