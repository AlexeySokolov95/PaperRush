using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Class;
using Assets.Scripts;

namespace Assets.Class
{
    class LevelBlockChooser
    {
        private List<ChoosenLevelBlock> allLevelBlocks;
        private List<ChoosenLevelBlock> allEasyLevelBlocks;
        private ChoosenLevelBlock[] anyLevelBlocks;
        private ChoosenLevelBlock[] smallLevelBlockWithClimbBonus;
        private ChoosenLevelBlock[] easyAnyLevelBlocks;
        private ChoosenLevelBlock[] easySmallLevelBlockWithClimbBonus;
        private System.Random random = new System.Random();
        private int numberIterationWhenProbabilityDontChanged = 3;
        private LevelCreater levelCreater;
        private float lengthOfEasyPeriod;
        private float lengthOfMiddlePeriod;
        Dictionary<string, string> easyAndNormalNames;
        public LevelBlockChooser(GameObject[] anyBlock, GameObject[] smallWithClimbBonus, GameObject[] easyAnyBlock, GameObject[] easySmallWithClimbBonus, float lengOfEasyPeriod)
        {
            lengthOfEasyPeriod = lengOfEasyPeriod;
            lengthOfMiddlePeriod = lengOfEasyPeriod * 2;
            anyLevelBlocks = new ChoosenLevelBlock[anyBlock.Count()];
            smallLevelBlockWithClimbBonus = new ChoosenLevelBlock[smallWithClimbBonus.Count()];
            int maxNumberIterationWhenProbabilityDontChanged = 3;
            float deltaProb = 1f / ((float)anyBlock.Length - (float)maxNumberIterationWhenProbabilityDontChanged);
            for (int i = 0; i < anyBlock.Count(); i++)
                anyLevelBlocks[i] = new ChoosenLevelBlock(anyBlock[i], deltaProb, maxNumberIterationWhenProbabilityDontChanged);
            for (int i = 0; i < smallWithClimbBonus.Count(); i++)
                smallLevelBlockWithClimbBonus[i] = new ChoosenLevelBlock(smallWithClimbBonus[i], deltaProb, maxNumberIterationWhenProbabilityDontChanged);
            for (int i = 0; i < smallWithClimbBonus.Count(); i++)
            {
                if (anyLevelBlocks.Any(x => x.levelBlock.name == smallWithClimbBonus[i].name))
                    smallLevelBlockWithClimbBonus[i] = anyLevelBlocks.FirstOrDefault(x => x.levelBlock.name == smallWithClimbBonus[i].name);
            }
            allLevelBlocks = new List<ChoosenLevelBlock>();
            allLevelBlocks.AddRange(anyLevelBlocks);
            foreach (var block in smallLevelBlockWithClimbBonus)
            {
                if (!allLevelBlocks.Any(x => x.levelBlock.name == block.levelBlock.name))
                    allLevelBlocks.Add(block);
            }
            //Add easy levelBlocks
            easyAnyLevelBlocks = new ChoosenLevelBlock[easyAnyBlock.Count()];
            easySmallLevelBlockWithClimbBonus = new ChoosenLevelBlock[easySmallWithClimbBonus.Count()];
            float deltaProbForeasyBlocks = 1f / ((float)easyAnyBlock.Length - (float)maxNumberIterationWhenProbabilityDontChanged);
            for (int i = 0; i < easyAnyBlock.Count(); i++)
                easyAnyLevelBlocks[i] = new ChoosenLevelBlock(easyAnyBlock[i], deltaProbForeasyBlocks, maxNumberIterationWhenProbabilityDontChanged);
            for (int i = 0; i < easySmallWithClimbBonus.Count(); i++)
                easySmallLevelBlockWithClimbBonus[i] = new ChoosenLevelBlock(easySmallWithClimbBonus[i], deltaProbForeasyBlocks, 2);
            for (int i = 0; i < easySmallLevelBlockWithClimbBonus.Count(); i++)
            {
                if (easyAnyLevelBlocks.Any(x => x.levelBlock.name == easySmallWithClimbBonus[i].name))
                    easySmallLevelBlockWithClimbBonus[i] = easyAnyLevelBlocks.FirstOrDefault(x => x.levelBlock.name == easySmallWithClimbBonus[i].name);
            }
            allEasyLevelBlocks = new List<ChoosenLevelBlock>();
            allEasyLevelBlocks.AddRange(easyAnyLevelBlocks);
            foreach (var block in easySmallLevelBlockWithClimbBonus)
            {
                if (!allEasyLevelBlocks.Any(x => x.levelBlock.name == block.levelBlock.name))
                    allEasyLevelBlocks.Add(block);
            }
            levelCreater = GameObject.FindWithTag("Level Creater").GetComponent<LevelCreater>();
            easyAndNormalNames = new Dictionary<string, string>
            {
                {"EasyBlocksWallBlock","BlocksWallBlock" },
                {"EasyCorridorBlock","CorridorBlock" },
                {"EasyCrackInWallBlock","CrackInWallBlock" },
                {"EasyDiagonalWallBlock","DiagonalWallBlock" },
                {"EasySharpTurnBlock","SharpTurnBlock" },
                {"EasyZigZagBlock2","ZigZagBlock2" },
                {"EasyMovingOnePillarBlock", "MovingOnePillarBlock"},
                {"EasyMovingCrackInWallBlock", "MovingCrackInWall" },
                {"EasySeveralMovingPillarBlock","SeveralMovingPillarBlock"}
            };
        }
        public GameObject Next(ref NextBlock nextBlockType, float playerPosZ)
        {
            GameObject newBlock = null;
            if (levelCreater.LevelLength < lengthOfEasyPeriod)
            {
                if (nextBlockType == NextBlock.Any)
                {
                    levelCreater.PutClimbBonus = false;
                    MixBlocks(easyAnyLevelBlocks);
                    double rand = random.NextDouble();
                    double maxPropability = easyAnyLevelBlocks.Max(x => x.probability);
                    if (rand > maxPropability)
                        rand = maxPropability;
                    int i = 0;
                    int save = 0;
                    while (newBlock == null && save < 100)
                    {
                        if (easyAnyLevelBlocks[i].probability >= rand)
                            newBlock = easyAnyLevelBlocks[i].levelBlock;
                        i++;
                        save++;
                        if (i >= easyAnyLevelBlocks.Length)
                            i = 0;
                    }
                    if (newBlock == null)
                        MixBlocks(easyAnyLevelBlocks);
                    if (newBlock.tag != "BigBlock")
                        nextBlockType = NextBlock.SmallBlockWithClimbBonus;
                    else
                        nextBlockType = NextBlock.Any;
                }
                else
                {
                    levelCreater.PutClimbBonus = true;
                    MixBlocks(easySmallLevelBlockWithClimbBonus);
                    double rand = random.NextDouble();
                    double maxPropability = easySmallLevelBlockWithClimbBonus.Max(x => x.probability);
                    if (rand > maxPropability)
                        rand = maxPropability;
                    int i = 0;
                    int save = 0;
                    while (newBlock == null && save < 100)
                    {
                        if (easySmallLevelBlockWithClimbBonus[i].probability >= rand)
                            newBlock = easySmallLevelBlockWithClimbBonus[i].levelBlock;
                        i++;
                        save++;
                        if (i >= easySmallLevelBlockWithClimbBonus.Length)
                            i = 0;
                    }
                    if (newBlock == null)
                        newBlock = easySmallLevelBlockWithClimbBonus[0].levelBlock;
                    nextBlockType = NextBlock.Any;
                }
                for (int j = 0; j < allEasyLevelBlocks.Count(); j++)
                {
                    if (allEasyLevelBlocks[j].levelBlock == newBlock)
                        allEasyLevelBlocks[j].IsSelected();
                    allEasyLevelBlocks[j].ChangeProbability();
                }
                ChoosenLevelBlock block = allLevelBlocks.First(x => x.levelBlock.name == easyAndNormalNames[newBlock.name]);
                block.IsSelected();
                block.ChangeProbability();
            }
            else
            {
                if (nextBlockType == NextBlock.Any)
                {
                    levelCreater.PutClimbBonus = false;
                    MixBlocks(anyLevelBlocks);
                    double rand = random.NextDouble();
                    double maxPropability = anyLevelBlocks.Max(x => x.probability);
                    if (rand > maxPropability)
                        rand = maxPropability;
                    int i = 0;
                    int save = 0;
                    while (newBlock == null && save < 100)
                    {
                        if (anyLevelBlocks[i].probability >= rand)
                            newBlock = anyLevelBlocks[i].levelBlock;
                        i++;
                        save++;
                        if (i >= anyLevelBlocks.Length)
                            i = 0;
                    }
                    if (newBlock == null)
                        MixBlocks(anyLevelBlocks);
                    if (newBlock.tag != "BigBlock")
                        nextBlockType = NextBlock.SmallBlockWithClimbBonus;
                    else
                        nextBlockType = NextBlock.Any;
                }
                else
                {
                    levelCreater.PutClimbBonus = true;
                    MixBlocks(smallLevelBlockWithClimbBonus);
                    double rand = random.NextDouble();
                    double maxPropability = smallLevelBlockWithClimbBonus.Max(x => x.probability);
                    if (rand > maxPropability)
                        rand = maxPropability;
                    int i = 0;
                    int save = 0;
                    while (newBlock == null && save < 100)
                    {
                        if (smallLevelBlockWithClimbBonus[i].probability >= rand)
                            newBlock = smallLevelBlockWithClimbBonus[i].levelBlock;
                        i++;
                        save++;
                        if (i >= smallLevelBlockWithClimbBonus.Length)
                            i = 0;
                    }
                    if (newBlock == null)
                        newBlock = smallLevelBlockWithClimbBonus[0].levelBlock;
                    nextBlockType = NextBlock.Any;
                }
                for (int j = 0; j < allLevelBlocks.Count(); j++)
                {
                    if (allLevelBlocks[j].levelBlock == newBlock)
                        allLevelBlocks[j].IsSelected();
                    allLevelBlocks[j].ChangeProbability();
                }
                if (newBlock.name == "TrapOutsideBlock" && newBlock.name == "TrapInsideBlock")
                {
                    smallLevelBlockWithClimbBonus.First(x => x.levelBlock.name == "TrapOutsideBlock").IsSelected();
                    smallLevelBlockWithClimbBonus.First(x => x.levelBlock.name == "TrapOutsideBlock").ChangeProbability();
                    smallLevelBlockWithClimbBonus.First(x => x.levelBlock.name == "TrapInsideBlock").IsSelected();
                    smallLevelBlockWithClimbBonus.First(x => x.levelBlock.name == "TrapInsideBlock").ChangeProbability();
                }
                if (newBlock.name == "TrapOutsideBlock" && newBlock.name == "TrapInsideBlock")
                {
                    smallLevelBlockWithClimbBonus.First(x => x.levelBlock.name == "TrapOutsideBlock").IsSelected();
                    smallLevelBlockWithClimbBonus.First(x => x.levelBlock.name == "TrapOutsideBlock").ChangeProbability();
                    smallLevelBlockWithClimbBonus.First(x => x.levelBlock.name == "TrapInsideBlock").IsSelected();
                    smallLevelBlockWithClimbBonus.First(x => x.levelBlock.name == "TrapInsideBlock").ChangeProbability();
                }
            }
            if(levelCreater.LevelLength > lengthOfEasyPeriod && levelCreater.LevelLength < lengthOfMiddlePeriod)
                newBlock.GetComponent<LevelBlock>().climbBonusRadius = 2.25f;
            if (levelCreater.LevelLength >  lengthOfMiddlePeriod)
                newBlock.GetComponent<LevelBlock>().climbBonusRadius = 1.5f;
            if (levelCreater.LevelLength < lengthOfEasyPeriod)
            {
                newBlock.GetComponent<LevelBlock>().climbBonusRadius = 2.25f;
                newBlock.GetComponent<LevelBlock>().easyClimbBonusRadius = 2.25f;
            }
            return newBlock;

        }
        private void MixBlocks(ChoosenLevelBlock[] levelBlocks)
        {
            var r = new System.Random();
            for (int i = levelBlocks.Count() - 1; i > 0; i--)
            {
                int j = r.Next(i);
                var t = levelBlocks[i];
                levelBlocks[i] = levelBlocks[j];
                levelBlocks[j] = t;
            }
        }
    }
}