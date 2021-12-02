CREATE TABLE [dbo].[ActivityDefinition]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NULL, 
    [Details] NVARCHAR(1000) NULL, 
    [ProcessDefinitionId] BIGINT NOT NULL, 
    CONSTRAINT [FK_ActivityDefinition_ProcessDefinition] FOREIGN KEY ([ProcessDefinitionId]) REFERENCES [ProcessDefinition]([Id]) ON DELETE CASCADE
)

GO

CREATE INDEX [IX_ActivityDefinition_ProcessDefinitionId] ON [dbo].[ActivityDefinition] ([ProcessDefinitionId])
