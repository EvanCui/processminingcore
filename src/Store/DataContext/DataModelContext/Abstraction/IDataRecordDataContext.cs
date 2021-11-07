using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.DataContext;

public interface IDataRecordDataContext
{
    IAsyncEnumerable<DataRecord> LoadDataRecordToDetectAsync(
        long currentKnowledgeWatermark,
        int batchSize,
        CancellationToken token);

    IAsyncEnumerable<DataRecord> LoadDataRecordToInvestigateAsync(
        long currentKnowledgeWatermark,
        int batchSize,
        CancellationToken token);

    Task DeleteDataRecordsAsync(
        long[] recordIds,
        CancellationToken token);
}

