using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class MovedSpikesScript : LevelBlock
{
    public float distanceBeetwenRow = 15;
    public int numberOfRow = 4;
    public float speed = 30;
    GameObject n_spike;
    // Use this for initialization
    void Start()
    {
        Initialization(numberOfRow * distanceBeetwenRow);
        PutWall();
        n_spike = Resources.Load("pref_MovingSingleSpike", typeof(GameObject)) as GameObject;
        float positionZNewSpike = 0;
        while (positionZNewSpike < lengthOfMainWall)
        {
            GameObject newSpike = Instantiate(n_spike);
            newSpike.transform.position = new Vector3(0, 0, zCoordinateBeginningOfBlock + positionZNewSpike);
            elements.Add(newSpike);
            positionZNewSpike += distanceBeetwenRow;
        }
    }

    // Update is called once per frame
    void Update()
    { }
}
