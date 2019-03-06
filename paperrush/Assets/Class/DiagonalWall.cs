using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Class
{
    class DiagonalWall : PartialLevelBlock
    {
        public int NumberOfBlocks { get; set; }
        public int BlocksForExit { get; set; }
        public void Initialization(float zCoordinate, float width, float height, int numberOfBlocks, int blocksForExit)
        {
            startZCoordinate = zCoordinate;
            widthWall = width;
            heightWall = height;
            NumberOfBlocks = numberOfBlocks;
            BlocksForExit = blocksForExit;
            endZCoordinate = zCoordinate + width;
        }
        void Start()
        {
            GameObject zigBlock = Instantiate(Resources.Load("pref_ZigWallBlock", typeof(GameObject))) as GameObject;
            zigBlock.transform.localScale = new Vector3(widthWall / NumberOfBlocks, heightWall, widthWall / NumberOfBlocks);
            for(int blockNumber = 1; blockNumber <= NumberOfBlocks - BlocksForExit; blockNumber ++)
            {
                PutBlock(zigBlock, blockNumber);
            }
            if (withClimbBonus)
                PutClimbBonus();


        } 
        void PutBlock(GameObject zigBlock, int blockNumber)
        {
            float xPosition = -(widthWall / 2) + (widthWall / (NumberOfBlocks * 2)) + ((widthWall / NumberOfBlocks) * (blockNumber - 1));
            float yPosition = heightWall / 2;
            float zPosition = startZCoordinate + (widthWall / (NumberOfBlocks * 2)) + ((widthWall / NumberOfBlocks) * (blockNumber - 1));
            zigBlock.transform.position = new Vector3(xPosition, yPosition, zPosition);
            GameObject newZigBlock = Instantiate(zigBlock) as GameObject;
        }
    }
}
