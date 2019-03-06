using UnityEngine;
using Assets.Class;

public class ComplexDoorScript : LevelBlock
{
    public float rotationSpeed = 0.30f;
    public GameObject complexDoor;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    public float blockLength = 50;
    Side rotationSide;
    void Start()
    {
        Initialization(blockLength);
        PutWall();
        float positionZ = zCoordinateBeginningOfBlock + (blockLength / 2);
        complexDoor.transform.position = new Vector3(0, heightWall / 2, positionZ);
        rotationSide = (Side)Random.Range(0, 2);
        complexDoor = Instantiate(complexDoor);
    }

    void Update()
    {
        if (rotationSide == Side.Right)
            complexDoor.transform.Rotate(new Vector3(0, -rotationSpeed, 0));
        else
            complexDoor.transform.Rotate(new Vector3(0, rotationSpeed, 0));
    }
}
