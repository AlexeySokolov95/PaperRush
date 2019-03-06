using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class SharpTurnBlockScript : LevelBlock
{
    public float distanceBeetwenWalls = 3;
    public float crackWidth = 4;
    public int wallsNumber = 20;
    public int minNumberTurns = 1;
    public int maxNumberTurns = 3;
    public float corridorDistanceFromWall = 4;
    public float xLengthOfTurn = 6;
    public float zLengthOfTurn = 4;
    public GameObject corridorWall;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    private List<Vector2> corridorPoints;
    private List<Vector2> turnPoints = new List<Vector2>();
    private int numberOfTurn;
    private Side turnSide;
    private List<Side> sides = new List<Side>();
    // Use this for initialization
    void Start()
    {
        numberOfTurn = Random.Range(minNumberTurns, maxNumberTurns);
        float blockLength = (wallsNumber - 1) * distanceBeetwenWalls + (numberOfTurn * distanceBeetwenWalls * zLengthOfTurn);
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
        for (int turn = 0; turn <= numberOfTurn; turn++)
        {
            if (turnSide == Side.Left)
                turnSide = Side.Right;
            else
                turnSide = Side.Left;
        }
        for (int turn = 0; turn < turnPoints.Count; turn++)
        {
            Vector2 previousPoint = turnPoints[turn];
            for (int wall = 0; wall < zLengthOfTurn; wall++)
            {
                float pozXLeftWall;
                float pozXRightWall;
                if (sides[turn] == Side.Left)
                {
                    pozXLeftWall = previousPoint.x - (xLengthOfTurn) - (corridorWall.transform.localScale.x / 2) - (crackWidth / 2);
                    pozXRightWall = previousPoint.x + (corridorWall.transform.localScale.x / 2) + (crackWidth / 2);
                }
                else
                {
                    pozXLeftWall = previousPoint.x - (corridorWall.transform.localScale.x / 2) - (crackWidth / 2);
                    pozXRightWall = previousPoint.x + (xLengthOfTurn) + (corridorWall.transform.localScale.x / 2) + (crackWidth / 2);
                }
                float pozZWall = previousPoint.y + distanceBeetwenWalls;
                corridorWall.transform.position = new Vector3(pozXLeftWall, heightWall / 2, pozZWall);
                corridorWall.transform.localEulerAngles = new Vector3(0, 0, 0);
                Instantiate(corridorWall);
                corridorWall.transform.position = new Vector3(pozXRightWall, heightWall / 2, pozZWall);
                previousPoint = new Vector2(previousPoint.x, pozZWall);
                corridorWall.transform.localEulerAngles = new Vector3(0, 180, 0);
                Instantiate(corridorWall);
            }
        }
        PutClimbBonus();
        PutCrystalBonuses();
    }
    
    List<Vector2> NewCorridor()
    {
        List<Vector2> corridorPoints = new List<Vector2>();
        int numberOfPointInOneDirection = (wallsNumber - (int)xLengthOfTurn) / (numberOfTurn + 1);
        int remainder = (wallsNumber - (int)xLengthOfTurn) % (numberOfTurn + 1) + (int)xLengthOfTurn;
        float pozXStartPoint = Random.Range((-widthWall / 2) + corridorDistanceFromWall, (widthWall / 2) - corridorDistanceFromWall);
        Vector2 startPoint = new Vector2(pozXStartPoint, zCoordinateBeginningOfBlock);
        Vector2 previousPoint = startPoint;
        turnSide = (Side)Random.Range(0, 2);
        for (int turn = 0; turn <= numberOfTurn; turn++)
        {           
            for (int point = 0; point < numberOfPointInOneDirection; point++)
            {
                float yPoz = previousPoint.y + distanceBeetwenWalls;
                float xPoz = previousPoint.x;
                Vector2 newPoint = new Vector2(xPoz, yPoz);
                corridorPoints.Add(newPoint);
                previousPoint = newPoint;
            }
            turnPoints.Add(previousPoint);
            previousPoint.y += zLengthOfTurn * distanceBeetwenWalls;
            if (turnSide == Side.Left)
            {
                if (previousPoint.x - xLengthOfTurn < (-widthWall / 2) + corridorDistanceFromWall)
                    turnSide = Side.Right;
                else
                    previousPoint.x -= xLengthOfTurn;
                if (turnSide == Side.Right)
                    previousPoint.x += xLengthOfTurn;
            }
            else if( turnSide == Side.Right )
            {
                if (previousPoint.x + xLengthOfTurn > (widthWall / 2) - corridorDistanceFromWall)
                    turnSide = Side.Left;
                else
                    previousPoint.x += xLengthOfTurn;
                if (turnSide == Side.Left)
                    previousPoint.x -= xLengthOfTurn;
            }
            sides.Add(turnSide);
            turnSide = (Side)Random.Range(0, 2);
        }
        for (int point = 0; point < remainder; point++)
        {
            float yPoz = previousPoint.y + distanceBeetwenWalls;
            float xPoz = previousPoint.x;
            Vector2 newPoint = new Vector2(xPoz, yPoz);
            corridorPoints.Add(newPoint);
            previousPoint = newPoint;
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
