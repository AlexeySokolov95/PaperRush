
using UnityEngine;
using Assets.Class;
using DG.Tweening;

public class ClosingDiagonalWallScript : LevelBlock
{
    public float crackHeight = 4;
    public float crackHeightDelta = 3;
    public float movingDuration = 1;
    public float blockLength = 50;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    float errorMultiplier = 2;
    public float YPozitionErrorMult = -0.20f;
    float XPozitionError = 0f;
    Side diagonalSide;

    void Start()
    {
        Initialization(blockLength);
        PutWall();
        rightWall = Instantiate(rightWall);
        leftWall = Instantiate(leftWall);

        float triangleWallHeight = heightWall / errorMultiplier - (crackHeightDelta / errorMultiplier);
        float triangleWallWight = (widthWall / errorMultiplier);
        float triangleWallLength = 1 / errorMultiplier;
        leftWall.transform.localScale = new Vector3(triangleWallWight, triangleWallLength, triangleWallHeight);
        rightWall.transform.localScale = new Vector3(triangleWallWight, triangleWallLength, triangleWallHeight);
        diagonalSide = (Side)Random.Range(0, 2);
        if (diagonalSide == Side.Left)
        {
            float startYPozLeftWall = (heightWall * YPozitionErrorMult) + (heightWall / 2) + crackHeight;
            float startXPozLeftWall = XPozitionError;
            float startZPozLeftWall = zCoordinateBeginningOfBlock + (blockLength / 2);
            leftWall.transform.position = new Vector3(startXPozLeftWall, startYPozLeftWall, startZPozLeftWall);
            Sequence leftWallSequence = DOTween.Sequence();
            float leftWallEndPozition = startXPozLeftWall - widthWall;
            leftWallSequence.Append(leftWall.transform.DOMoveX(leftWallEndPozition, movingDuration, false));
            leftWallSequence.Append(leftWall.transform.DOMoveX(startXPozLeftWall, movingDuration, false));
            leftWallSequence.SetLoops(50, LoopType.Restart).SetEase(Ease.Linear);

            float startYPozRightWall = (heightWall * YPozitionErrorMult) + heightWall / 2;
            float startXPozRightWall = XPozitionError;
            float startZPozRightWall = zCoordinateBeginningOfBlock + (blockLength / 2);
            rightWall.transform.position = new Vector3(startXPozRightWall, startYPozRightWall, startZPozRightWall);
            Sequence rightWallSequence = DOTween.Sequence();
            float rightWallEndPozition = startXPozLeftWall + widthWall;
            rightWallSequence.Append(rightWall.transform.DOMoveX(rightWallEndPozition, movingDuration, false));
            rightWallSequence.Append(rightWall.transform.DOMoveX(startXPozRightWall, movingDuration, false));
            rightWallSequence.SetLoops(50, LoopType.Restart).SetEase(Ease.Linear);
        }
        else if (diagonalSide == Side.Right)
        {


            float startYPozLeftWall = (heightWall * YPozitionErrorMult) + (heightWall / 2);
            float startXPozLeftWall = XPozitionError;
            float startZPozLeftWall = zCoordinateBeginningOfBlock + (blockLength / 2);
            rightWall.transform.position = new Vector3(startXPozLeftWall, startYPozLeftWall, startZPozLeftWall);
            rightWall.transform.localEulerAngles = new Vector3(90, 180, 0);

            Sequence leftWallSequence = DOTween.Sequence();
            float leftWallEndPozition = startXPozLeftWall - widthWall;
            leftWallSequence.Append(rightWall.transform.DOMoveX(leftWallEndPozition, movingDuration, false));
            leftWallSequence.Append(rightWall.transform.DOMoveX(startXPozLeftWall, movingDuration, false));
            leftWallSequence.SetLoops(50, LoopType.Restart).SetEase(Ease.Linear);

            float startYPozRightWall = (heightWall * YPozitionErrorMult) + (heightWall / 2) + crackHeight;
            float startXPozRightWall = XPozitionError;
            float startZPozRightWall = zCoordinateBeginningOfBlock + (blockLength / 2);
            leftWall.transform.position = new Vector3(startXPozRightWall, startYPozRightWall, startZPozRightWall);
            leftWall.transform.localEulerAngles = new Vector3(-90, 0, 0);

            Sequence rightWallSequence = DOTween.Sequence();
            float rightWallEndPozition = startXPozLeftWall + widthWall;
            rightWallSequence.Append(leftWall.transform.DOMoveX(rightWallEndPozition, movingDuration, false));
            rightWallSequence.Append(leftWall.transform.DOMoveX(startXPozRightWall, movingDuration, false));
            rightWallSequence.SetLoops(50, LoopType.Restart).SetEase(Ease.Linear);
        }
        if (LevelManager.PutClimbBonus)
            PutClimbBonus();
        PutCrystalBonuses();
    }
    public void PutClimbBonus()
    {
        climbBonus = Instantiate(climbBonusPref);
        float distanceFromObstacle = 15;
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
    void Update()
    {

    }
}
