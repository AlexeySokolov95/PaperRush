using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class DancingSpikesScript : LevelBlock
{
    public float lengthOfAreaForDancing = 25;
    public int numberOfArea = 4;
    public float speed = 30;
    public float numberSpikesOnArea = 2f;
    GameObject n_spike;

    void Start()
    {
        Initialization(numberOfArea * lengthOfAreaForDancing);
        PutWall();
        n_spike = Resources.Load("pref_DancingSingleSpike", typeof(GameObject)) as GameObject;
        float positionZNewSpike = 0;
        while (positionZNewSpike < lengthOfMainWall)
        {
            for (int i = 0; i < numberSpikesOnArea; i++)
            {
                GameObject newSpike = Instantiate(n_spike);
                newSpike.transform.position = new Vector3(0, 0, zCoordinateBeginningOfBlock + positionZNewSpike);
                elements.Add(newSpike);
            }
            positionZNewSpike += lengthOfAreaForDancing;
        }
    }


    void Update()
    {

    }
}
