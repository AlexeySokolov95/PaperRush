using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class DancingSingleSpikeScript : BlockElement
{
    Direction MovingDirection = Direction.Left;
    float n_speed;
    float n_lengthOfAreaForDancing;
    float n_zPositionOfEnd;
    float zPosition;
    // Use this for initialization
    void Start()
    {
        Initialization();
        zPosition = transform.position.z;
        transform.localScale = new Vector3(transform.localScale.x, heightWall, transform.localScale.z);
        DancingSpikesScript motheScript = GameObject.Find("DancingSpikesBlock(Clone)").GetComponent<DancingSpikesScript>();
        n_speed = motheScript.speed;
        n_lengthOfAreaForDancing = motheScript.lengthOfAreaForDancing;
        float spikeNewPositionX = Random.Range(-widthWall / 2, widthWall / 2);
        transform.position = new Vector3(spikeNewPositionX, heightWall / 2, transform.position.z);
        n_zPositionOfEnd = Random.Range(0, n_lengthOfAreaForDancing);
        float numberForSelectDirection = Random.value;
        if (numberForSelectDirection < 0.5)
            MovingDirection = Direction.Left;
        else
            MovingDirection = Direction.Right;
    }

    // Update is called once per frame
    void Update()
    {
        switch (MovingDirection)
        {
            case Direction.Right:
                if (transform.position.x < (widthWall / 2 - (transform.localScale.x / 2)))
                    transform.Translate(DeltaPosition());
                else
                {
                    transform.position = new Vector3(widthWall / 2 - (transform.localScale.x / 2), transform.position.y, zPosition + n_zPositionOfEnd);
                    MovingDirection = Direction.Left;
                }
                break;
            case Direction.Left:
                if (transform.position.x > (-widthWall / 2 + (transform.localScale.x / 2)))
                    transform.Translate(DeltaPosition());
                else
                {
                    transform.position = new Vector3(-widthWall / 2 + (transform.localScale.x / 2), transform.position.y, zPosition + n_zPositionOfEnd);
                    MovingDirection = Direction.Right;
                }
                break;
        }
    }
    Vector3 DeltaPosition()
    {
        Vector3 endPosition;
        Vector3 deltaPosition;
        float multiplierZ;
        if (MovingDirection == Direction.Right)
        {
            endPosition = new Vector3(widthWall / 2 - (transform.localScale.x / 2), transform.position.y, zPosition + n_zPositionOfEnd);
            multiplierZ = (endPosition.z - transform.position.z) / (endPosition.x - transform.position.x);
            deltaPosition = new Vector3(n_speed * Time.deltaTime, 0, n_speed * Time.deltaTime * multiplierZ);
        }
        else
        {
            endPosition = new Vector3(-widthWall / 2 + (transform.localScale.x / 2), transform.position.y, zPosition + n_zPositionOfEnd);
             multiplierZ = (endPosition.z - transform.position.z) / (endPosition.x - transform.position.x);
            deltaPosition = new Vector3(-n_speed * Time.deltaTime, 0, n_speed * Time.deltaTime * multiplierZ);
        }
        return deltaPosition;
    }
}
