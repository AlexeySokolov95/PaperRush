using UnityEngine;
using Assets.Class;

public class CogwheelScript : LevelBlock
{
    public float rotationSpeed;
    public float wheelSize = 28f;
    public GameObject cogwheel;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    public float blockLength = 40;
    Side rotationSde;
    // Use this for initialization
    void Start()
    {
        Initialization(blockLength);
        PutWall();
        rotationSde = (Side)Random.Range(0,2);
        cogwheel.transform.position = new Vector3(0, -1.5f, zCoordinateBeginningOfBlock + (blockLength / 2));
        cogwheel.transform.localScale = new Vector3(wheelSize, wheelSize,1);
        cogwheel = Instantiate(cogwheel);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rotationSde == Side.Right)
            cogwheel.transform.Rotate(new Vector3(0,0, -rotationSpeed));
        else
            cogwheel.transform.Rotate(new Vector3(0, 0, rotationSpeed));
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
        Vector3 crystalPosition;
        float minZPos = zCoordinateBeginningOfBlock + 5;
        float maxZPos = zCoordinateBeginningOfBlock + blockLength - 5;
        float climbBonusZPosition = Random.Range(minZPos, maxZPos);
        float minXPos = (widthWall / 2) * 0.8f;
        float climbBonusXPosition = Random.Range(-minXPos, minXPos);
        crystalPosition = new Vector3(climbBonusXPosition, 0, climbBonusZPosition);
        return crystalPosition;
    }
}
