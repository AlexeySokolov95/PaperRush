using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Class
{
    class ChoosenElement<T>
    {
        public T Element { get; set; }
        public double Probability { get; set; }
        private bool isSelected = false;
        const double startingProbability = 0.5f;
        public double deltaProbability = 0.1f;
        public int maxNumberIterationWhenProbabilityDontChanged = 3;
        public int iterationWhenProbabilityDontChanged = 0;
        public ChoosenElement(T elem, double deltaProb, int numberIterationWhenProbabilityDontChanged)
        {
            Element = elem;
            Probability = startingProbability;
            deltaProbability = deltaProb;
            maxNumberIterationWhenProbabilityDontChanged = numberIterationWhenProbabilityDontChanged;
        }
        public void IsSelected()
        {
            isSelected = true;
            Probability = 0;
        }
        public void ChangeProbability()
        {
            if (!isSelected)
                Probability += deltaProbability;
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
