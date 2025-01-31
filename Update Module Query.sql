update [DPIDB]..[Modules]
set [ModuleName]=N'رطوبت سالن'
where [ModuleNumber]='4'

SELECT 
	c.[Id],
	c.[Name],
	m.[Id],
	m.[Minimum],
	m.[Maximum],
	m.[ModuleNumber],
	m.[IsTemp],
	m.[ModuleName]
FROM [DPIDB]..[Modules] m
JOIN [DPIDB]..[Centers] c on c.[Id]=m.[CenterId]
where m.[ModuleNumber]='4'
order by m.[ModuleNumber]