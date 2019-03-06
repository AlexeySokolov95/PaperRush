using UnityEngine;
using Assets.Class;
using DG.Tweening;

public class LongObstaclesScript : LevelBlock
{
    public float obstacleHeight = 1.5f;
    public float obstacleWidth = 1.5f;
    public float cellHeight = 2f;
    public float cellWidth = 2f;
    public float probability = 0.68f;
    public float blockLength = 40;
    public GameObject[] longObstacles;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    private GameObject curentLongObstacle;
    // Use this for initialization
    void Start()
    {
        Initialization(blockLength);
        PutWall();
        //Obstacle settings
        int curentLongObstacleNumber = Random.Range(0, longObstacles.Length);
        curentLongObstacle = longObstacles[curentLongObstacleNumber];
        curentLongObstacle.transform.localScale = new Vector3(obstacleHeight, obstacleWidth, blockLength);
        //Grid
        float widthRemainder = widthWall % cellWidth;
        int gridWidth = (int)((widthWall - widthRemainder) / cellWidth );
        float heightRemainder = heightWall % cellHeight;
        int gridHeight = (int)((heightWall - heightRemainder) / cellHeight);
        //Create a grid of obstacles
        float zCoord = zCoordinateBeginningOfBlock + (blockLength / 2);
        for(int widthCoord = 0; widthCoord < gridWidth; widthCoord++)
        {
            for(int heightCoord = 0; heightCoord < gridHeight; heightCoord++)
            {
               /* bool blockIsExist;
                if (Random.Range(0f, 1f) < probability)
                    blockIsExist = true;
                else
                    blockIsExist = false;
                if(blockIsExist)
                {*/
                    if ((widthCoord + heightCoord) % 2 == 0)
                    {
                        float xCoord = -(widthWall / 2) + (widthRemainder / 2) + (widthCoord * cellWidth) + (cellWidth / 2);
                        float yCoord = (heightRemainder / 2) + (heightCoord * cellHeight);
                        curentLongObstacle.transform.position = new Vector3(xCoord, yCoord, zCoord);
                        Instantiate(curentLongObstacle);
                    }
                //}
            }
            PutCrystalBonuses();
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
        Vector3 crystalPosition;
        float minZPos = zCoordinateBeginningOfBlock + 5;
        float maxZPos = zCoordinateBeginningOfBlock + blockLength - 5;
        float climbBonusZPosition = Random.Range(minZPos, maxZPos);
        float minXPos = (widthWall / 2) * 0.8f;
        float climbBonusXPosition = Random.Range(-minXPos, minXPos);
        crystalPosition = new Vector3(climbBonusXPosition, 0, climbBonusZPosition);
        return crystalPosition;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
