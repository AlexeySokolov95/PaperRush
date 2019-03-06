using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class MovingPlanerPS : MonoBehaviour {

    public RBPlayerMoving planer;
    public float deltaZ = 8;
    bool isStart = false;
    ParticleSystem ps;
	void Start ()
    {
        ps = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(planer.isMoving && !isStart)
        {
            ps.Play();
            isStart = true;
        }
        ps.transform.position = planer.gameObject.transform.position + new Vector3(0, 0, deltaZ);
    }
}
