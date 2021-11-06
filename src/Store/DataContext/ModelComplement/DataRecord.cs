using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Encoo.ProcessMining.DataContext.Model;

public partial class DataRecord
{
    [NotMapped]
    private ContentData contentData = null;

    [NotMapped]
    public ContentData ContentData
    {
        get => this.contentData ??= new ContentData(
            this.Content,
            this.Time,
            this.Template,
            this.ParametersArray);
    }

    [NotMapped]
    private string[] parametersArray = null;

    [NotMapped]
    public string[] ParametersArray
    {
        get => this.parametersArray ??= JsonConvert.DeserializeObject<string[]>(this.Parameters);
        set => this.Parameters = JsonConvert.SerializeObject(this.parametersArray = value);
    }
}
