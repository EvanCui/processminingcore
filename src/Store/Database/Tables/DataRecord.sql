CREATE TABLE [dbo].[DataRecord]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [KnowledgeWatermark] BIGINT NOT NULL DEFAULT 0, 
    [Content] TEXT NULL, 
    [Time] DATETIMEOFFSET NULL, 
    [Template] TEXT NULL, 
    [Parameters] TEXT NULL, 
    [IsTemplateDetected] BIT NOT NULL DEFAULT 0, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [IsActivityDetected] BIT NOT NULL DEFAULT 0
)

GO

CREATE INDEX [IX_DataRecord_IsDeleted_IsTemplateDetected_IsActivityDetected_KnowledgeWatermark] ON [dbo].[DataRecord] ([IsDeleted],[IsTemplateDetected],[IsActivityDetected],[KnowledgeWatermark])
