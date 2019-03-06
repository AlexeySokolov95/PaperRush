using UnityEngine;
using Assets.Class;

public class TrapInsideBlockScript : LevelBlock
{
    public float lengthBeetwenWalls = 30;
    public float widthEnter = 5;
    public float minZBonus = 12;
    public float maxZBonus = 11;
    public GameObject enterWall;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    private float sideOfSquare = 0;
    void Start()
    {
        sideOfSquare = (20 - widthEnter) / 2;
        float blockLength = (sideOfSquare * 2) + lengthBeetwenWalls;
        Initialization(blockLength);
        PutWall();
        float enterWallLength = Mathf.Sqrt((sideOfSquare * sideOfSquare) + (sideOfSquare * sideOfSquare));
        enterWall.transform.localScale = new Vector3(enterWallLength, heightWall, 1);

        float enterLeftWallPozX = -(sideOfSquare / 2 + (widthEnter / 2));
        float enterLeftWallPozZ = zCoordinateBeginningOfBlock + sideOfSquare / 2;
        enterWall.transform.position = new Vector3(enterLeftWallPozX, heightWall / 2, enterLeftWallPozZ);
        enterWall.transform.localEulerAngles = new Vector3(0, -45, 0);
        Instantiate(enterWall);

        float enterRigthWallPozX = sideOfSquare / 2 + (widthEnter / 2);
        float enterRigthWallPozZ = enterLeftWallPozZ;
        enterWall.transform.position = new Vector3(enterRigthWallPozX, heightWall / 2, enterRigthWallPozZ);
        enterWall.transform.localEulerAngles = new Vector3(0, 225, 0);
        Instantiate(enterWall);

        float exitLeftWallPozX = -(sideOfSquare / 2 + (widthEnter / 2));
        float exitLeftWallPozZ = zCoordinateBeginningOfBlock + sideOfSquare + lengthBeetwenWalls + sideOfSquare / 2;
        enterWall.transform.position = new Vector3(exitLeftWallPozX, heightWall / 2, exitLeftWallPozZ);
        enterWall.transform.localEulerAngles = new Vector3(0, 45, 0);
        Instantiate(enterWall);

        float exitRigthWallPozX = sideOfSquare / 2 + (widthEnter / 2);
        float exitRigthWallPozZ = exitLeftWallPozZ;
        enterWall.transform.position = new Vector3(exitRigthWallPozX, heightWall / 2, exitRigthWallPozZ);
        enterWall.transform.localEulerAngles = new Vector3(0, -225, 0);
        Instantiate(enterWall);
        if (LevelManager.PutClimbBonus)
            PutClimbBonus();
        PutCrystalBonuses();
    }
    protected override void PutClimbBonus()
    {
        climbBonus = Instantiate(climbBonusPref);
        float distanceFromWall = 3;
        Side bonusSide = (Side)Random.Range(0, 2);
        float xBonusPosition = Random.Range(-widthWall / 2 + distanceFromWall, -widthEnter / 2);
        float zBonusPosition = Random.Range(zCoordinateBeginningOfBlock + sideOfSquare + minZBonus, zCoordinateBeginningOfBlock + sideOfSquare + lengthBeetwenWalls - maxZBonus);
        if (bonusSide == Side.Right)
            xBonusPosition = -xBonusPosition;
        climbBonus.transform.position = new Vector3(xBonusPosition, climbBonus.transform.position.y, zBonusPosition);
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
        float distanceFromWall = 3;
        Side bonusSide = (Side)Random.Range(0, 2);
        float xBonusPosition = Random.Range(-widthWall / 2 + distanceFromWall, -widthEnter / 2);
        float zBonusPosition = Random.Range(zCoordinateBeginningOfBlock + sideOfSquare + 10, zCoordinateBeginningOfBlock + sideOfSquare + lengthBeetwenWalls - 10);
        if (bonusSide == Side.Right)
            xBonusPosition = -xBonusPosition;
        position = new Vector3(xBonusPosition, 0, zBonusPosition);
        return position;
    }
}
