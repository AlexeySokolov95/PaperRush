using UnityEngine;
using Assets.Class;
using DG.Tweening;

namespace Assets.Scripts
{
    class ClosingWallScript : LevelBlock
    {
        public float crackWidth = 5;
        public float closingDuration = 1;
        public float blockLength = 50;
        private GameObject leftObstacle;
        private GameObject rightObstacle;
        private bool closingIsStarted = false;
        private float crackPosition;
        public GameObject crackWall;
        public GameObject climbBonusPref;
        public GameObject crystalBonus;
        void Start()
        {
            Initialization(blockLength);
            PutWall();
            leftObstacle = Instantiate(crackWall) as GameObject;
            rightObstacle = Instantiate(crackWall) as GameObject;
            leftObstacle.transform.localScale = new Vector3(widthWall, heightWall, leftObstacle.transform.localScale.z);
            rightObstacle.transform.localScale = new Vector3(widthWall, heightWall, rightObstacle.transform.localScale.z);
            crackPosition = 0;
            float obstacleZPosition = zCoordinateBeginningOfBlock + (blockLength / 2);
            leftObstacle.transform.position = new Vector3(-widthWall + 1, heightWall / 2, obstacleZPosition);
            rightObstacle.transform.position = new Vector3(widthWall - 1, heightWall / 2, obstacleZPosition);
            if(LevelManager.PutClimbBonus)
                PutClimbBonus();
            PutCrystalBonuses();
        }
        void Update()
        {
            if (!closingIsStarted && zCoordinateBeginningOfBlock - LevelManager.player.transform.position.z <= 100)
            {
                float endPositionLeftObstacle = -(widthWall / 2) + crackPosition - (crackWidth / 2);
                float startPositionLeftObstacle = -widthWall + 1;
                Sequence leftObstacleSequence = DOTween.Sequence();
                leftObstacleSequence.Append(leftObstacle.transform.DOMoveX(endPositionLeftObstacle, closingDuration, false));
                leftObstacleSequence.Append(leftObstacle.transform.DOMoveX(startPositionLeftObstacle, closingDuration, false));
                leftObstacleSequence.SetLoops(50, LoopType.Restart).SetEase(Ease.Linear);
                float endPositionRightObstacle = (widthWall / 2) + crackPosition + (crackWidth / 2);
                float startPositionRightObstacle = widthWall - 1;
                Sequence rightObstacleSequence = DOTween.Sequence();
                rightObstacleSequence.Append(rightObstacle.transform.DOMoveX(endPositionRightObstacle, closingDuration, false));
                rightObstacleSequence.Append(rightObstacle.transform.DOMoveX(startPositionRightObstacle, closingDuration, false));
                rightObstacleSequence.SetLoops(50, LoopType.Restart).SetEase(Ease.Linear);
                closingIsStarted = true;
            }
        }
        public void PutClimbBonus()
        {
            climbBonus = Instantiate(climbBonusPref) ;
            float distanceFromObstacle = 10;
            float distantZPosition = zCoordinateBeginningOfBlock + (blockLength / 2) - distanceFromObstacle;
            float distanceFromStartBlock = 5;
            float nearZPosisition = zCoordinateBeginningOfBlock + distanceFromStartBlock;
            float climbBonusZPosition = Random.Range(nearZPosisition, distantZPosition);
            float distanceFromWall = widthWall * 0.3f;
            float climbBonusXPosition = Random.Range((-widthWall / 2) + distanceFromWall, (widthWall / 2) - distanceFromWall);
            climbBonus.transform.position = new Vector3(climbBonusXPosition, climbBonus.transform.position.y, climbBonusZPosition);
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
            float distanceFromObstacle = 10;
            float distantZPosition = zCoordinateBeginningOfBlock + (blockLength / 2) - distanceFromObstacle;
            float distanceFromStartBlock = 5;
            float nearZPosisition = zCoordinateBeginningOfBlock + distanceFromStartBlock;
            float climbBonusZPosition = Random.Range(nearZPosisition, distantZPosition);
            float distanceFromWall = widthWall * 0.3f;
            float climbBonusXPosition = Random.Range((-widthWall / 2) + distanceFromWall, (widthWall / 2) - distanceFromWall);
            position = new Vector3(climbBonusXPosition, 0, climbBonusZPosition);
            return position;
        }
        private Vector3 PlaceForNewCrystalBonus()
        {
            Vector3 position;
            float distanceFromObstacle = 10;
            float distantZPosition = zCoordinateBeginningOfBlock + (blockLength / 2) - distanceFromObstacle;
            float distanceFromStartBlock = 5;
            float nearZPosisition = zCoordinateBeginningOfBlock + distanceFromStartBlock;
            float climbBonusZPosition = Random.Range(nearZPosisition, distantZPosition);
            float distanceFromWall = widthWall * 0.3f;
            float climbBonusXPosition = Random.Range((-widthWall / 2) + distanceFromWall, (widthWall / 2) - distanceFromWall);
            position = new Vector3(climbBonusXPosition, 0, climbBonusZPosition);
            return position;
        }
    }
}
