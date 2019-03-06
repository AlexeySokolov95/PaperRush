using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

//Two stage: central and lateral obstacles.
enum RhombCorridorStage { Central, Lateral }
public class RhombCorridorScript : LevelBlock
{
    public int numberOfStages = 6;
    public float distanceBeetwenStage = 20.0f;
    public float centralObstacleWidth = 0.6f;
    public float lateralObstacleWidth = 0.3f;
    public float obstacleThickness = 0.5f;
    private RhombCorridorStage rhombStage = RhombCorridorStage.Central;
    public GameObject centralObstaclePref;
    public GameObject lateralObstaclePref;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    void Start()
    {
        float blockLength = (numberOfStages - 1) * distanceBeetwenStage;
        Initialization(blockLength);
        PutWall();
        //Set scales of obstacles
        GameObject centralObstacle = Instantiate(centralObstaclePref);
        centralObstacle.transform.localScale = new Vector3(centralObstacleWidth * widthWall, heightWall, obstacleThickness);
        GameObject lateralObstacle = Instantiate(lateralObstaclePref);
        lateralObstacle.transform.localScale = new Vector3(lateralObstacleWidth * widthWall, heightWall, obstacleThickness);
        //Put obstacles on the level
        float currentObstaclesZCoordinate = 0;
        while(currentObstaclesZCoordinate <= blockLength)
        {
            if(rhombStage == RhombCorridorStage.Central)
            {
                GameObject newCentralObstacle = Instantiate(centralObstacle) as GameObject;
                float yCoordinatesOfObstacle = newCentralObstacle.transform.localScale.y / 2;
                float zCoordinatesOfObstacle = zCoordinateBeginningOfBlock + currentObstaclesZCoordinate;
                newCentralObstacle.transform.position = new Vector3(0, yCoordinatesOfObstacle, zCoordinatesOfObstacle);
                
                rhombStage = RhombCorridorStage.Lateral;
            }
            else
            {
                //Put left obstacle of lateral stage
                GameObject leftObstacle = Instantiate(lateralObstacle) as GameObject;
                float xCoordinatesOfLeftObstacle =  0 - (widthWall / 2) + (lateralObstacle.transform.localScale.x  / 2);
                float yCoordinatesOfLeftObstacle = leftObstacle.transform.localScale.y / 2;
                float zCoordinatesOfLeftObstacle = zCoordinateBeginningOfBlock + currentObstaclesZCoordinate;
                leftObstacle.transform.position = new Vector3(xCoordinatesOfLeftObstacle, yCoordinatesOfLeftObstacle, 
                    zCoordinatesOfLeftObstacle);
                leftObstacle.transform.localEulerAngles = new Vector3(0, 0, 0);
                //Put right obstacle of lateral stage
                GameObject rightObstacle = Instantiate(lateralObstacle) as GameObject;
                float xCoordinatesOfRightObstacle = 0 + (widthWall / 2) - (lateralObstacle.transform.localScale.x / 2);
                float yCoordinatesOfRightObstacle = rightObstacle.transform.localScale.y / 2;
                float zCoordinatesOfRightObstacle = zCoordinateBeginningOfBlock + currentObstaclesZCoordinate;
                rightObstacle.transform.position = new Vector3(xCoordinatesOfRightObstacle, yCoordinatesOfRightObstacle,
                    zCoordinatesOfRightObstacle);
                rightObstacle.transform.localEulerAngles = new Vector3(0, 180, 0);
                rhombStage = RhombCorridorStage.Central;
            }
            currentObstaclesZCoordinate += distanceBeetwenStage;
        }
        Destroy(centralObstacle);
        Destroy(lateralObstacle);
        PutClimbBonus();
        PutCrystalBonuses();
    }

