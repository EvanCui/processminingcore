CREATE TABLE [dbo].[DataRecord]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
    [Status] INT NOT NULL, 
    [KnowledgeWatermark] BIGINT NULL, 
    [Content] TEXT NULL, 
    [Time] DATETIMEOFFSET NULL, 
    [Template] NVARCHAR(1000) NULL, 
    [Parameters] NVARCHAR(1000) NULL
)

GO

CREATE INDEX [IX_DataRecord_Status_KnowledgeWatermark] ON [dbo].[DataRecord] ([Status],[KnowledgeWatermark])
