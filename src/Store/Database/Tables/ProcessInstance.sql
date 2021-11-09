CREATE TABLE [dbo].[ProcessInstance]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Subject] NVARCHAR(200) NOT NULL,
    [IsClassified] BIT NOT NULL DEFAULT 0,
    [Thumbprint] NVARCHAR(2000) NOT NULL,
    [ProcessClassificationId] BIGINT NULL
)

GO

CREATE UNIQUE INDEX [IX_ProcessInstance_Subject] ON [dbo].[ProcessInstance] ([Subject])
