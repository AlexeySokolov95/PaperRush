using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Class
{
    public class BlockElement : MonoBehaviour
    {
        protected LevelCreater LevelManager;
        public Vector3 Position { get; set; }
        protected float widthWall;
        protected float heightWall;
        protected void Initialization()
        {
            LevelManager = GameObject.Find("LevelManager").GetComponent<LevelCreater>();
            widthWall = LevelManager.WidthWall;
            heightWall = LevelManager.HeightWall;
        }
    }   
}
