CREATE TABLE [dbo].[ProcessInstance]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Subject] NVARCHAR(200) NOT NULL,
    [IsGrouped] BIT NOT NULL DEFAULT 0,
    [IsAnalyzed] BIT NOT NULL DEFAULT 0,
    [Thumbprint] NVARCHAR(100) NOT NULL,
    [ProcessGroupId] BIGINT NULL,
    CONSTRAINT [FK_ProcessInstance_ProcessGroup] FOREIGN KEY ([ProcessGroupId]) REFERENCES [ProcessGroup]([Id])
)

GO

CREATE UNIQUE INDEX [IX_ProcessInstance_Subject] ON [dbo].[ProcessInstance] ([Subject])

GO

CREATE INDEX [IX_ProcessInstance_IsGrouped_Thumbprint] ON [dbo].[ProcessInstance] ([IsGrouped],[Thumbprint])
