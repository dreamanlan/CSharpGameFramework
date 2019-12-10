using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.EnumerationUtilities
{
    internal class EnumerationStatus
    {
        public int StepCount { private set; get; }
        public int CurrentStep { private set; get; }
        public string StepStatus;

        public EnumerationStatus(int steps)
        {
            StepCount = steps;
            CurrentStep = 0;
        }

        public void IncrementStep()
        {
            if (CurrentStep + 1 == StepCount)
            {
                Debug.LogError("Conversion status, failed to increment step as it would exceed maximum step count");
                return;
            }
            ++CurrentStep;
        }
    }
}
