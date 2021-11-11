CREATE PROCEDURE [dbo].[AttachActivityInstanceToProcessInstance]
	@batchSize int = 1000
AS BEGIN

	-- Create some process instance
	INSERT TOP(@batchSize) INTO ProcessInstance (Subject)
	SELECT ProcessSubject FROM ActivityInstance WHERE ProcessInstanceId IS NULL GROUP BY ProcessSubject
	EXCEPT
	SELECT Subject FROM ProcessInstance

	-- attach some activity instance, there should be something to attach as long as there are new process instance created.
	DECLARE @attached TABLE (Id BIGINT NOT NULL)

	UPDATE TOP(@batchSize) ai
	SET ai.ProcessInstanceId = pi.Id
	OUTPUT INSERTED.ProcessInstanceId INTO @attached
	FROM ActivityInstance ai
	JOIN ProcessInstance pi
	ON ai.ProcessSubject = pi.Subject
	WHERE ai.ProcessInstanceId IS NULL

	UPDATE ProcessInstance
	SET IsClustered = 0, IsAnalyzed = 0, Thumbprint = NULL
	WHERE Id IN (SELECT Id FROM @attached)

	UPDATE pc
	SET pc.IsAnalyzed = 0
	FROM ProcessCluster pc
	JOIN ProcessInstance pi
	ON pc.Id = pi.ProcessClusterId
	WHERE pi.Id IN (SELECT Id FROM @attached)

	RETURN 0
END