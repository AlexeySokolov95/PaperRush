using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Class;
using Assets.Scripts;

namespace Assets.Class
{
    class ChoosenLevelBlock
    {
        public  GameObject levelBlock;
        public double probability { get; set; }
        const float startingProbability = 0.5f;
        public float deltaProbability = 0.1f;
        public int maxNumberIterationWhenProbabilityDontChanged = 3;
        public int iterationWhenProbabilityDontChanged = 0;
        private bool isSelected = false;
        public ChoosenLevelBlock(GameObject levelBlock_, float deltaProb, int numberIterationWhenProbabilityDontChanged)
        {
            levelBlock = levelBlock_;
            probability = startingProbability;
            deltaProbability = deltaProb;
            maxNumberIterationWhenProbabilityDontChanged = numberIterationWhenProbabilityDontChanged;
        }
        public void IsSelected()
        {
            isSelected = true;
            probability = 0f;
        }
        public void ChangeProbability()
        {
            if (!isSelected)
                probability += deltaProbability;
            else
            {
                iterationWhenProbabilityDontChanged++;
                if (iterationWhenProbabilityDontChanged >= maxNumberIterationWhenProbabilityDontChanged)
                {
                    isSelected = false;
                    iterationWhenProbabilityDontChanged = 0;
                }
            }
        }
    }
}
