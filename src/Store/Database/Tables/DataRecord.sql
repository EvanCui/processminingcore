CREATE TABLE [dbo].[DataRecord]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
    [KnowledgeWatermark] BIGINT NULL, 
    [Content] TEXT NULL, 
    [Time] DATETIMEOFFSET NULL, 
    [Template] NVARCHAR(1000) NULL, 
    [Parameters] NVARCHAR(1000) NULL, 
    [IsTemplateDetected] BIT NOT NULL DEFAULT 0, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [IsActivityDetected] BIT NOT NULL DEFAULT 0
)

GO

CREATE INDEX [IX_DataRecord_IsDeleted_IsTemplateDetected_IsActivityDetected_KnowledgeWatermark] ON [dbo].[DataRecord] ([IsDeleted],[IsTemplateDetected],[IsActivityDetected],[KnowledgeWatermark])
