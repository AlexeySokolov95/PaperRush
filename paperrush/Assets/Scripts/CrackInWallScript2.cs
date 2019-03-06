using UnityEngine;
using Assets.Class;
using System.Collections.Generic;

public class CrackInWallScript2 : LevelBlock
{
    List<Vector3> crackPositions = new List<Vector3>();
    float crackXPosition;
    public int numberOfWalls = 4;
    public float distanceBetweenWalls = 30f;
    public float closingSpeed = 25f;
    public float crackWidth = 4;
    public float climbDeltaX = 2;
    public float crackMinDistanceFromWall = 4;
    public GameObject crackWall;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    void Start()
    {
        float blockLength = (numberOfWalls - 1) * distanceBetweenWalls;
        Initialization(blockLength);
        PutWall();
        GameObject obstacleWall  = Instantiate(crackWall);
        obstacleWall.transform.localScale  = new Vector3(widthWall, heightWall, obstacleWall.transform.localScale.z);
        float positionZNewWall = 0f;
        while (positionZNewWall <= blockLength)
        {
            crackXPosition = Random.Range((-widthWall / 2) + crackMinDistanceFromWall, (widthWall / 2) - crackMinDistanceFromWall);
            GameObject leftObstacle  = Instantiate(obstacleWall) as GameObject;
            GameObject rightObstacle = Instantiate(obstacleWall) as GameObject;
            leftObstacle.transform.position  = new Vector3(-(widthWall / 2) + crackXPosition - (crackWidth / 2), heightWall / 2, zCoordinateBeginningOfBlock + positionZNewWall);
            leftObstacle.transform.localEulerAngles = new Vector3(0, 0, 0);
            rightObstacle.transform.position = new Vector3((widthWall / 2) + crackXPosition + (crackWidth / 2), heightWall / 2, zCoordinateBeginningOfBlock + positionZNewWall);
            rightObstacle.transform.localEulerAngles = new Vector3(0, 180, 0);
            Vector3 crackPos = new Vector3(crackXPosition, 0, zCoordinateBeginningOfBlock + positionZNewWall);
            crackPositions.Add(crackPos);
            positionZNewWall = positionZNewWall + distanceBetweenWalls;
        }
        Destroy(obstacleWall);
        PutClimbBonus();
        PutCrystalBonuses();
    }
    void Update()
    {
        
    }
    protected override void PutClimbBonus()
    {
        climbBonus = Instantiate(climbBonusPref);
        int numCrack = Random.Range(1, crackPositions.Count - 1);
        Vector3 crack = crackPositions[numCrack];
        float pozXClimbBonus = Random.Range(crack.x - climbDeltaX, crack.x + climbDeltaX);
        float pozZClimbBonus = crack.z - (distanceBetweenWalls * 0.38f);
        climbBonus.transform.position = new Vector3(pozXClimbBonus, climbBonus.transform.position.y, pozZClimbBonus);
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
        int centralWalls = numberOfWalls - 2;
        int climbBonusPlace = Random.Range(1, centralWalls + 1);
        //Сhoose between the right and left wall.
        Side selectedWallSide = (Side)Random.Range(0, 2);
        float xCoordinatesOfClimbBonus = 0;
        float zCoordinateOfClimbBonus = 0;
        if (selectedWallSide == Side.Left)
        {
            float widthLeftCrackWall = (widthWall / 2) + crackXPosition - (crackWidth / 2);
            float xCoordinatesOnWall = Random.Range(crackMinDistanceFromWall, widthLeftCrackWall);
            xCoordinatesOfClimbBonus = -(widthWall / 2) + xCoordinatesOnWall;
            float zCoordinateOfSelectedObstacles = zCoordinateBeginningOfBlock + (climbBonusPlace * distanceBetweenWalls);
            float fractionalOfHeightTriangle = 0.5f;
            float zDistanceFromObstacle = distanceBetweenWalls * 0.4f;
            zCoordinateOfClimbBonus = zCoordinateOfSelectedObstacles - ((widthLeftCrackWall - xCoordinatesOnWall) * fractionalOfHeightTriangle) - zDistanceFromObstacle;
        }
        else if (selectedWallSide == Side.Right)
        {
            float widthRightCrackWall = (widthWall / 2) + crackXPosition + (crackWidth / 2);
            float xCoordinatesOnWall = Random.Range(crackMinDistanceFromWall, widthRightCrackWall);
            xCoordinatesOfClimbBonus = +(widthWall / 2) - xCoordinatesOnWall;
            float zCoordinateOfSelectedObstacles = zCoordinateBeginningOfBlock + (climbBonusPlace * distanceBetweenWalls);
            float fractionalOfHeightTriangle = 0.5f;
            float zDistanceFromObstacle = distanceBetweenWalls * 0.4f;
            zCoordinateOfClimbBonus = zCoordinateOfSelectedObstacles - ((widthRightCrackWall - xCoordinatesOnWall) * fractionalOfHeightTriangle) - zDistanceFromObstacle;
        }
        position = new Vector3(xCoordinatesOfClimbBonus, 0, zCoordinateOfClimbBonus);
        return position;
    }
}
