using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Class;

public class ForestOfSpikeScript : LevelBlock
{
    public int numberSpikeOnZRow = 5;
    public int numberSpikeOnXRow = 3;
    public float newLength = 100;
    void Start()
    {
        Initialization(newLength);
        PutWall();
        PutSpike();
    }
    private void PutSpike()
    {
        float deltaI = lengthOfMainWall / numberSpikeOnZRow;
        float deltaJ = widthWall / numberSpikeOnXRow;
        for (float i = 0f; i < lengthOfMainWall; i += deltaI)
        {
            for (float j = 2f; j < widthWall; j += deltaJ)
            {
                if (j > widthWall)
                    break;
                GameObject newSpike = Instantiate(Resources.Load("pref_Spike", typeof(GameObject))) as GameObject;
                newSpike.transform.position = PlaceANewSpike(i,j);
                elements.Add(newSpike);
            }
        }
    }
    private Vector3 PlaceANewSpike(float z, float x)
    {
        Vector3 spikePlace;
        float spikeZ;
        float spikeX;
        do
        {
            spikeZ = z + zCoordinateBeginningOfBlock + Random.Range(0, 10);
            spikeX = x + Random.Range(0, 10) - (widthWall / 2);
            if (x > 24)
                x = 24;
            spikePlace = new Vector3(spikeX, -heightWall/2, spikeZ);
            
        }
        while ((elements.Any(u => Mathf.Abs(u.transform.position.x - spikeX) < 2 && Mathf.Abs(u.transform.position.z - spikeZ) < 2)));
        return spikePlace;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
