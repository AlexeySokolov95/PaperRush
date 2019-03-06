using UnityEngine;
using Assets.Class;
using DG.Tweening;

public class MovingCrackInWallBlockScript : LevelBlock
{
    public float crackWidth = 5;
    public float movingDuration = 2;
    public float distanceFromWall = 5;
    public float blockLength = 10;
    public GameObject crackWall;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    private GameObject leftWall;
    private GameObject rightWall;
    void Start()
    {
        Initialization(blockLength);
        PutWall();
        crackWall.transform.localScale = new Vector3(widthWall, heightWall, 1);
        float pozZWall = zCoordinateBeginningOfBlock + (blockLength / 2);

        float startingXPozXLeftWall = -widthWall + distanceFromWall;
        float endingXPozXLeftWall = widthWall - distanceFromWall - crackWidth - widthWall;
        crackWall.transform.position = new Vector3(startingXPozXLeftWall, heightWall/2, pozZWall);
        leftWall = Instantiate(crackWall);
        leftWall.transform.localEulerAngles = new Vector3(0, 0, 0);

        Sequence leftObstacleSequence = DOTween.Sequence();
        leftObstacleSequence.Append(leftWall.transform.DOMoveX(endingXPozXLeftWall, movingDuration, false));
        leftObstacleSequence.Append(leftWall.transform.DOMoveX(startingXPozXLeftWall, movingDuration, false));
        leftObstacleSequence.SetLoops(20, LoopType.Restart).SetEase(Ease.Linear);

        float startingPozXRightWall = -widthWall + distanceFromWall + crackWidth + widthWall;
        float endingPozXRightWall = widthWall - distanceFromWall;
        crackWall.transform.position = new Vector3(startingPozXRightWall, heightWall / 2, pozZWall);
        rightWall = Instantiate(crackWall);
        rightWall.transform.localEulerAngles = new Vector3(0, 180, 0);

        Sequence rightObstacleSequence = DOTween.Sequence();
        rightObstacleSequence.Append(rightWall.transform.DOMoveX(endingPozXRightWall, movingDuration, false));
        rightObstacleSequence.Append(rightWall.transform.DOMoveX(startingPozXRightWall, movingDuration, false));
        rightObstacleSequence.SetLoops(20, LoopType.Restart).SetEase(Ease.Linear);
        if (LevelManager.PutClimbBonus)
            PutClimbBonus();
        PutCrystalBonuses();
    }
    public void PutClimbBonus()
    {
        climbBonus = Instantiate(climbBonusPref);
        float distanceFromObstacle = 15;
        float distantZPosition = zCoordinateBeginningOfBlock + (blockLength / 2) - distanceFromObstacle;
        float distanceFromWall = widthWall * 0.3f;
        float climbBonusXPosition = Random.Range((-widthWall / 2) + distanceFromWall, (widthWall / 2) - distanceFromWall);
        climbBonus.transform.position = new Vector3(climbBonusXPosition, climbBonus.transform.position.y, distantZPosition);
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
        float distanceFromObstacle = 10;
        float nearZPosition = zCoordinateBeginningOfBlock + (blockLength / 2) - distanceFromObstacle;
        float distanceFromStartBlock = 5;
        float distantZPosisition = zCoordinateBeginningOfBlock + (blockLength / 2) - distanceFromStartBlock;
        float distanceFromWall = widthWall * 0.3f;
        float crystalZPosition = Random.Range(nearZPosition, distantZPosisition);
        float climbBonusXPosition = Random.Range((-widthWall / 2) + distanceFromWall, (widthWall / 2) - distanceFromWall);
        position = new Vector3(climbBonusXPosition, 0, crystalZPosition);
        return position;
    }
}

