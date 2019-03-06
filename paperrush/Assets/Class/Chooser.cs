using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Class
{
   public class Chooser<T> : Object
    {
        private List<ChoosenElement<T>> choosenElements;
        private System.Random random = new System.Random();
        public Chooser(IEnumerable<T> elems)
        {
            List<T> mixElements = new List<T>(elems);
            MixElements(mixElements);
            int maxNumberIterationWhenProbabilityDontChanged = elems.Count() / 5;
            double deltaProb = 1 / (elems.Count() + 1);
            choosenElements = new List<ChoosenElement<T>>();
            foreach (var elem in mixElements)
                choosenElements.Add(new ChoosenElement<T>(elem, deltaProb, maxNumberIterationWhenProbabilityDontChanged));
        }
        public Chooser(IEnumerable<T> elems, double deltaProb, int maxNumberIterationWhenProbabilityDontChanged)
        {
            List<T> mixElements = new List<T>(elems);
            MixElements(mixElements);
            choosenElements = new List<ChoosenElement<T>>();
            foreach (var elem in mixElements)
                choosenElements.Add(new ChoosenElement<T>(elem, deltaProb, maxNumberIterationWhenProbabilityDontChanged));
        }
        public T Next()
        {
            T nextElem = default(T);
            double rand = random.NextDouble();
            double maxPropability = choosenElements.Max(x => x.Probability);
            if (rand > maxPropability)
                rand = maxPropability;
            foreach(var elem in choosenElements)
            {
                if (elem.Probability >= rand)
                {
                    nextElem = elem.Element;
                    elem.IsSelected();
                    break;
                }
            }
            foreach (var elem in choosenElements)
                elem.ChangeProbability();
            return nextElem;
        }
        private void MixElements(List<T> elements)
        {
            var r = new System.Random();
            for (int i = elements.Count() - 1; i > 0; i--)
            {
                int j = r.Next(i);
                var t = elements[i];
                elements[i] = elements[j];
                elements[j] = t;
            }
        }
    }
}
