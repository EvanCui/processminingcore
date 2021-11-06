﻿CREATE TABLE [dbo].[ActivityDetectionRule]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NULL, 
    [Priority] INT NOT NULL DEFAULT 0, 
    [RuleData] TEXT NULL, 
    [ActivityDefinitionId] BIGINT NOT NULL, 
    CONSTRAINT [FK_ActivityDetectionRule_ActivityDefinition] FOREIGN KEY ([ActivityDefinitionId]) REFERENCES [ActivityDefinition]([Id])
)

GO

CREATE INDEX [IX_ActivityDetectionRule_Priority_Id] ON [dbo].[ActivityDetectionRule] ([Priority], [Id])

GO

CREATE UNIQUE INDEX [IX_ActivityDetectionRule_ActivityDefinitionId] ON [dbo].[ActivityDetectionRule] ([ActivityDefinitionId])
