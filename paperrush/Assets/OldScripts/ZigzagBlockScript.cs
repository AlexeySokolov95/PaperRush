using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigzagBlockScript : MonoBehaviour
{
    public List<GameObject> walls = new List<GameObject>();
    public List<GameObject> elements = new List<GameObject>();
    private GameObject LevelManager;
    private float length = 0;
    void Start()
    {
        LevelManager = GameObject.Find("LevelManager");
        float zPosition = LevelManager.GetComponent<LevelCreater>().LevelLength;
        float distanceBetweenWalls = 20f;
        walls.Add(Instantiate(Resources.Load("pref_SimpleWalls 1 1", typeof(GameObject))) as GameObject);
        length = 100f;
        walls[0].transform.position = new Vector3(0, 0, zPosition);
        float positionZNewWall = 0f;
        float halfWidthWall = 15f;
        float halfHeightWall = 15f;
        Side side = Side.Right;
        // Get a random side
        float random = Random.value;
        if (random > 0.5f)
            side = Side.Left;
        // Put walls
        while (positionZNewWall < 100)
        {
            GameObject zigZagWall = Instantiate(Resources.Load("prefZigZagWall 1", typeof(GameObject))) as GameObject;
            if (side == Side.Left)
            {
                zigZagWall.transform.position = new Vector3(0 - halfWidthWall, halfHeightWall, zPosition + positionZNewWall - (length / 2));
                side = Side.Right;
            }
            else if (side == Side.Right)
            {
                zigZagWall.transform.position = new Vector3(0 + halfWidthWall, halfHeightWall, zPosition + positionZNewWall - (length / 2));
                side = Side.Left;
            }
            elements.Add(zigZagWall);
            positionZNewWall = positionZNewWall + distanceBetweenWalls;
        }
        LevelManager.GetComponent<LevelCreater>().AddLength(length);
    }
    enum Side { Left, Right }
    // Update is called once per frame
    void Update()
    {
    }
}
