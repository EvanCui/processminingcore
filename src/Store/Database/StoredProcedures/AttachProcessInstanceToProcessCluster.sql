CREATE PROCEDURE [dbo].[AttachProcessInstanceToProcessCluster]
	@batchSize int = 1000
AS BEGIN
	-- TODO: check the affected rows return value.
	DECLARE @newProcessClusters TABLE (Id BIGINT NOT NULL)

	-- Create some process clusters
	INSERT TOP(@batchSize) INTO ProcessCluster (Thumbprint)
	OUTPUT INSERTED.Id INTO @newProcessClusters
	SELECT Thumbprint FROM ProcessInstance WHERE IsClustered = 0 AND Thumbprint IS NOT NULL GROUP BY Thumbprint
	EXCEPT
	SELECT Thumbprint FROM ProcessCluster

	-- attach some process instance
	-- there should be something to attach as long as there are new process clusters created.
	-- note: it's necessary to attach at least one instance for each newly created cluster.
	DECLARE @attached TABLE (Id BIGINT NOT NULL)

	-- attach for the newly created process clusters.
	UPDATE pi
	SET pi.ProcessClusterId = pc.Id
	OUTPUT INSERTED.ProcessClusterId INTO @attached
	FROM ProcessInstance pi
	JOIN ProcessCluster pc
	ON pi.Thumbprint = pi.Thumbprint
	WHERE pi.IsClustered = 0 AND pi.Thumbprint IS NOT NULL AND pc.Id IN (SELECT Id FROM @newProcessClusters)

	-- attach for the existing process clusters.
	UPDATE TOP(@batchSize) pi
	SET pi.ProcessClusterId = pc.Id
	OUTPUT INSERTED.ProcessClusterId INTO @attached
	FROM ProcessInstance pi
	JOIN ProcessCluster pc
	ON pi.Thumbprint = pi.Thumbprint
	WHERE pi.IsClustered = 0 AND pi.Thumbprint IS NOT NULL AND pc.Id NOT IN (SELECT Id FROM @newProcessClusters)

	UPDATE ProcessCluster
	SET IsAnalyzed = 0
	WHERE Id IN (SELECT Id FROM @attached)

	-- Generate relationships for new process clusters
	;WITH SampleInstance (SampleInstanceId, ProcessClusterId) AS (
		SELECT MIN(Id), ProcessClusterId
		FROM ProcessInstance
		WHERE ProcessClusterId IN (SELECT Id FROM @newProcessClusters)
		GROUP BY ProcessClusterId
	)
	INSERT INTO ProcessClusterActivityDefinitionRelationship
	SELECT si.ProcessClusterId, ai.ActivityDefinitionId
	FROM SampleInstance si
	JOIN ActivityInstance ai
	ON si.SampleInstanceId = ai.ProcessInstanceId

	-- Delete orphaned cluster
	-- Newly created clusters wouldn't be deleted here because the full attachment above.
	-- Some legacy cluster could be deleted in case all its attachments are gone, but non of the new instances is attached yet.
	DELETE pc
	FROM ProcessCluster pc
	WHERE NOT EXISTS (SELECT TOP(1) pi.Id FROM ProcessInstance pi WHERE pi.ProcessClusterId = pc.Id)

	RETURN 0
END