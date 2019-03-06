using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class DiagonalWallAndClosingWallScript : LevelBlock
{
    public int ZigWallNumberOfBlock = 4;
    public int ZigWallBlocksForExit = 1;
    public float wallCrackWidth = 5;
    public float WallClosingSpeed = 1;
    // Use this for initialization
    void Start()
    {
        InteractionWithLevelManager();
        DiagonalWall diagWall = gameObject.AddComponent(typeof(DiagonalWall)) as DiagonalWall;
        diagWall.Initialization(zCoordinateBeginningOfBlock, widthWall, heightWall, ZigWallNumberOfBlock, ZigWallBlocksForExit);
        ClosingWall closWall = gameObject.AddComponent(typeof(ClosingWall)) as ClosingWall;
        closWall.Initialization(zCoordinateBeginningOfBlock + diagWall.Length, widthWall, heightWall, wallCrackWidth, WallClosingSpeed);
        float blockLength = diagWall.Length + closWall.Length;
        LengthOfMainWall = blockLength;
        PutWall();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
