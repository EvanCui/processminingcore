CREATE TABLE [dbo].[ProcessInstance]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Subject] NVARCHAR(200) NOT NULL,
    [IsClustered] BIT NOT NULL DEFAULT 0,
    [IsAnalyzed] BIT NOT NULL DEFAULT 0,
    [Thumbprint] NVARCHAR(100) NOT NULL,
    [ProcessClusterId] BIGINT NULL,
    CONSTRAINT [FK_ProcessInstance_ProcessCluster] FOREIGN KEY ([ProcessClusterId]) REFERENCES [ProcessCluster]([Id]) ON DELETE SET NULL
)

GO

CREATE UNIQUE INDEX [IX_ProcessInstance_Subject] ON [dbo].[ProcessInstance] ([Subject])

GO

CREATE INDEX [IX_ProcessInstance_IsGrouped_Thumbprint] ON [dbo].[ProcessInstance] ([IsClustered],[Thumbprint])
