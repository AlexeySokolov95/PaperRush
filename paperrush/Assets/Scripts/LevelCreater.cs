using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;
using Assets.Scripts;

public class LevelCreater : MonoBehaviour
{
    [SerializeField]
    List<LevelBlock> Level = new List<LevelBlock>();

    public int numberOfCylinder = 5;
    public float cylinderMaxRandomWidthPart = 1;
    private CylinderWall cylinderWall;

    public GameObject player;
    private float levelLength = 0;
    private float _playerPositionZ;
    public float widthWall = 50;
    public float heightWall = 30;
    private bool putRandomBlocks = true;
    public bool putClimbBonusForSmallBlock = true;
    private NextBlock nextBlock = NextBlock.Any;
    private float lengthOfEasyPeriod = 200;

    private GameObject waveWalls;
    private GameObject downManager;

    public bool onlyOneBlock = false;
    public GameObject lonelyBlock;

    public List<GameObject> anyLevelBlocks = new List<GameObject>();
    public List<GameObject> smallLevelBlocks = new List<GameObject>();
    public List<GameObject> easyAnyLevelBlocks = new List<GameObject>();
    public List<GameObject> easySmallLevelBlocks = new List<GameObject>();
    private LevelBlockChooser levelBlockChooser;
    public RBPlayerMoving rbpPlayer;
    public void AddLength(float inc)
    {
        levelLength = levelLength + inc;
    }
    public float LevelLength
    {
        get { return levelLength; }
    }
    public float WidthWall
    {
        get { return widthWall; }
    }
    public float HeightWall
    {
        get { return heightWall; }
    }
    public float LevelXPosition
    {
        get { return levelLength; }
    }
    public bool PutClimbBonus
    {
        set { putClimbBonusForSmallBlock = value; }
        get { return putClimbBonusForSmallBlock; }
    }
    /*private float LevelLength
    {
        get
        {
            float lentgh = 0f;
            foreach (var block in Level)
                lentgh += block.length;
            return lentgh;
        }
    }*/
    delegate void BlockCreater(LevelBlock block);

    void Start()
    {
        // Application.LoadLevel("Scene1");
        Instantiate(Resources.Load("StartBlock", typeof(GameObject)) as GameObject);
        AddLength(55);
        if (!putRandomBlocks)
            LoadAllBlocks();
        waveWalls = Instantiate(Resources.Load("WaveWalls", typeof(GameObject)) as GameObject);
        downManager = Instantiate(Resources.Load("OnlyDownManager", typeof(GameObject)) as GameObject);
        levelBlockChooser = new LevelBlockChooser(anyLevelBlocks.ToArray(), smallLevelBlocks.ToArray(),easyAnyLevelBlocks.ToArray(), easySmallLevelBlocks.ToArray(), lengthOfEasyPeriod);
        rbpPlayer = player.GetComponent<RBPlayerMoving>();
        Application.targetFrameRate = 60;
        /*GameObject startBlock1 = Instantiate(wallPrefab) as GameObject;
        startBlock1.transform.position = new Vector3(levelXPosition, 0, 0);
        Level.Add(startBlock1);
        GameObject startBlock2 = Instantiate(wallPrefab) as GameObject;
        startBlock1.transform.position = new Vector3(levelXPosition, 0, 100);
        Level.Add(startBlock2); */
    }


    // Update is called once per frame
    void Update()
    {
        if (putRandomBlocks)
        {
            _playerPositionZ = player.transform.position.z;
            float endOfLevel = levelLength;
            if (endOfLevel - _playerPositionZ < 200)
            {
                CreateNewBlock(endOfLevel);
            }
        }
        /*if (_playerPositionZ + 250 > urbanEnvironment.CurrentWallLength)
            urbanEnvironment.Put();*/
    }
    private void CreateNewBlock(float endOfLevel)
    {
        GameObject newBlock = SelectNewBlock3(endOfLevel);
        Instantiate(newBlock);
    }
    private GameObject SelectNewBlock3(float startPosition)
    {
        GameObject newBlock;
        if (onlyOneBlock)
            newBlock = lonelyBlock;
        else
            newBlock = levelBlockChooser.Next(ref nextBlock, player.transform.position.z);
        return newBlock;
    }
    private void LoadAllBlocks()
    {
        List<GameObject> allBlocks = AllBlocks();
        for (int i = 0; i < allBlocks.Count; i++)
        {
            Instantiate(allBlocks[i]);
            for (int j = 0; j < allBlocks.Count; j++)
                Instantiate(allBlocks[j]);
        }
    }
    private List<GameObject> AllBlocks()
    {
        List<GameObject> allBlocks = new List<GameObject>() {
            /*Resources.Load("SpikeWall 1",typeof(GameObject)) as GameObject,*/
            Resources.Load("CrackInWallBlock", typeof(GameObject)) as GameObject,
            Resources.Load("ZigZagBlock2", typeof(GameObject)) as GameObject,
            Resources.Load("ForestOfSpike", typeof(GameObject)) as GameObject,
            Resources.Load("DancingSpikesBlock", typeof(GameObject)) as GameObject,
            Resources.Load("ChessSpikesBlock", typeof(GameObject)) as GameObject,
            Resources.Load("MovingSpikesBlock", typeof(GameObject)) as GameObject };
        return allBlocks;
    }

}
enum NextBlock { Any, SmallBlockWithClimbBonus}

