using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Class
{
    [Serializable]
    public class PurchasedPlanerMaterial
    {
        public string Name;
        public bool IsPurchased;
        public PurchasedPlanerMaterial(string name, bool isPurchased)
        {
            IsPurchased = isPurchased;
            Name = name;
        }
    }
}
