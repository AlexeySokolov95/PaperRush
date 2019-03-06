using UnityEngine;
using Assets.Class;

public class DoubleDiagonalWallBlockScript : LevelBlock
{
    public int numberOfBlocks = 5;
    public int blocksForExit = 1;
    public float distanceBeetwenWalls = 10;
    public float blockLength = 70;
    private Side exitSide;
    public GameObject columnBlock;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    void Start()
    {
        Initialization(blockLength);
        PutWall();
        GameObject zigBlock = Instantiate(columnBlock);
        zigBlock.transform.localScale = new Vector3(widthWall / numberOfBlocks, heightWall, widthWall / numberOfBlocks);
        exitSide = (Side)Random.Range(0, 2);
        for (int blockNumber = 1; blockNumber <= numberOfBlocks - blocksForExit; blockNumber++)
            PutBlockForFirstWall(zigBlock, blockNumber);
        if (exitSide == Side.Left)
            exitSide = Side.Right;
        else if (exitSide == Side.Right)
            exitSide = Side.Left;
        for (int blockNumber = 1; blockNumber <= numberOfBlocks - blocksForExit; blockNumber++)
            PutBlockForSecondWall(zigBlock, blockNumber);
        if (LevelManager.PutClimbBonus)
            PutClimbBonus();
        PutCrystalBonuses();
    }
    void PutBlockForFirstWall(GameObject zigBlock, int blockNumber)
    {
        float xPosition;
        if (exitSide == Side.Right)
            xPosition = -(widthWall / 2) + (widthWall / (numberOfBlocks * 2)) + ((widthWall / numberOfBlocks) * (blockNumber - 1));
        else
            xPosition = (widthWall / 2) - ((widthWall / (numberOfBlocks * 2)) + ((widthWall / numberOfBlocks) * (blockNumber - 1)));
        float yPosition = heightWall / 2;
        float zPosition = zCoordinateBeginningOfBlock + (widthWall / (numberOfBlocks * 2)) - ((widthWall / numberOfBlocks) * (blockNumber - 1));
        zigBlock.transform.position = new Vector3(xPosition, yPosition, zPosition);
        GameObject newZigBlock = Instantiate(zigBlock) as GameObject;
    }
    void PutBlockForSecondWall(GameObject zigBlock, int blockNumber)
    {
        float xPosition;
        if (exitSide == Side.Right)
            xPosition = -(widthWall / 2) + (widthWall / (numberOfBlocks * 2)) + ((widthWall / numberOfBlocks) * (blockNumber - 1));
        else
            xPosition = (widthWall / 2) - ((widthWall / (numberOfBlocks * 2)) + ((widthWall / numberOfBlocks) * (blockNumber - 1)));
        float yPosition = heightWall / 2;
        float zPosition = zCoordinateBeginningOfBlock + distanceBeetwenWalls + (widthWall / (numberOfBlocks * 2)) - ((widthWall / numberOfBlocks) * (blockNumber - 1));
        zigBlock.transform.position = new Vector3(xPosition, yPosition, zPosition);
        GameObject newZigBlock = Instantiate(zigBlock) as GameObject;
    }
    protected override void PutClimbBonus()
    {
        BonusPozition climbPozition = (BonusPozition)Random.Range(0, 2);
        if (climbPozition == BonusPozition.Front)
        {
            if (exitSide == Side.Left)
                exitSide = Side.Right;
            else if (exitSide == Side.Right)
                exitSide = Side.Left;
            climbBonus = Instantiate(climbBonusPref);
            int blockNumber = Random.Range(3, numberOfBlocks);
            float minDistanceToBlock = 10f;
            float zPositionEdgeBlock = zCoordinateBeginningOfBlock + (widthWall / (numberOfBlocks * 2)) - ((widthWall / numberOfBlocks) * (blockNumber - 1)) - (widthWall / (numberOfBlocks * 2));
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
        else if(climbPozition == BonusPozition.Inside)
        {
            climbBonus = Instantiate(climbBonusPref);
            int blockNumber = numberOfBlocks - blocksForExit;
            float zPozClimbBonus = zCoordinateBeginningOfBlock + (widthWall / (numberOfBlocks * 2)) - ((widthWall / numberOfBlocks) * (blockNumber - 1)) + (distanceBeetwenWalls / 1.5f);
            float minPozX = -widthWall / 2 * 0.25f;
            float maxPozX = widthWall / 2 * 0.25f;
            float pozXClimbBonus = Random.Range(minPozX, maxPozX);
            climbBonus.transform.position = new Vector3(pozXClimbBonus, climbBonus.transform.position.y, zPozClimbBonus);
        }
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
        Vector3 position =  new Vector3();
        BonusPozition crystalPozition = (BonusPozition)Random.Range(0, 2);
        if (crystalPozition == BonusPozition.Front)
        {
            if (exitSide == Side.Left)
                exitSide = Side.Right;
            else if (exitSide == Side.Right)
                exitSide = Side.Left;
            int blockNumber = Random.Range(3, numberOfBlocks);
            float minDistanceToBlock = 10f;
            float zPositionEdgeBlock = zCoordinateBeginningOfBlock + (widthWall / (numberOfBlocks * 2)) - ((widthWall / numberOfBlocks) * (blockNumber - 1)) - (widthWall / (numberOfBlocks * 2));
            float addDistanceToZposition = 2f;
            float zBonusPosition = zPositionEdgeBlock - (minDistanceToBlock + ((numberOfBlocks - blockNumber) * addDistanceToZposition));
            float xBonusPosition = 0;
            if (exitSide == Side.Right)
                xBonusPosition = -(widthWall / 2) + (widthWall / (numberOfBlocks * 2)) + ((widthWall / numberOfBlocks) * (blockNumber - 1));
            else
                xBonusPosition = (widthWall / 2) - ((widthWall / (numberOfBlocks * 2)) + ((widthWall / numberOfBlocks) * (blockNumber - 1)));
            position = new Vector3(xBonusPosition, 0, zBonusPosition);
        }
        else if (crystalPozition == BonusPozition.Inside)
        {
            int blockNumber = numberOfBlocks - blocksForExit;
            float zPozClimbBonus = zCoordinateBeginningOfBlock + (widthWall / (numberOfBlocks * 2)) - ((widthWall / numberOfBlocks) * (blockNumber - 1)) + (distanceBeetwenWalls / 1.5f);
            float minPozX = -widthWall / 2 * 0.25f;
            float maxPozX = widthWall / 2 * 0.25f;
            float pozXClimbBonus = Random.Range(minPozX, maxPozX);
            position = new Vector3(pozXClimbBonus, 0, zPozClimbBonus);
        }
        return position;
    }
    enum BonusPozition { Front, Inside }
}
