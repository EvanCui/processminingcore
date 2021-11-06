using Encoo.ProcessMining.DataContext.Model;
using Newtonsoft.Json.Linq;

namespace Encoo.ProcessMining.Engine;

class CustomTokenExtractor : ITokenExtractor
{
    private readonly JObject options;

    public CustomTokenExtractor(JObject options)
    {
        this.options = options;
    }

    public object Extract(ContentData contentData, string[] matchingTokens)
    {
        Debug.Assert(this.options != null);
        throw new NotImplementedException();
    }
}
