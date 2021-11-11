CREATE TABLE [dbo].[ProcessClusterActivityDefinitionRelationship]
(
	[ProcessClusterId] BIGINT NOT NULL PRIMARY KEY, 
    [ActivityDefinitionId] BIGINT NOT NULL, 
    CONSTRAINT [FK_ProcessClusterActivityDefinitionRelationship_ProcessCluster] FOREIGN KEY ([ProcessClusterId]) REFERENCES [ProcessCluster]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProcessClusterActivityDefinitionRelationship_ActivityDefinition] FOREIGN KEY ([ActivityDefinitionId]) REFERENCES [ActivityDefinition]([Id]) ON DELETE CASCADE
)

GO

CREATE INDEX [IX_ProcessClusterActivityDefinitionRelationship_ActivityDefinitionId] ON [dbo].[ProcessClusterActivityDefinitionRelationship] ([ActivityDefinitionId])
