using UnityEngine;
using Assets.Class;
using DG.Tweening;

public class ALotOfRectanglesScript : LevelBlock
{
    public int rows = 5;
    public int layers = 20;
    public int rectangleLength = 3;
    public float crackWidth = 5;
    public float distanceBeetwenRows = 5;
    public float maxDeltaX = 3;
    public float distanceFromWall = 5;
    public GameObject reactangle;
    public GameObject climbBonusPref;
    public GameObject crystalBonus;
    private float blockLength = 0;

    void Start()
    {
        blockLength = (rectangleLength + (distanceBeetwenRows - 1)) * (layers * rows);
        Initialization(blockLength);
        PutWall();
        float rectScaleY = heightWall / rows;
        reactangle.transform.localScale = new Vector3(widthWall, rectScaleY, rectangleLength);
        float crackCoordX = Random.Range(-distanceFromWall, distanceFromWall);
        //Search the random order of rectangles Y position
        int[] yOrder = new int[rows];
        for (int i = 0; i < rows; i++)
            yOrder[i] = i;
        MixOrder(yOrder);
        //Put rectangles
        for (int layerCoord = 0; layerCoord < layers; layerCoord++)
        {
            for (int rowsCoord = 0; rowsCoord < rows; rowsCoord++)
            {
                float yCoordRect = rectScaleY/2 + rectScaleY * yOrder[rowsCoord]; 
                //Search coordinate x of crack centr
                float deltaX = Random.Range(1,maxDeltaX);
                Side deltaSide = (Side)Random.Range(0, 2);
                if(deltaSide == Side.Left)
                {
                    crackCoordX -= deltaX;
                    if (crackCoordX < (-widthWall / 2) + distanceFromWall)
                        crackCoordX = (-widthWall / 2) + distanceFromWall;
                }
                else
                {
                    crackCoordX += deltaX;
                    if (crackCoordX > (widthWall / 2) - distanceFromWall)
                        crackCoordX = (widthWall / 2) - distanceFromWall;
                }
                float rectCoordZ = zCoordinateBeginningOfBlock + (((rectangleLength / 2) + distanceBeetwenRows) * (layerCoord * rows + rowsCoord));
                //Put left rectangles
                float leftRectCoordX = crackCoordX - (widthWall / 2) - (crackWidth / 2);
                reactangle.transform.position = new Vector3(leftRectCoordX, yCoordRect, rectCoordZ);
                Instantiate(reactangle);
                //Put right rectangles
                float rightRectCoordX = crackCoordX + (widthWall / 2) + (crackWidth / 2);
                reactangle.transform.position = new Vector3(rightRectCoordX, yCoordRect, rectCoordZ);
                Instantiate(reactangle);
                //Try to mix order Y
                if (rowsCoord == rows - 1)
                    MixOrder(yOrder);
            }
        }
        PutCrystalBonuses();
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void PutCrystalBonuses()
    {
        int numberOfCrystalBonus = 5;
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
    private void MixOrder(int[] order)
    {
        var r = new System.Random();
        for (int i = order.Length - 1; i > 0; i--)
        {
            int j = r.Next(i);
            var t = order[i];
            order[i] = order[j];
            order[j] = t;
        }
    }
}
