CREATE TABLE [dbo].[ActivityDefinition]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NULL, 
    [Details] NVARCHAR(1000) NULL
)
