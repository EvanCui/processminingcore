CREATE PROCEDURE [dbo].[AttachProcessInstanceToProcessGroup]
	@batchSize int = 1000
AS BEGIN

	-- Create some process group
	INSERT TOP(@batchSize) INTO ProcessGroup (Thumbprint)
	SELECT Thumbprint FROM ProcessInstance WHERE IsGrouped = 0 AND Thumbprint IS NOT NULL GROUP BY Thumbprint
	EXCEPT
	SELECT Thumbprint FROM ProcessGroup

	-- attach some process instance, there should be something to attach as long as there are new process group created.
	DECLARE @attached TABLE (Id BIGINT NOT NULL)

	UPDATE TOP(@batchSize) pi
	SET pi.ProcessGroupId = pg.Id
	OUTPUT INSERTED.ProcessGroupId INTO @attached
	FROM ProcessInstance pi
	JOIN ProcessGroup pg
	ON pi.Thumbprint = pi.Thumbprint
	WHERE pi.IsGrouped = 0 AND pi.Thumbprint IS NOT NULL

	UPDATE ProcessGroup
	SET IsAnalyzed = 0
	WHERE Id IN (SELECT Id FROM @attached)

	RETURN 0
END