    void Update()
    {

    }
    protected override void PutClimbBonus()
    {
        //Initialization of climb bonus.
        climbBonus = Instantiate(climbBonusPref);
        //Search place for a bonus
        /*(switch (climbPlace)
        {
            case ClimbBonusPlace.LeftObstacle:
                break;
            case ClimbBonusPlace.RightObstacle:
                break;
            case ClimbBonusPlace.CentralObstacle:
                break;
        }*/
        
        Vector3 newPosition = PlaceForNewBonus();
        climbBonus.transform.position = new Vector3(newPosition.x, climbBonus.transform.position.y, newPosition.z);
        if (easyBlock)
            climbBonus.transform.localScale = new Vector3(easyClimbBonusRadius, climbBonus.transform.localScale.y, easyClimbBonusRadius);
        else
            climbBonus.transform.localScale = new Vector3(climbBonusRadius, climbBonus.transform.localScale.y, climbBonusRadius);
    }
    private bool IsOdd(int number)
    {
        bool answer = false;
        if (number % 2 != 0)
            answer = true;
        return answer;
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
    private Vector3 PlaceForNewBonus()
    {
        Vector3 position;
        int centralStages = numberOfStages - 2;
        int climbBonusPlace = Random.Range(1, centralStages + 1);
        float xCoordinatesOfClimbBonus = 0;
        float zCoordinateOfClimbBonus = 0;
        if (IsOdd(climbBonusPlace))
        {
            //Put bonus on right or left Obstacle.
            Side ObstacleSide = (Side)Random.Range(0, 2);
            float numericalWidthLateralObstacle = lateralObstacleWidth * widthWall;
            if (ObstacleSide == Side.Left)
            {
                float xCoordinatesOnObstacles = Random.Range(3, numericalWidthLateralObstacle);
                xCoordinatesOfClimbBonus = -(widthWall / 2) + xCoordinatesOnObstacles;
                float zCoordinateOfSelectedObstacles = zCoordinateBeginningOfBlock + (climbBonusPlace * distanceBeetwenStage);
                float fractionalOfHeightTriangle = 1.2f;
                zCoordinateOfClimbBonus = zCoordinateOfSelectedObstacles - ((numericalWidthLateralObstacle - xCoordinatesOnObstacles) * fractionalOfHeightTriangle) - 3;
            }
            else
            {
                float xCoordinatesOnObstacles = Random.Range(3, numericalWidthLateralObstacle);
                xCoordinatesOfClimbBonus = +(widthWall / 2) - xCoordinatesOnObstacles;
                float zCoordinateOfSelectedObstacles = zCoordinateBeginningOfBlock + (climbBonusPlace * distanceBeetwenStage);
                float fractionalOfHeightTriangle = 1.2f;
                zCoordinateOfClimbBonus = zCoordinateOfSelectedObstacles - ((numericalWidthLateralObstacle - xCoordinatesOnObstacles) * fractionalOfHeightTriangle) - 3;                
            }
        }
        else
        {
            float numericalWidthCentralObstacle = centralObstacleWidth * widthWall;
            float fractionalPartOfWidthCentralObstacleForClimbBonus = 0.6f;
            float numericalPartOfWidthCentralObstacleForClimbBonus = numericalWidthCentralObstacle * fractionalPartOfWidthCentralObstacleForClimbBonus;
            xCoordinatesOfClimbBonus = Random.Range(0, numericalWidthCentralObstacle) - (numericalWidthCentralObstacle / 2);
            float zCoordinateOfSelectedObstacles = zCoordinateBeginningOfBlock + (climbBonusPlace * distanceBeetwenStage);
            float fractionalOfHeightTriangle = 0.5f;
            float minDistanceFromObstacle = 3;
            zCoordinateOfClimbBonus = zCoordinateOfSelectedObstacles - ((numericalWidthCentralObstacle - Mathf.Abs(xCoordinatesOfClimbBonus)) * fractionalOfHeightTriangle) - minDistanceFromObstacle;         
        }
        position = new Vector3(xCoordinatesOfClimbBonus, 0, zCoordinateOfClimbBonus);
        return position;
    }
    private Vector3 PlaceForNewCrystalBonus()
    {
        Vector3 position;
        int centralStages = numberOfStages - 2;
        int climbBonusPlace = Random.Range(1, centralStages + 1);
        float xCoordinatesOfClimbBonus = 0;
        float zCoordinateOfClimbBonus = 0;
        if (IsOdd(climbBonusPlace))
        {
            //Put bonus on right or left Obstacle.
            Side ObstacleSide = (Side)Random.Range(0, 2);
            float numericalWidthLateralObstacle = lateralObstacleWidth * widthWall;
            if (ObstacleSide == Side.Left)
            {
                float xCoordinatesOnObstacles = Random.Range(3, numericalWidthLateralObstacle);
                xCoordinatesOfClimbBonus = -(widthWall / 2) + xCoordinatesOnObstacles;
                float zCoordinateOfSelectedObstacles = zCoordinateBeginningOfBlock + (climbBonusPlace * distanceBeetwenStage);
                float fractionalOfHeightTriangle = 1.2f;
                zCoordinateOfClimbBonus = zCoordinateOfSelectedObstacles - ((numericalWidthLateralObstacle - xCoordinatesOnObstacles) * fractionalOfHeightTriangle) - 3;
            }
            else
            {
                float xCoordinatesOnObstacles = Random.Range(3, numericalWidthLateralObstacle);
                xCoordinatesOfClimbBonus = +(widthWall / 2) - xCoordinatesOnObstacles;
                float zCoordinateOfSelectedObstacles = zCoordinateBeginningOfBlock + (climbBonusPlace * distanceBeetwenStage);
                float fractionalOfHeightTriangle = 1.2f;
                zCoordinateOfClimbBonus = zCoordinateOfSelectedObstacles - ((numericalWidthLateralObstacle - xCoordinatesOnObstacles) * fractionalOfHeightTriangle) - 3;
            }
        }
        else
        {
            float minDistanceFromWall = 3;
            float coordinateXForLeftBorder =  - widthWall / 2 + minDistanceFromWall;
            float coordinateXForRightBorder = + widthWall / 2 - minDistanceFromWall;
            xCoordinatesOfClimbBonus = Random.Range(coordinateXForLeftBorder, coordinateXForRightBorder);
            float minDistanceFromObstacle = 5;
            float zCoordinateOfSelectedObstacles = zCoordinateBeginningOfBlock + (climbBonusPlace * distanceBeetwenStage);
            float coordinateZForDistantBorder = zCoordinateOfSelectedObstacles - distanceBeetwenStage + minDistanceFromObstacle;
            float coordinateZForCloserBorder = zCoordinateOfSelectedObstacles - minDistanceFromObstacle;
            zCoordinateOfClimbBonus = Random.Range(coordinateZForDistantBorder, coordinateZForCloserBorder);
        }
        position = new Vector3(xCoordinatesOfClimbBonus, 0, zCoordinateOfClimbBonus);
        return position;
    }
}

