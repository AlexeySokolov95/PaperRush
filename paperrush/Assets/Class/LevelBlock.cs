using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Class
{
    public class LevelBlock : MonoBehaviour
    {
        protected GameObject climbBonus;
        protected Vector3[] crystalsPosition;

        public List<GameObject> walls = new List<GameObject>();
        public List<GameObject> elements = new List<GameObject>();
        protected LevelCreater LevelManager;
        protected float zCoordinateBeginningOfBlock;
        protected float lengthOfMainWall;
        protected float widthWall;
        protected float heightWall;
        protected float wallPosition;
        protected bool withClimbBonus;
        public float lengthOfOpenningWall = 0;
        public float lengthOfEndingWall = 0;
        public float climbBonusRadius = 1.5f;
        public float easyClimbBonusRadius = 3f;
        public bool easyBlock = false;
        GameObject wall;
        public float LengthOfMainWall
        {
            set { lengthOfMainWall = value;  }
        }
        protected void Initialization(float length)
        {
            InteractionWithLevelManager();
            SetBlockLength(length);
        }

        protected void InteractionWithLevelManager()
        {
            LevelManager = GameObject.FindWithTag("Level Creater").GetComponent<LevelCreater>();
            widthWall = LevelManager.WidthWall;
            heightWall = LevelManager.HeightWall;
            zCoordinateBeginningOfBlock = LevelManager.GetComponent<LevelCreater>().LevelLength + lengthOfOpenningWall;
            withClimbBonus = LevelManager.PutClimbBonus;
        }
        protected void SetBlockLength(float length)
        {
            lengthOfMainWall = length;
        }

        protected virtual void PutWall()
        {
            wallPosition = LevelManager.GetComponent<LevelCreater>().LevelLength + lengthOfMainWall / 2 + lengthOfOpenningWall;
            wall = Resources.Load("Nothing", typeof(GameObject)) as GameObject;
            walls.Add(Instantiate(wall));
            walls[0].transform.localScale = new Vector3(widthWall / 100, heightWall / 100, lengthOfMainWall / 100);
            walls[0].transform.position = new Vector3(0, 0, wallPosition);
            PutAdditionalWalls();
            PutSpecificWall();
            float fullLengthOfBlock = lengthOfMainWall + lengthOfOpenningWall + lengthOfEndingWall;
            LevelManager.GetComponent<LevelCreater>().AddLength(fullLengthOfBlock);
        }
        private void PutAdditionalWalls()
        {
            if (lengthOfOpenningWall > 0)
            {
                walls.Add(Instantiate(wall));
                walls[1].transform.localScale = new Vector3(widthWall / 100, heightWall / 100, lengthOfOpenningWall / 100);
                float OpeningWallPosition = LevelManager.GetComponent<LevelCreater>().LevelLength + (lengthOfOpenningWall / 2);
                walls[1].transform.position = new Vector3(0, 0, OpeningWallPosition);
            }
            if (lengthOfEndingWall > 0)
            {
                walls.Add(Instantiate(wall));
                walls[2].transform.localScale = new Vector3(widthWall / 100, heightWall / 100, lengthOfEndingWall / 100);
                float EndingWallPosition = LevelManager.GetComponent<LevelCreater>().LevelLength + lengthOfOpenningWall + lengthOfMainWall + (lengthOfEndingWall / 2);
                walls[2].transform.position = new Vector3(0, 0, EndingWallPosition);
            }
        }
        protected void PutSpecificWall()
        {

        }
        protected virtual void PutClimbBonus()
        {

        }
        protected bool AnyBonusBeside(Vector3 checkedVector)
        {
            bool answ = false;
            float minDistanceBeetwenBonuses = 3;
            bool climbBonusBeside = false;
            if (climbBonus != null)
                climbBonusBeside = Vector2.Distance(new Vector2(climbBonus.transform.position.x, climbBonus.transform.position.z), 
                    new Vector2(checkedVector.x, checkedVector.z)) < minDistanceBeetwenBonuses;
            bool crystalBonusBeside = false;
            for (int i = 0; i < crystalsPosition.Length; i++)
            {
                if (crystalsPosition[i] != null)
                {
                    Vector3 crystalPosition = crystalsPosition[i];
                    Vector2 crystalBonusesInXZ = new Vector2(crystalPosition.x, crystalPosition.z);
                    Vector2 checkedBonusesInXZ = new Vector2(checkedVector.x,checkedVector.z);
                    if (Vector2.Distance(crystalBonusesInXZ, checkedBonusesInXZ) < minDistanceBeetwenBonuses)
                    {
                        crystalBonusBeside = true;
                        break;
                    }
                }
            }
            if (climbBonusBeside || crystalBonusBeside)
            {
                answ = true;
            }
            return answ;
        }
        protected bool PlayerIsNear(float distance)
        {
            bool isNear = LevelManager.player.transform.position.z > transform.position.z - distance && LevelManager.rbpPlayer.isMoving;
            return isNear;
        }
    }
}
