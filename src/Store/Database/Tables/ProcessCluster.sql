CREATE TABLE [dbo].[ProcessCluster]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Thumbprint] NVARCHAR(100) NOT NULL,
    [Name] NVARCHAR(50) NULL,
    [IsAnalyzed] BIT NOT NULL DEFAULT 0
)

GO

CREATE UNIQUE INDEX [IX_ProcessCluster_Thumbprint] ON [dbo].[ProcessCluster] ([Thumbprint])
