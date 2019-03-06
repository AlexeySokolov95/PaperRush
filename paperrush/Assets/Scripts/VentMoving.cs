using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentMoving : MonoBehaviour {
    public float rotationSpeed = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, -5, 0));
	}
}
