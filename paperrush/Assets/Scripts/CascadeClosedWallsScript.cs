using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class CascadeClosedWallsScript : LevelBlock
{
    List<Vector3> crackPositions = new List<Vector3>();
    public int numberOfWalls = 4;
    public float distanceBetweenWalls = 30f;
    public float crackWidth = 0.5f;
    public float crackMinDistanceFromWall = 2;
    public float climbDeltaX = 2;
    public GameObject crackWall;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    void Start()
    {
        float blockLength = (numberOfWalls - 1) * distanceBetweenWalls;
        Initialization(blockLength);
        PutWall();
        GameObject obstacleWall = Instantiate(crackWall);
        obstacleWall.transform.localScale = new Vector3(widthWall, heightWall, 1);
        float positionZNewWall = zCoordinateBeginningOfBlock;
        for (int i = 0; i < numberOfWalls; i++)
        {
            float crackXPosition = Random.Range((-widthWall / 2) + crackMinDistanceFromWall + 3, (widthWall / 2) - crackMinDistanceFromWall - 3);
            GameObject leftObstacle = Instantiate(obstacleWall) as GameObject;
            GameObject rightObstacle = Instantiate(obstacleWall) as GameObject;
            leftObstacle.transform.position = new Vector3(-(widthWall / 2) + crackXPosition - (crackWidth / 2), heightWall / 2, positionZNewWall);
            leftObstacle.AddComponent<ClosingWallInCascadeScript>();
            leftObstacle.transform.localEulerAngles = new Vector3(0, 0, 0);
            rightObstacle.transform.position = new Vector3((widthWall / 2) + crackXPosition + (crackWidth / 2), heightWall / 2, positionZNewWall);
            rightObstacle.AddComponent<ClosingWallInCascadeScript>();
            rightObstacle.transform.localEulerAngles = new Vector3(0, 180, 0);
            Vector3 crackPos = new Vector3(crackXPosition, 0, positionZNewWall);
            crackPositions.Add(crackPos);
            positionZNewWall = positionZNewWall + distanceBetweenWalls;
        }
        Destroy(obstacleWall);
        PutClimbBonus();
        PutCrystalBonuses();
    }
    protected override void PutClimbBonus()
    {
        climbBonus = Instantiate(climbBonusPref);
        int numCrack = Random.Range(1, crackPositions.Count -1);
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
        int numCrack = Random.Range(2, crackPositions.Count);
        Vector3 crack = crackPositions[numCrack];
        float pozXCrystalBonus = Random.Range(crack.x - climbDeltaX, crack.x + climbDeltaX);
        float pozZCrystalBonus = crack.z - (distanceBetweenWalls * 0.38f);
        position = new Vector3(pozXCrystalBonus, 0, pozZCrystalBonus);
        return position;
    }
}