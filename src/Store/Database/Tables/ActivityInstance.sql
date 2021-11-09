CREATE TABLE [dbo].[ActivityInstance]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [DataRecordId] BIGINT NOT NULL, 
    [ActivityDefinitionId] BIGINT NOT NULL, 
    [DetectionRuleId] BIGINT NOT NULL, 
    [ProcessSubject] NVARCHAR(200) NOT NULL,
    [Time] DATETIMEOFFSET NULL,
    [Actor] NVARCHAR(50) NULL,
    [ProcessInstanceId] BIGINT NULL,
    CONSTRAINT [FK_ActivityInstance_DataRecord] FOREIGN KEY ([DataRecordId]) REFERENCES [DataRecord]([Id]),
    CONSTRAINT [FK_ActivityInstance_ActivityDefinition] FOREIGN KEY ([ActivityDefinitionId]) REFERENCES [ActivityDefinition]([Id]),
    CONSTRAINT [FK_ActivityInstance_ActivityDetectionRule] FOREIGN KEY ([DetectionRuleId]) REFERENCES [ActivityDetectionRule]([Id]),
)

GO

CREATE INDEX [IX_ActivityInstance_ProcessInstanceId_ProcessSubject] ON [dbo].[ActivityInstance] ([ProcessInstanceId],[ProcessSubject])
