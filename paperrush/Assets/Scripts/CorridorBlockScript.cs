using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class CorridorBlockScript : LevelBlock
{
    public float distanceBeetwenWalls = 3;
    public float crackWidth = 4;
    public int wallsNumber = 20;
    public int minNumberInflectionPoints = 1;
    public int maxNumberInflectionPoints = 3;
    public float corridorDistanceFromWall = 4;
    public float xLengthOfTurn = 6;
    public GameObject corridorWall;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    private List<Vector2> corridorPoints;
    // Use this for initialization
    void Start()
    {
        float blockLength = (wallsNumber - 1) * distanceBeetwenWalls;
        Initialization(blockLength);
        PutWall();
        corridorWall.transform.localScale = new Vector3(widthWall, heightWall, 1);
        corridorPoints = NewCorridor();
        foreach (var point in corridorPoints)
        {
            float pozXLeftWall = point.x - (corridorWall.transform.localScale.x / 2) - (crackWidth / 2);
            corridorWall.transform.position = new Vector3(pozXLeftWall, heightWall / 2, point.y);
            corridorWall.transform.localEulerAngles = new Vector3(0, 0, 0);
            Instantiate(corridorWall);
            float pozXRightWall = point.x + (corridorWall.transform.localScale.x / 2) + (crackWidth / 2);
            corridorWall.transform.position = new Vector3(pozXRightWall, heightWall / 2, point.y);
            corridorWall.transform.localEulerAngles = new Vector3(0, 180, 0);
            Instantiate(corridorWall);
        }
        
        PutClimbBonus();
        PutCrystalBonuses();
    }

    List<Vector2> NewCorridor()
    {
        List<Vector2> corridorPoints = new List<Vector2>();
        int numberOfInflectionPoints = Random.Range(minNumberInflectionPoints, maxNumberInflectionPoints);
        int numberOfPointInOneDirection = wallsNumber / (numberOfInflectionPoints + 1);
        int remainder = wallsNumber % (numberOfInflectionPoints + 1);
        float pozXStartPoint = Random.Range((-widthWall / 2) + corridorDistanceFromWall, (widthWall / 2) - corridorDistanceFromWall);
        Vector2 startPoint = new Vector2(pozXStartPoint, zCoordinateBeginningOfBlock);
        float middleDelta = xLengthOfTurn / numberOfPointInOneDirection;
        Side turnSide = (Side)Random.Range(0, 2);
        Vector2 previousPoint = startPoint;
        for (int turn = 0; turn <= numberOfInflectionPoints; turn++)
        {
            for (int point = 0; point < numberOfPointInOneDirection; point++)
            {
                float deltaX = middleDelta * (1f - (1f / ((float)numberOfPointInOneDirection / 2)) + ((1f / (float)numberOfPointInOneDirection) * (float)point));
                float xPoz = 0;
                if (turnSide == Side.Left)
                    xPoz = previousPoint.x - deltaX;
                else
                    xPoz = previousPoint.x + deltaX;
                float yPoz = previousPoint.y + distanceBeetwenWalls;
                if (xPoz < (-widthWall / 2) + corridorDistanceFromWall)
                    xPoz = (-widthWall / 2) + corridorDistanceFromWall;
                if (xPoz > (widthWall / 2) - corridorDistanceFromWall)
                    xPoz = (widthWall / 2) - corridorDistanceFromWall;
                Vector2 newPoint = new Vector2(xPoz, yPoz);
                corridorPoints.Add(newPoint);
                previousPoint = newPoint;
            }
            if (turnSide == Side.Left)
                turnSide = Side.Right;
            else
                turnSide = Side.Left;
        }
        for (int point = 1; point < remainder + 1; point++)
        {
            if (turnSide == Side.Left)
            {
                float deltaX = middleDelta * (1f - (1f / ((float)numberOfPointInOneDirection / 2)) + ((1f / (float)numberOfPointInOneDirection) * (float)point));
                float xPoz = previousPoint.x - deltaX;
                float yPoz = previousPoint.y + distanceBeetwenWalls;
                if (xPoz < (-widthWall / 2) + corridorDistanceFromWall)
                    xPoz = (-widthWall / 2) + corridorDistanceFromWall;
                if (xPoz > (widthWall / 2) - corridorDistanceFromWall)
                    xPoz = (widthWall / 2) - corridorDistanceFromWall;
                Vector2 newPoint = new Vector2(xPoz, yPoz);
                corridorPoints.Add(newPoint);
                previousPoint = newPoint;
            }
            else if (turnSide == Side.Right)
            {
                float deltaX = middleDelta * (1f - (1f / ((float)numberOfPointInOneDirection / 2)) + ((1f / (float)numberOfPointInOneDirection) * (float)point));
                float xPoz = previousPoint.x + deltaX;
                float yPoz = previousPoint.y + distanceBeetwenWalls;
                if (xPoz < (-widthWall / 2) + corridorDistanceFromWall)
                    xPoz = (-widthWall / 2) + corridorDistanceFromWall;
                if (xPoz > (widthWall / 2) - corridorDistanceFromWall)
                    xPoz = (widthWall / 2) - corridorDistanceFromWall;
                Vector2 newPoint = new Vector2(xPoz, yPoz);
                corridorPoints.Add(newPoint);
                previousPoint = newPoint;
            }
        }
        return corridorPoints;
    }
    protected override void PutClimbBonus()
    {
        climbBonus = Instantiate(climbBonusPref);
        int countOfClimbPoint = Random.Range(corridorPoints.Count / 2 - 4, corridorPoints.Count / 2 + 4);
        Vector2 climbPoint = corridorPoints[countOfClimbPoint];
        climbBonus.transform.position = new Vector3(climbPoint.x, climbBonus.transform.position.y, climbPoint.y);
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
        int countOfCrystalPoint = Random.Range(0 + 4, corridorPoints.Count - 4);
        Vector2 crystalPoint = corridorPoints[countOfCrystalPoint];
        position = new Vector3(crystalPoint.x, 0, crystalPoint.y);
        return position;
    }
}
