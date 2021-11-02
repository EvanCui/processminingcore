﻿using Encoo.ProcessMining.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoo.ProcessMining.Engine
{
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
                    .Select(d => (Id: d.Id.Value, Detector: this.activityDetectorFactory.CreateActivityDetector(d)))
                    .ToDictionary(d => d.Id, d => d.Detector);
            }
            else
            {
                var toDelete = this.detectors.Keys.Except(definitions.Select(d => d.Id.Value)).ToList();
                toDelete.ForEach(id => this.detectors.Remove(id));

                foreach (var definition in definitions)
                {
                    if (!this.detectors.ContainsKey(definition.Id.Value))
                    {
                        this.detectors[definition.Id.Value] = this.activityDetectorFactory.CreateActivityDetector(definition);
                    }
                }
            }
        }
    }
}
