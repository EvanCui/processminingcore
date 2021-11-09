/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.
*/

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
	SET IsClassified = 0
	WHERE Id IN (SELECT Id FROM @attached)

	RETURN 0
END