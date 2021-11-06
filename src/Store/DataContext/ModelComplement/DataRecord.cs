using Newtonsoft.Json;

namespace Encoo.ProcessMining.DataContext.Model;

public partial class DataRecord
{
    private ContentData contentData = null;

    public ContentData ContentData
    {
        get => this.contentData ??= new ContentData(
            this.Content,
            this.Time,
            this.Template,
            JsonConvert.DeserializeObject<string[]>(this.Parameters));
    }
}
