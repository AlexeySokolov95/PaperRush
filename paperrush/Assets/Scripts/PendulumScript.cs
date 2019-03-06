using UnityEngine;
using Assets.Class;
using DG.Tweening;

public class PendulumScript : LevelBlock
{
    public float duration = 1;
    public float pozY = 10.5f;
    float blockLength = 40;
    public GameObject pendulum;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    public GameObject fakeBlock;
    bool obstacleMovingIsStarted = false;
    // Use this for initialization
    void Start()
    {
        Initialization(blockLength);
        PutWall();
        float pozZ = zCoordinateBeginningOfBlock + blockLength / 2;
        float pozX = 0;
        pendulum = Instantiate(pendulum);
        pendulum.transform.position = new Vector3(pozX,pozY,pozZ);
        //pendulum.transform.localEulerAngles = new Vector3(180, 0, 250);
        fakeBlock = Instantiate(fakeBlock);
        fakeBlock.transform.position = new Vector3(180, 0, 270);
        PutCrystalBonuses();
    }

    void Update()
    {
        if (PlayerIsNear(100) && !obstacleMovingIsStarted)
        {
            Sequence pendulumSequence = DOTween.Sequence();
            pendulumSequence.Append(fakeBlock.transform.DOMoveZ(90, duration).SetEase(Ease.InOutQuad));
            pendulumSequence.Append(fakeBlock.transform.DOMoveZ(270, duration).SetEase(Ease.InOutQuad));
            pendulumSequence.SetLoops(20);
            obstacleMovingIsStarted = true;
        }
        pendulum.transform.localEulerAngles = fakeBlock.transform.position;
    }
    private Vector3 PlaceForNewCrystalBonus()
    {
        Vector3 crystalPosition;
        float minXPos = (widthWall / 2) * 0.8f;
        float minZPos = zCoordinateBeginningOfBlock + 5;
        float maxZPos = zCoordinateBeginningOfBlock + blockLength - 5;
        float climbBonusXPosition = Random.Range(-minXPos, minXPos);
        float climbBonusZPosition = Random.Range(minZPos, maxZPos);
        crystalPosition = new Vector3(climbBonusXPosition,0, climbBonusZPosition);
        return crystalPosition;
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
}
