using UnityEngine;
using Assets.Class;

public class ALotOfObstaclesScript : LevelBlock
{
    public float sphereHeight = 1.5f;
    public float sphereWidth = 1.5f;
    public float widthOffset = 0.5f;
    public float heightOffset = 0.5f;
    public float sizeOffset = 1f;
    public float cellHeight = 2f;
    public float cellWidth = 2f;
    public float cellLength = 2f;
    public float blockLength = 70;
    public float probability = 0.68f;

    public GameObject sphere;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    void Start()
    {
        Initialization(blockLength);
        PutWall();
        //Sphere settings
       
        float widthRemainder = widthWall % cellWidth;
        int gridWidth = (int)((widthWall - widthRemainder) / cellWidth);
        float heightRemainder = heightWall % cellHeight;
        int gridHeight = (int)((heightWall - heightRemainder) / cellHeight) + 1;
        float lengthRemainder = blockLength % cellLength;
        int gridLength = (int)((blockLength - lengthRemainder) / cellLength);
        //Create a grid of obstacles
        for (int widthCoord = 0; widthCoord < gridWidth; widthCoord++)
        {
            for (int heightCoord = 0; heightCoord < gridHeight; heightCoord++)
            {
                for (int lengthCoord = 0; lengthCoord < gridLength; lengthCoord++)
                {
                    if ((widthCoord + heightCoord + lengthCoord) % 2 == 0)
                    {
                        bool blockIsExist;
                        if (Random.Range(0f, 1f) < probability)
                            blockIsExist = true;
                        else
                            blockIsExist = false;
                        if (blockIsExist)
                        {
                            float xCoord = -(widthWall / 2) + (widthRemainder / 2) + (widthCoord * cellWidth) + (cellWidth / 2) + Random.Range(-widthOffset, widthOffset);
                            float yCoord = (heightRemainder / 2) + (heightCoord * cellHeight) + Random.Range(-heightOffset, heightOffset);
                            if (yCoord - sphere.transform.localScale.y < 0)
                                yCoord = sphere.transform.localScale.y + 0.2f;
                            float zCoord = zCoordinateBeginningOfBlock + (lengthRemainder / 2) + (lengthCoord * cellLength);
                            sphere.transform.position = new Vector3(xCoord, yCoord, zCoord);
                            float offset = Random.Range(-sizeOffset, sizeOffset);
                            sphere.transform.localScale = new Vector3(sphereHeight + offset, sphereHeight + offset, sphereHeight + offset);
                            Instantiate(sphere);
                        }
                    }
                }
            }
        }
        PutCrystalBonuses();
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

    void Update()
    {

    }
}
