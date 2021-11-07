using Encoo.ProcessMining.DataContext.DatabaseContext;
using Encoo.ProcessMining.DataContext.Model;
using Microsoft.EntityFrameworkCore;

namespace Encoo.ProcessMining.DataContext;

public class DataRecordDataContext : IDataRecordDataContext
{
    private readonly ProcessMiningDatabaseContext databaseContext;

    public DataRecordDataContext(ProcessMiningDatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    public async Task DeleteDataRecordsAsync(long[] recordIds, CancellationToken token)
    {
        if (recordIds != null)
        {
            this.databaseContext.DataRecords.RemoveRange(recordIds.Select(id => new DataRecord { Id = id }));
            await this.databaseContext.SaveChangesAsync(token);
        }
    }

    public IAsyncEnumerable<DataRecord> LoadDataRecordToDetectAsync(long currentKnowledgeWatermark, int batchSize, CancellationToken token) => this.databaseContext.DataRecords
        .Where(d => d.IsDeleted == false && d.IsTemplateDetected == true && d.IsActivityDetected == false && d.KnowledgeWatermark < currentKnowledgeWatermark)
        .Take(batchSize)
        .AsAsyncEnumerable();

    public IAsyncEnumerable<DataRecord> LoadDataRecordToInvestigateAsync(long currentKnowledgeWatermark, int batchSize, CancellationToken token) => this.databaseContext.DataRecords
        .Where(d => d.IsDeleted == false && d.IsTemplateDetected == true && d.IsActivityDetected == false && d.KnowledgeWatermark >= currentKnowledgeWatermark)
        .Take(batchSize)
        .AsAsyncEnumerable();
}

