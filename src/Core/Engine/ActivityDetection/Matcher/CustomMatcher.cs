using Encoo.ProcessMining.DataContext.Model;
using Newtonsoft.Json.Linq;

namespace Encoo.ProcessMining.Engine;

class CustomMatcher : IMatcher
{
    private readonly JObject options;

    public CustomMatcher(JObject options)
    {
        this.options = options;
    }

    public (bool Success, string[] Tokens) Match(ContentData contentData)
    {
        Debug.Assert(this.options != null);
        throw new NotImplementedException();
    }
}
