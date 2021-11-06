using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

interface ITokenExtractorFactory
{
    ITokenExtractor CreateTokenExtractor(TokenExtractionOptions options);
}
