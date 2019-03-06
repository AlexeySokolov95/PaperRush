using UnityEngine;
using Assets.Class;

public class Trap3BlockScript : LevelBlock
{
    public float lengthBeetwenWalls = 30;
    public float widthEnter = 5;
    public float bonusDistanceFromWall = 6;
    public GameObject obstacleWall;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    private float sideOfSquare = 0;
    void Start()
    {
        sideOfSquare = (20 - (widthEnter*2)) / 2;
        float blockLength = (sideOfSquare * 2) + lengthBeetwenWalls;
        Initialization(blockLength);
        PutWall();
        float enterWallLength = Mathf.Sqrt((sideOfSquare * sideOfSquare) + (sideOfSquare * sideOfSquare));
        obstacleWall.transform.localScale = new Vector3(enterWallLength, heightWall, 1);

        float enterLeftWallPozX = -(sideOfSquare / 2) + 0.35f;
        float enterLeftWallPozZ = zCoordinateBeginningOfBlock + sideOfSquare / 2;
        obstacleWall.transform.position = new Vector3(enterLeftWallPozX, heightWall / 2, enterLeftWallPozZ);
        obstacleWall.transform.localEulerAngles = new Vector3(0, 225, 0);
        Instantiate(obstacleWall);

        float enterRigthWallPozX = sideOfSquare / 2 - 0.35f;
        float enterRigthWallPozZ = enterLeftWallPozZ;
        obstacleWall.transform.position = new Vector3(enterRigthWallPozX, heightWall / 2, enterRigthWallPozZ);
        obstacleWall.transform.localEulerAngles = new Vector3(0, -45, 0);
        Instantiate(obstacleWall);
        ExitType exitType = (ExitType)Random.Range(0, 2);
        if (exitType == ExitType.OnTheSides)
        {         
            float exitLeftWallPozX = -(sideOfSquare / 2);
            float exitLeftWallPozZ = zCoordinateBeginningOfBlock + sideOfSquare + lengthBeetwenWalls + sideOfSquare / 2;
            obstacleWall.transform.position = new Vector3(exitLeftWallPozX, heightWall / 2, exitLeftWallPozZ);
            obstacleWall.transform.localEulerAngles = new Vector3(0, -224, 0);
            Instantiate(obstacleWall);

            float exitRigthWallPozX = sideOfSquare / 2;
            float exitRigthWallPozZ = exitLeftWallPozZ;
            obstacleWall.transform.position = new Vector3(exitRigthWallPozX, heightWall / 2, exitRigthWallPozZ);
            obstacleWall.transform.localEulerAngles = new Vector3(0, 45, 0);
            Instantiate(obstacleWall);
        }
        else if(exitType == ExitType.Center)
        {
            float exitLeftWallPozX = -(sideOfSquare / 2 + (widthEnter / 2)) + 0.35f;
            float exitLeftWallPozZ = zCoordinateBeginningOfBlock + sideOfSquare + lengthBeetwenWalls + sideOfSquare / 2;
            obstacleWall.transform.position = new Vector3(exitLeftWallPozX, heightWall / 2, exitLeftWallPozZ);
            obstacleWall.transform.localEulerAngles = new Vector3(0, 45, 0);
            Instantiate(obstacleWall);

            float exitRigthWallPozX = sideOfSquare / 2 + (widthEnter / 2) - 0.35f;
            float exitRigthWallPozZ = exitLeftWallPozZ;
            obstacleWall.transform.position = new Vector3(exitRigthWallPozX, heightWall / 2, exitRigthWallPozZ);
            obstacleWall.transform.localEulerAngles = new Vector3(0, -225, 0);
            Instantiate(obstacleWall);
        }
        PutClimbBonus();
        PutCrystalBonuses();
    }
    protected override void PutClimbBonus()
    {
        climbBonus = Instantiate(climbBonusPref);
        float xBonusPosition = Random.Range(-widthWall / 2 + bonusDistanceFromWall, widthWall / 2 - bonusDistanceFromWall);
        float zBonusPosition = zCoordinateBeginningOfBlock + sideOfSquare + (lengthBeetwenWalls / 2) + 4;
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
        float xBonusPosition = Random.Range(-widthWall / 2 + bonusDistanceFromWall, widthWall / 2 - bonusDistanceFromWall);
        float distanceFromObstacle = 7;
        float zBonusPosition = Random.Range(zCoordinateBeginningOfBlock + sideOfSquare + distanceFromObstacle, zCoordinateBeginningOfBlock + sideOfSquare + lengthBeetwenWalls - distanceFromObstacle);
        position = new Vector3(xBonusPosition, 0, zBonusPosition);
        return position;
    }
    enum ExitType { Center, OnTheSides }
}


