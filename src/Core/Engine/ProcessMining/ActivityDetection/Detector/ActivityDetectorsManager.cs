using Encoo.ProcessMining.DataContext.Model;

namespace Encoo.ProcessMining.Engine;

class ActivityDetectorsManager : IActivityDetectorsManager
{
    private IDictionary<long, IActivityDetector> detectors = new Dictionary<long, IActivityDetector>();

    private readonly IActivityDetectorFactory activityDetectorFactory = new DefaultActivityDetectorFactory();

    public IActivityDetector GetDetector(long definitionId) => this.detectors[definitionId];

    public void Initialize(IEnumerable<ActivityDefinition> definitions, bool rebuild = false)
    {
        if (rebuild)
        {
            this.detectors = definitions
                .Select(d => (d.Id, Detector: this.activityDetectorFactory.CreateActivityDetector(d)))
                .ToDictionary(d => d.Id, d => d.Detector);
        }
        else
        {
            var toDelete = this.detectors.Keys.Except(definitions.Select(d => d.Id)).ToList();
            toDelete.ForEach(id => this.detectors.Remove(id));

            foreach (var definition in definitions)
            {
                if (!this.detectors.ContainsKey(definition.Id))
                {
                    this.detectors[definition.Id] = this.activityDetectorFactory.CreateActivityDetector(definition);
                }
            }
        }
    }
}
