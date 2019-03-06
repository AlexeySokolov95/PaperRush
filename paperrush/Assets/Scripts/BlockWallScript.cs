using UnityEngine;
using Assets.Class;
using DG.Tweening;

public class BlockWallScript : LevelBlock
{
    public int numberBlocks = 3;
    public float passageWidth = 4;
    public float movingDuration = 1;
    public float areaMoving = 15;
    public float blockLength = 25;
    private float blockWallBlockScaleX;  
    
    private GameObject[] blocks;
    public GameObject blockWallBlock;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    void Start()
    {
        Initialization(blockLength);
        PutWall();
        blocks = new GameObject[numberBlocks];
        GameObject block = Instantiate(blockWallBlock);
        blockWallBlockScaleX = (widthWall - ((numberBlocks - 1) * passageWidth)) / 3;
        block.transform.localScale = new Vector3(blockWallBlockScaleX, heightWall, blockWallBlockScaleX);
        float distantPosition = zCoordinateBeginningOfBlock + (blockLength / 2) + (areaMoving / 2);
        float nearPosition = zCoordinateBeginningOfBlock + (blockLength / 2) - (areaMoving / 2);
        for (int i = 0; i < numberBlocks; i++)
        {
            float positionX = (-widthWall / 2) + (blockWallBlockScaleX / 2) + (passageWidth * i) + (blockWallBlockScaleX * i);
            GameObject newBlock = Instantiate(block);
            if (i % 2 == 0)
            {
                newBlock.transform.position = new Vector3(positionX, heightWall / 2, distantPosition);
                Sequence newBlockSequence = DOTween.Sequence();
                newBlockSequence.Append(newBlock.transform.DOMoveZ(nearPosition, movingDuration, false));
                newBlockSequence.Append(newBlock.transform.DOMoveZ(distantPosition, movingDuration, false));
                newBlockSequence.SetLoops(30, LoopType.Restart).SetEase(Ease.Linear);
            }
            else
            {
                newBlock.transform.position = new Vector3(positionX, heightWall / 2, nearPosition);
                Sequence newBlockSequence = DOTween.Sequence();
                newBlockSequence.Append(newBlock.transform.DOMoveZ(distantPosition, movingDuration, false));
                newBlockSequence.Append(newBlock.transform.DOMoveZ(nearPosition, movingDuration, false));
                newBlockSequence.SetLoops(30, LoopType.Restart).SetEase(Ease.Linear);
            }
        }
        if (LevelManager.PutClimbBonus)
            PutClimbBonus();
        PutCrystalBonuses();
        Destroy(block);
    }
    void Update()
    {

    }
    protected override void PutClimbBonus()
    {
        climbBonus = Instantiate(climbBonusPref);
        if (!easyBlock)
        {
            ClimbBonusPositionOnBlockWall position = (ClimbBonusPositionOnBlockWall)Random.Range(0, 2);
            if (position == ClimbBonusPositionOnBlockWall.BeforeBlocks)
            {
                float distanceFromObstacle = 10;
                float distantZPosition = zCoordinateBeginningOfBlock + (blockLength / 2 - (areaMoving / 2)) - distanceFromObstacle;
                float nearZPosisition = zCoordinateBeginningOfBlock;
                float climbBonusZPosition = Random.Range(nearZPosisition, distantZPosition);
                float distanceFromWall = 3;
                float climbBonusXPosition = Random.Range((-widthWall / 2) + distanceFromWall, (widthWall / 2) - distanceFromWall);
                climbBonus.transform.position = new Vector3(climbBonusXPosition, climbBonus.transform.position.y, climbBonusZPosition);
            }
            else
            {
                int numberOfPassage = Random.Range(0, numberBlocks - 1);
                float centrXPositionOfPassage = (-widthWall / 2) + blockWallBlockScaleX + (passageWidth / 2) + ((blockWallBlockScaleX + passageWidth) * numberOfPassage);
                float distantZPositionClimbBonus = zCoordinateBeginningOfBlock + (blockLength / 2) + (areaMoving / 2);
                float nearZPositionClimbBonus = zCoordinateBeginningOfBlock + (blockLength / 2) - (areaMoving / 2);
                float climbBonusZPosition = Random.Range(nearZPositionClimbBonus, nearZPositionClimbBonus);
                climbBonus.transform.position = new Vector3(centrXPositionOfPassage, climbBonus.transform.position.y, climbBonusZPosition);
            }
        }
        else
        {
            int numberOfPassage = Random.Range(0, numberBlocks - 1);
            float centrXPositionOfPassage = (-widthWall / 2) + blockWallBlockScaleX + (passageWidth / 2) + ((blockWallBlockScaleX + passageWidth) * numberOfPassage);
            float distantZPositionClimbBonus = zCoordinateBeginningOfBlock + (blockLength / 2) + (areaMoving / 2);
            float nearZPositionClimbBonus = zCoordinateBeginningOfBlock + (blockLength / 2) - (areaMoving / 2);
            float climbBonusZPosition = Random.Range(nearZPositionClimbBonus, nearZPositionClimbBonus);
            climbBonus.transform.position = new Vector3(centrXPositionOfPassage, climbBonus.transform.position.y, climbBonusZPosition);
            climbBonus.transform.localScale = new Vector3(climbBonusRadius, climbBonus.transform.localScale.y, climbBonusRadius);
        }
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
        Vector3 crystalPosition;
        ClimbBonusPositionOnBlockWall position = (ClimbBonusPositionOnBlockWall)Random.Range(0, 2);
        float climbBonusXPosition = 0;
        float climbBonusZPosition = 0;
        if (position == ClimbBonusPositionOnBlockWall.BeforeBlocks)
        {
            float distanceFromObstacle = 10;
            float distantZPosition = zCoordinateBeginningOfBlock + (blockLength / 2 - (areaMoving / 2)) - distanceFromObstacle;
            float nearZPosisition = zCoordinateBeginningOfBlock;
            climbBonusZPosition = Random.Range(nearZPosisition, distantZPosition);
            float distanceFromWall = 3;
            climbBonusXPosition = Random.Range((-widthWall / 2) + distanceFromWall, (widthWall / 2) - distanceFromWall);
        }
        else
        {
            int numberOfPassage = Random.Range(0, numberBlocks - 1);
            climbBonusXPosition = (-widthWall / 2) + blockWallBlockScaleX + (passageWidth / 2) + ((blockWallBlockScaleX + passageWidth) * numberOfPassage);
            float distantZPositionClimbBonus = zCoordinateBeginningOfBlock + (blockLength / 2) + (areaMoving / 2);
            float nearZPositionClimbBonus = zCoordinateBeginningOfBlock + (blockLength / 2) - (areaMoving / 2);
            climbBonusZPosition = Random.Range(nearZPositionClimbBonus, nearZPositionClimbBonus);
        }
        crystalPosition = new Vector3(climbBonusXPosition, 0, climbBonusZPosition);
        return crystalPosition;
    }
}
enum ClimbBonusPositionOnBlockWall {BeforeBlocks, BeetwenBlocks }

