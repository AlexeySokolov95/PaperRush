using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class ZigZagScript : LevelBlock
{
    public int numberOfWalls = 6;
    public float distanceBetweenWalls = 20f;
    public float moreThanHalfWidthWall = 0.1f;
    Side currentObstacleSide;
    public GameObject zigZagBlock;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    void Start()
    {
        float blockLength = (numberOfWalls - 1) * distanceBetweenWalls;
        Initialization(blockLength);
        PutWall();
        GameObject zigZagWall = Instantiate(zigZagBlock);
        float newScaleXZigZagWall = widthWall / 2 + (widthWall * moreThanHalfWidthWall);
        zigZagWall.transform.localScale = new Vector3(newScaleXZigZagWall, heightWall, zigZagWall.transform.localScale.z);
        float halfHeightWall = zigZagWall.transform.localScale.y / 2;
        currentObstacleSide = (Side)Random.Range(0, 2);
        float positionZNewWall = 0f;
        while (positionZNewWall <= blockLength)
        {
            GameObject newZigZagWall = Instantiate(zigZagWall) as GameObject;
            if (currentObstacleSide == Side.Left)
            {
                float leftWallXCoordinates = -(widthWall / 2) + (newScaleXZigZagWall / 2);
                newZigZagWall.transform.position = new Vector3(leftWallXCoordinates, halfHeightWall, zCoordinateBeginningOfBlock + positionZNewWall);
                newZigZagWall.transform.localEulerAngles = new Vector3(0, 0, 0);
                currentObstacleSide = Side.Right;
            }
            else if (currentObstacleSide == Side.Right)
            {
                float rightWallXCoordinates = (widthWall / 2) - (newScaleXZigZagWall / 2);
                newZigZagWall.transform.position = new Vector3(rightWallXCoordinates, halfHeightWall, zCoordinateBeginningOfBlock + positionZNewWall);
                newZigZagWall.transform.localEulerAngles = new Vector3(0, 180, 0);
                currentObstacleSide = Side.Left;
            }
            positionZNewWall = positionZNewWall + distanceBetweenWalls;
        }
        Destroy(zigZagWall);
        PutClimbBonus();
        PutCrystalBonuses();
    }

    void Update()
    {

    }
    protected override void PutClimbBonus()
    {
        climbBonus = climbBonusPref;
        Vector3 bonusPosition = PlaceForNewClimbBonus();
        climbBonus.transform.position = new Vector3(bonusPosition.x, climbBonus.transform.position.y, bonusPosition.z);
        climbBonus = Instantiate(climbBonus);
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
    
    private Vector3 PlaceForNewClimbBonus()
    {
        Vector3 position;
        int centralWalls = numberOfWalls - 2;
        int climbBonusPlace = Random.Range(1, centralWalls + 1);
        //Search on which side to be the selected wall
        Side selectedWallSide = currentObstacleSide;
        int numberWallsFromTheEndToTheSelectedWall = numberOfWalls - climbBonusPlace;
        for (int i = 0; i < numberWallsFromTheEndToTheSelectedWall; i++)
        {
            if (selectedWallSide == Side.Left)
                selectedWallSide = Side.Right;
            else if (selectedWallSide == Side.Right)
                selectedWallSide = Side.Left;
        }
        //Put climb bonus
        float widthOfObstacleWall = widthWall / 2 + (widthWall * moreThanHalfWidthWall);
        float minDistanceFromWall = 8;
        float xCoordinatesOnObstacles = Random.Range(minDistanceFromWall, widthOfObstacleWall);
        float xCoordinatesOfClimbBonus = 0;
        if (selectedWallSide == Side.Left)
            xCoordinatesOfClimbBonus = -(widthWall / 2) + xCoordinatesOnObstacles;
        else if (selectedWallSide == Side.Right)
            xCoordinatesOfClimbBonus = +(widthWall / 2) - xCoordinatesOnObstacles;
        float zCoordinateOfSelectedObstacles = zCoordinateBeginningOfBlock + (climbBonusPlace * distanceBetweenWalls);
        float fractionalOfHeightTriangle = 0.5f;
        float zDistanceFromObstacle = distanceBetweenWalls * 0.4f;
        float zCoordinateOfClimbBonus = zCoordinateOfSelectedObstacles - ((widthOfObstacleWall - xCoordinatesOnObstacles) * fractionalOfHeightTriangle) - zDistanceFromObstacle;
        position = new Vector3(xCoordinatesOfClimbBonus, 0 , zCoordinateOfClimbBonus);
        return position;
    }
    private Vector3 PlaceForNewCrystalBonus()
    {
        Vector3 position;
        int centralWalls = numberOfWalls - 2;
        int climbBonusPlace = Random.Range(1, centralWalls + 1);
        //Search on which side to be the selected wall
        Side selectedWallSide = currentObstacleSide;
        int numberWallsFromTheEndToTheSelectedWall = numberOfWalls - climbBonusPlace;
        for (int i = 0; i < numberWallsFromTheEndToTheSelectedWall; i++)
        {
            if (selectedWallSide == Side.Left)
                selectedWallSide = Side.Right;
            else if (selectedWallSide == Side.Right)
                selectedWallSide = Side.Left;
        }
        //Put climb bonus
        float widthOfObstacleWall = widthWall / 2 + (widthWall * moreThanHalfWidthWall);
        float minDistanceFromWall = 5;
        float xCoordinatesOnObstacles = Random.Range(minDistanceFromWall, widthOfObstacleWall);
        float xCoordinatesOfClimbBonus = 0;
        if (selectedWallSide == Side.Left)
            xCoordinatesOfClimbBonus = -(widthWall / 2) + xCoordinatesOnObstacles;
        else if (selectedWallSide == Side.Right)
            xCoordinatesOfClimbBonus = +(widthWall / 2) - xCoordinatesOnObstacles;
        float zCoordinateOfSelectedObstacles = zCoordinateBeginningOfBlock + (climbBonusPlace * distanceBetweenWalls);
        float fractionalOfHeightTriangle = 0.5f;
        float zDistanceFromObstacle = distanceBetweenWalls * 0.3f;
        float zCoordinateOfClimbBonus = zCoordinateOfSelectedObstacles - ((widthOfObstacleWall - xCoordinatesOnObstacles) * fractionalOfHeightTriangle) - zDistanceFromObstacle;
        position = new Vector3(xCoordinatesOfClimbBonus, 0, zCoordinateOfClimbBonus);
        return position;
    }
}
enum Side { Left, Right }
