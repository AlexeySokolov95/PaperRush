using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class SpikeScript : BlockElement
{

    // Use this for initialization
    void Start()
    {
        Initialization();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 25f;
        if (transform.position.z - LevelManager.player.transform.position.z < 60)
        {
             if(transform.position.y < heightWall / 2 - 0.5)
                transform.Translate(new Vector3(0, speed * Time.deltaTime,0 ));
        }
    }
}
