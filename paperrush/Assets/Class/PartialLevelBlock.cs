using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Class
{
    class PartialLevelBlock : MonoBehaviour
    {
        public float startZCoordinate { get; set;}
        public float endZCoordinate { get; set; }
        public float widthWall { get; set; }
        public float heightWall { get; set; }
        public bool withClimbBonus { get; set; }

        public float Length
        {
            get { return endZCoordinate - startZCoordinate; }
        }
        protected virtual void PutClimbBonus()
        {

        }
        public virtual void Initialization(float zCoordinate, float width, float height,bool climbBonus)
        {
            startZCoordinate = zCoordinate;
            widthWall = width;
            heightWall = height;
            withClimbBonus = climbBonus;
        }
    }
}
