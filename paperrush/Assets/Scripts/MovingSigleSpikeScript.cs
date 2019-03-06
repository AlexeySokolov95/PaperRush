using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class MovingSigleSpikeScript : BlockElement
{

    Direction MovingDirection = Direction.Left;
    public float speed = 30;
    // Use this for initialization
    void Start()
    {
        Initialization();
        transform.localScale = new Vector3(transform.localScale.x, heightWall, transform.localScale.z);
        float spikeNewPositionX = Random.Range(-widthWall/2, widthWall / 2);
        transform.position = new Vector3(spikeNewPositionX, heightWall/2, transform.position.z);
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
                if (transform.position.x < (widthWall/2 - (transform.localScale.x/2)))
                    transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
                else
                {
                    transform.position = new Vector3(widthWall / 2 -(transform.localScale.x / 2), transform.position.y, transform.position.z);
                    MovingDirection = Direction.Left;
                }
                break;
            case Direction.Left:
                if (transform.position.x > (-widthWall / 2 + (transform.localScale.x / 2)))
                    transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
                else
                {
                    transform.position = new Vector3(-widthWall / 2 + (transform.localScale.x / 2), transform.position.y, transform.position.z);
                    MovingDirection = Direction.Right;
                }
                break;
        }
    }

}
enum Direction { Left, Right }
