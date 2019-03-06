using UnityEngine;
using Assets.Class;

public class DiagonalWallScript : LevelBlock
{
    public int numberOfBlocks  = 5;
    public int blocksForExit = 1;
    private Side exitSide;
    public GameObject zigWallBlock;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;

    void Start()
    {
        float blockLength = 20;
        Initialization(blockLength);
        PutWall();
        GameObject zigBlock = Instantiate(zigWallBlock);
        zigBlock.transform.localScale = new Vector3(widthWall / numberOfBlocks, heightWall, widthWall / numberOfBlocks);
        exitSide = (Side)Random.Range(0, 2);
        for (int blockNumber = 1; blockNumber <= numberOfBlocks - blocksForExit; blockNumber++)
        {
            PutBlock(zigBlock, blockNumber);
        }
        if (LevelManager.PutClimbBonus)
            PutClimbBonus();
        PutCrystalBonuses();
    }
    void PutBlock(GameObject zigBlock, int blockNumber)
    {
        float xPosition;        
        if (exitSide == Side.Right)
            xPosition = -(widthWall / 2) + (widthWall / (numberOfBlocks * 2)) + ((widthWall / numberOfBlocks) * (blockNumber - 1));
        else
            xPosition = (widthWall / 2) - ((widthWall / (numberOfBlocks * 2)) + ((widthWall / numberOfBlocks) * (blockNumber - 1)));
        float yPosition = heightWall / 2;
        float zPosition = zCoordinateBeginningOfBlock + (widthWall / (numberOfBlocks * 2)) + ((widthWall / numberOfBlocks) * (blockNumber - 1));
        zigBlock.transform.position = new Vector3(xPosition, yPosition, zPosition);
        GameObject newZigBlock = Instantiate(zigBlock) as GameObject;
    }

    protected override void PutClimbBonus()
    {
        climbBonus = Instantiate(climbBonusPref);
        int blockNumber;
        float minDistanceToBlock;
        if (easyBlock)
        {
            climbBonus.transform.localScale = new Vector3(easyClimbBonusRadius, climbBonus.transform.localScale.y, easyClimbBonusRadius);
            blockNumber = numberOfBlocks;
            minDistanceToBlock = 15f;
        }
        else
        {
            climbBonus.transform.localScale = new Vector3(climbBonusRadius, climbBonus.transform.localScale.y, climbBonusRadius);
            blockNumber = Random.Range(3, numberOfBlocks);
            minDistanceToBlock = 10f;
        }
        float zPositionEdgeBlock = zCoordinateBeginningOfBlock + (widthWall / (numberOfBlocks * 2)) + ((widthWall / numberOfBlocks) * (blockNumber - 1)) - (widthWall / (numberOfBlocks * 2));
        float addDistanceToZposition = 2f;
        float zBonusPosition = zPositionEdgeBlock - (minDistanceToBlock + ((numberOfBlocks - blockNumber) * addDistanceToZposition));
        float xBonusPosition = 0;
        if (exitSide == Side.Right)
            xBonusPosition = -(widthWall / 2) + (widthWall / (numberOfBlocks * 2)) + ((widthWall / numberOfBlocks) * (blockNumber - 1));
        else
            xBonusPosition = (widthWall / 2) - ((widthWall / (numberOfBlocks * 2)) + ((widthWall / numberOfBlocks) * (blockNumber - 1)));
        climbBonus.transform.position = new Vector3(xBonusPosition, climbBonus.transform.position.y, zBonusPosition);
        if (easyBlock)
            climbBonus.transform.localScale = new Vector3(easyClimbBonusRadius, climbBonus.transform.localScale.y, easyClimbBonusRadius);
        else
            climbBonus.transform.localScale = new Vector3(climbBonusRadius, climbBonus.transform.localScale.y, climbBonusRadius);
    }
    private void PutCrystalBonuses()
    {
        int numberOfCrystalBonus = 3;
        crystalsPosition = new Vector3[numberOfCrystalBonus];
        for (int i = 0; i < numberOfCrystalBonus; i++)
        {
            Vector3 bonusPosition = PlaceForNewCrystalBonus();
            if (!AnyBonusBeside(bonusPosition))
            {
                crystalBonus.transform.position = new Vector3(bonusPosition.x, crystalBonus.transform.position.y, bonusPosition.z);
                Instantiate(crystalBonus);
                crystalsPosition[i] = bonusPosition;
            }
        }
    }
    
    private Vector3 PlaceForNewCrystalBonus()
    {
        Vector3 position;
        int numberOfBlock = Random.Range(2, numberOfBlocks);
        float minDistanceToBlock = 0;
        if (easyBlock)
            minDistanceToBlock = 15f;
        else
        {
            numberOfBlock = Random.Range(2, numberOfBlocks);
            minDistanceToBlock = 10f;
        }
        float zPositionEdgeBlock = zCoordinateBeginningOfBlock + (widthWall / (numberOfBlocks * 2)) + ((widthWall / numberOfBlocks) * (numberOfBlock - 1)) - (widthWall / (numberOfBlocks * 2));
        float addDistanceToZposition = 2f;
        float zBonusPosition = zPositionEdgeBlock - (minDistanceToBlock + ((numberOfBlocks - numberOfBlock) * addDistanceToZposition));
        float xBonusPosition = 0;
        if (exitSide == Side.Right)
            xBonusPosition = -(widthWall / 2) + (widthWall / (numberOfBlocks * 2)) + ((widthWall / numberOfBlocks) * (numberOfBlock - 1));
        else
            xBonusPosition = (widthWall / 2) - ((widthWall / (numberOfBlocks * 2)) + ((widthWall / numberOfBlocks) * (numberOfBlock - 1)));
        position = new Vector3(xBonusPosition, 0, zBonusPosition);
        return position;
    }
}
