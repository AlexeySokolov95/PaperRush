using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class MovingSpike : BlockElement
{
    GameObject player;
    Direction MovingDirection;
    // Use this for initialization
    void Start()
    {
        Initialization();
        player = GameObject.Find("Player");
        transform.localScale = new Vector3(transform.localScale.x, heightWall, transform.localScale.x);
        float spikeNewPositionX = Random.Range(-widthWall/2, widthWall / 2);
        float numberForSelectDirection = Random.value;
        if (numberForSelectDirection < 0.5)
            MovingDirection = Direction.Left;
        else
            MovingDirection = Direction.Right;
    }
    // Update is called once per frame
    void Update()
    {
        switch(MovingDirection)
        {
            case Direction.Right:
               // transform.Translate(new Vector3(speed *Time.deltaTime));
                break;
            case Direction.Left:
                break;
        }
    }
    enum Direction { Left, Right }
}
