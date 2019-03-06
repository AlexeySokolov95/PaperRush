using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

namespace Assets.Class
{
    public class CrackInWallScript : LevelBlock
    {
        float crackPosition;
        public float closingSpeed = 25f;
        public float blockLength = 75;
        public float crackWidth = 6;
        public float crackMinDistanceFromWall = 0;
        void Start()
        {
            Initialization(blockLength);
            PutWall();
            crackPosition = Random.Range((-widthWall / 2) + crackMinDistanceFromWall, (widthWall / 2) - crackMinDistanceFromWall);
            float positionZNewWall = zCoordinateBeginningOfBlock + (lengthOfMainWall / 2);
            elements.Add(Instantiate(Resources.Load("prefCrackWall", typeof(GameObject))) as GameObject);
            elements.Add(Instantiate(Resources.Load("prefCrackWall", typeof(GameObject))) as GameObject);
            elements[0].transform.transform.localScale = new Vector3(widthWall, heightWall, elements[0].transform.localScale.z);
            elements[1].transform.transform.localScale = new Vector3(widthWall, heightWall, elements[1].transform.localScale.z);
            elements[0].transform.position = new Vector3(0 - widthWall + 1.01f, heightWall / 2, positionZNewWall);
            elements[1].transform.position = new Vector3(0 + widthWall - 1.01f, heightWall / 2, positionZNewWall);
            PutClimbBonus();
        }
        void Update()
        {
            //If player is close, close walls
            if (zCoordinateBeginningOfBlock - LevelManager.GetComponent<LevelCreater>().player.transform.position.z < 110)
            {
                bool crackWallIsClose = elements[0].transform.position.x + widthWall / 2 == crackPosition - (crackWidth / 2) &&
                    elements[1].transform.position.x - widthWall / 2 == crackPosition + (crackWidth / 2);
                if (!crackWallIsClose)
                {
                    if (elements[0].transform.position.x + (widthWall / 2) <= crackPosition - (crackWidth / 2))
                        elements[0].transform.Translate(new Vector3(closingSpeed * Time.deltaTime, 0, 0));
                    else
                        elements[0].transform.position = new Vector3(crackPosition - (crackWidth / 2), elements[0].transform.position.y, elements[0].transform.position.z);
                    if (elements[1].transform.position.x - (widthWall / 2) >= crackPosition + (crackWidth / 2))
                        elements[1].transform.Translate(new Vector3(-closingSpeed * Time.deltaTime, 0, 0));
                    else
                        elements[1].transform.position = new Vector3(crackPosition + (crackWidth / 2), elements[0].transform.position.y, elements[1].transform.position.z);
                }
            }
        }
        protected override void PutClimbBonus()
        {
            GameObject climbBonus = Instantiate(Resources.Load("bns_Climb1", typeof(GameObject)) as GameObject);
            //Сhoose between the right and left wall.
            Side selectedWallSide = (Side)Random.Range(0,2);
            if(selectedWallSide == Side.Left)
            {
                float widthLeftCrackWall = (widthWall / 2) + crackPosition - (crackWidth / 2);
                float xCoordinatesOnWall = Random.Range(crackMinDistanceFromWall, widthLeftCrackWall);
                float xCoordinatesOfClimbBonus = -(widthWall / 2) + xCoordinatesOnWall;
                float zCoordinateOfSelectedObstacles = zCoordinateBeginningOfBlock + (lengthOfMainWall / 2);
                float fractionalOfHeightTriangle = 0.5f;
                float zDistanceFromObstacle = 3;
                float zCoordinateOfClimbBonus = zCoordinateOfSelectedObstacles - ((widthLeftCrackWall - xCoordinatesOnWall) * fractionalOfHeightTriangle) - zDistanceFromObstacle;
                climbBonus.transform.position = new Vector3(xCoordinatesOfClimbBonus, climbBonus.transform.position.y, zCoordinateOfClimbBonus);
            }
            else if(selectedWallSide == Side.Right)
            {
                float widthRightCrackWall = (widthWall / 2) + crackPosition + (crackWidth / 2);
                float xCoordinatesOnWall = Random.Range(crackMinDistanceFromWall, widthRightCrackWall);
                float xCoordinatesOfClimbBonus = +(widthWall / 2) - xCoordinatesOnWall;
                float zCoordinateOfSelectedObstacles = zCoordinateBeginningOfBlock + (lengthOfMainWall / 2);
                float fractionalOfHeightTriangle = 0.5f;
                float zDistanceFromObstacle = 3;
                float zCoordinateOfClimbBonus = zCoordinateOfSelectedObstacles - ((widthRightCrackWall - xCoordinatesOnWall) * fractionalOfHeightTriangle) - zDistanceFromObstacle;
                climbBonus.transform.position = new Vector3(xCoordinatesOfClimbBonus, climbBonus.transform.position.y, zCoordinateOfClimbBonus);
            }
        }
        /*
    {
        float distanceBetweenWalls = 20f;
        walls.Add(Instantiate(Resources.Load("pref_SimpleWalls 1 1", typeof(GameObject))) as GameObject);
        length = 100f;
        walls[0].transform.position = new Vector3(levelXPosition, 0, startPosition);
        float positionZNewWall = 0f;
        float widthWall = 50f;
        float heightWall = 15f;
        float heightBlock = 25f;
        while (positionZNewWall < 100)
        {
            float crackPosition = Random.Range(-21, 21);
            elements.Add(Instantiate(Resources.Load("prefCrackWall", typeof(GameObject))) as GameObject);
            elements.Add(Instantiate(Resources.Load("prefCrackWall", typeof(GameObject))) as GameObject);
            elements[0].transform.position = new Vector3(levelXPosition - widthWall + 1.01f, heightWall / 2, startPosition + positionZNewWall - (length / 2));
            elements[1].transform.position = new Vector3(levelXPosition + widthWall - 1.01f, heightWall / 2, startPosition + positionZNewWall - (length / 2));                

        }

    }
    private IEnumerator CloseCrackInWall(float startTime)
    {
        float closeSpeed = 0.1f;
        float crackCoordinate = FindCrackCoordinate();
        yield return new WaitForSeconds(startTime);
        float displacement = Time.deltaTime * closeSpeed;
        if()
        elements[0].transform.Translate(new Vector3(Displacement(),0,0));
        yield return null;

    }
    private float FindCrackPosition(float widthWall)
    {
        float crackPosition = Random.Range(4, 46);
        Random.Range(2,50);
    }*/
    }
}
