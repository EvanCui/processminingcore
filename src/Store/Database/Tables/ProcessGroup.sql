CREATE TABLE [dbo].[ProcessGroup]
(
	[Id] BIGINT NOT NULL PRIMARY KEY,
    [Thumbprint] NVARCHAR(100) NOT NULL,
    [Name] NVARCHAR(50) NULL,
    [IsAnalyzed] BIT NOT NULL DEFAULT 0
)

GO

CREATE UNIQUE INDEX [IX_ProcessGroup_Thumbprint] ON [dbo].[ProcessGroup] ([Thumbprint])
