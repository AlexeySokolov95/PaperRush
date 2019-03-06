using System;
using UnityEngine;

namespace Assets.Class
{
    class UniversalLevelBlock<Block> : LevelBlock 
        where Block : PartialLevelBlock
    {
        private Block levelBlock;
        void Start()
        {
            InteractionWithLevelManager();
            levelBlock = gameObject.AddComponent(typeof(Block)) as Block;
            levelBlock.Initialization(zCoordinateBeginningOfBlock, widthWall, heightWall, withClimbBonus);
            LengthOfMainWall = levelBlock.Length;
            PutWall();
        }
    }
}
