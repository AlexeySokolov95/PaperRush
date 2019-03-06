using System;
using UnityEngine;
using DG.Tweening;

namespace Assets.Class
{
    class ClosingWall : PartialLevelBlock
    {
        private float crackWidth = 5;
        private float closingSpeed = 5;
        private GameObject player;
        private GameObject leftObstacle;
        private GameObject rightObstacle;
        private bool closingIsStarted = false;
        private float crackPosition;
        public void Initialization(float zCoordinate, float width, float height, float crackSize, float closeSpeed)
        {
            startZCoordinate = zCoordinate;
            widthWall = width;
            heightWall = height;
            crackWidth = crackSize;
            closingSpeed = closeSpeed;
            endZCoordinate = zCoordinate + width;
        }
        void Start()
        {
            player = GameObject.FindWithTag("Player");
            leftObstacle = Instantiate(Resources.Load("prefCrackWall", typeof(GameObject))) as GameObject;
            rightObstacle = Instantiate(Resources.Load("prefCrackWall", typeof(GameObject))) as GameObject;
            leftObstacle.transform.localScale = new Vector3(widthWall, heightWall, leftObstacle.transform.localScale.z);
            rightObstacle.transform.localScale = new Vector3(widthWall, heightWall, rightObstacle.transform.localScale.z);
            crackPosition = 0;
            float obstacleZPosition = startZCoordinate + (Length / 2);
            leftObstacle.transform.position = new Vector3(-widthWall - 1, heightWall / 2, obstacleZPosition);
            rightObstacle.transform.position = new Vector3(widthWall + 1, heightWall / 2, obstacleZPosition);
        }
        void Update()
        {
            if (!closingIsStarted && startZCoordinate - player.transform.position.z <= 50)
            {
                float endPositionLeftObstacle = -(widthWall / 2) + crackPosition - (crackWidth / 2);
                leftObstacle.transform.DOMoveX(endPositionLeftObstacle, closingSpeed, false);
                float endPositionRightObstacle = (widthWall / 2) + crackPosition + (crackWidth / 2);
                rightObstacle.transform.DOMoveX(endPositionRightObstacle, closingSpeed, false);
                closingIsStarted = true;
            }
        }
    }
}
