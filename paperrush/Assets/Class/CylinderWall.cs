using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Assets.Class
{
    class CylinderWall : MonoBehaviour
    {

        private float currentWallLength = 0;
        private float startPozition = 0;
        private int numberCylinderInHeights = 0;
        private float cylinderWidth = 250;
        private float cylinderMaxRandomWidthPart = 1;
        private float wallWidth = 0;
        private float wallHeight = 0;
        private float approximateDistanceOfWallPutting = 15;
        public float CurrentWallLength
        {
            get { return currentWallLength; }
        }
        public CylinderWall(float startZPosition, int numberOfCyclinder, float widthOfWall, float heightOfWall,float  maxRandomPart)
        {
            startPozition = startZPosition;
            numberCylinderInHeights = numberOfCyclinder;
            wallWidth = widthOfWall;
            wallHeight = heightOfWall;
            cylinderMaxRandomWidthPart = maxRandomPart;
        }
        public void Put()
        {
            //Initialization
            GameObject cylinder = Instantiate(Resources.Load("pref_CyclinderWallPart", typeof(GameObject))) as GameObject;
            float scaleXAndY = wallHeight / numberCylinderInHeights;
            cylinder.transform.localScale = new Vector3(scaleXAndY, scaleXAndY, cylinderWidth);
            //Put cylinder wall
            int numberCylinderInLengths = (int)System.Math.Round((approximateDistanceOfWallPutting / scaleXAndY)) + 1; 
            //Put on left side
            float positionXForLeftCylinders = -(wallWidth / 2) - (cylinderWidth / 2);
            for (int column = 0; column < numberCylinderInLengths; column++)
            {
                for (int row = 0; row < numberCylinderInHeights; row++)
                {
                    GameObject newCylinder = Instantiate(cylinder) as GameObject;
                    float randomPart = Random.Range(0f, cylinderMaxRandomWidthPart);
                    float positionX = positionXForLeftCylinders - randomPart;
                    float positionY = 1.2886f + (row * scaleXAndY);
                    float positionZ = startPozition + currentWallLength + (column * scaleXAndY);                 
                    newCylinder.transform.position = new Vector3(positionX, positionY, positionZ);
                }
            }
            //Put on right side
            float positionXForRightCylinders = (wallWidth / 2) + (cylinderWidth / 2);
            for (int column = 0; column < numberCylinderInLengths; column++)
            {
                for (int row = 0; row < numberCylinderInHeights; row++)
                {
                    GameObject newCylinder = Instantiate(cylinder) as GameObject;
                    float randomPart = Random.Range(0f, cylinderMaxRandomWidthPart);
                    float positionX = positionXForRightCylinders + randomPart;
                    float positionY = 1.2886f + (row * scaleXAndY);
                    float positionZ = startPozition + currentWallLength + (column * scaleXAndY);
                    newCylinder.transform.position = new Vector3(positionX, positionY, positionZ);
                }
            }
            currentWallLength = currentWallLength + (numberCylinderInLengths * scaleXAndY);
            Destroy(cylinder);
        }
        public void PutWithMoving()
        {
            //Initialization
            GameObject cylinder = Instantiate(Resources.Load("pref_CyclinderWallPart", typeof(GameObject))) as GameObject;
            float scaleXAndY = wallHeight / numberCylinderInHeights;
            cylinder.transform.localScale = new Vector3(scaleXAndY, scaleXAndY, cylinderWidth);
            //Put cylinder wall
            int numberCylinderInLengths = (int)System.Math.Round((approximateDistanceOfWallPutting / scaleXAndY)) + 1;
            //Put on left side
            float positionXForLeftCylinders = -(wallWidth / 2) - (cylinderWidth / 2);
            for (int column = 0; column < numberCylinderInLengths; column++)
            {
                for (int row = 0; row < numberCylinderInHeights; row++)
                {
                    GameObject newCylinder = Instantiate(cylinder) as GameObject;
                    float positionX = positionXForLeftCylinders;
                    float positionY = 1.2886f + (row * scaleXAndY);
                    float positionZ = startPozition + currentWallLength + (column * scaleXAndY);
                    newCylinder.transform.position = new Vector3(positionX, positionY, positionZ);
                    float randomPart = Random.Range(1f, cylinderMaxRandomWidthPart);
                    float leftPosition = positionX - randomPart;
                    newCylinder.transform.position = new Vector3(positionX, positionY, positionZ);
                    Sequence cylinderSequence = DOTween.Sequence();
                    float randomSpeed = Random.Range(0.3f, 0.6f);
                    cylinderSequence.Append(newCylinder.transform.DOMoveX(leftPosition, randomSpeed, false));
                    cylinderSequence.Append(newCylinder.transform.DOMoveX(positionX, randomSpeed, false));
                    cylinderSequence.SetLoops(50, LoopType.Restart).SetEase(Ease.Linear);
                }
            }
            //Put on right side
            float positionXForRightCylinders = (wallWidth / 2) + (cylinderWidth / 2);
            for (int column = 0; column < numberCylinderInLengths; column++)
            {
                for (int row = 0; row < numberCylinderInHeights; row++)
                {
                    GameObject newCylinder = Instantiate(cylinder) as GameObject;
                    float positionX = positionXForRightCylinders;
                    float positionY = 1.2886f + (row * scaleXAndY);
                    float positionZ = startPozition + currentWallLength + (column * scaleXAndY);
                    newCylinder.transform.position = new Vector3(positionX, positionY, positionZ);
                    float randomPart = Random.Range(1f, cylinderMaxRandomWidthPart);
                    float rightPosition = positionX + randomPart;
                    newCylinder.transform.position = new Vector3(positionX, positionY, positionZ);
                    Sequence cylinderSequence = DOTween.Sequence();
                    float randomSpeed = Random.Range(0.3f,0.6f);
                    cylinderSequence.Append(newCylinder.transform.DOMoveX(rightPosition, randomSpeed, false));
                    cylinderSequence.Append(newCylinder.transform.DOMoveX(positionX, randomSpeed, false));
                    cylinderSequence.SetLoops(50, LoopType.Restart).SetEase(Ease.Linear); ;
                }
            }
            currentWallLength = currentWallLength + (numberCylinderInLengths * scaleXAndY);
            Destroy(cylinder);
        }
    }
}
