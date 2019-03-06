using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class StartBlockScript : LevelBlock
{
    public GameObject onlyDown;
    void Start()
    {
        Initialization(110f);
        PutWall();
        /*
        float zPosition = LevelManager.GetComponent<LevelCreater>().Length;
        walls.Add(Instantiate(Resources.Load("pref_SimpleWalls100x100", typeof(GameObject))) as GameObject);

        walls[0].transform.position = new Vector3(0, 0, 0);
        walls.Add(Instantiate(Resources.Load("pref_SimpleWalls100x100", typeof(GameObject))) as GameObject);
        walls[0].transform.position = new Vector3(0, 0, 100);
        length = 150;
        LevelManager.GetComponent<LevelCreater>().AddLength(length);*/
       /* GameObject wall = Instantiate(onlyDown);
        wall.transform.localScale = new Vector3(widthWall / 100, heightWall / 100, lengthOfMainWall / 100);*/
    }
    protected override void PutWall()
    {
        /*walls.Add(Instantiate(Resources.Load("pref_SimpleWalls100x100", typeof(GameObject))) as GameObject);
        walls[0].transform.localScale = new Vector3(widthWall / 100, heightWall / 100, lengthOfMainWall / 100);
        walls[0].transform.position = new Vector3(0, 0, 0);*/
    }

    // Update is called once per frame
    void Update()
    {

    }
}
