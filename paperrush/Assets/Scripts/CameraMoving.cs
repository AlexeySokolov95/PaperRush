using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    //public ParticleSystem mainParticle;
    public float cameraZ = -7.5f;
    public float cameraY = 5.6f;
    public float playerXSpeed = 1.7f;
    public float sphereRadius = 4.3f;
    private GameObject player;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {        
        float posX = player.transform.position.x / playerXSpeed;
        float posY = player.transform.position.y + cameraY;
        float posZ = player.transform.position.z + cameraZ;
        transform.position = new Vector3(posX, posY, posZ);
        var hitColliders = Physics.OverlapSphere(transform.position, sphereRadius);
        foreach(var gameObj in hitColliders)
        {
            if(gameObj.gameObject.tag == "LevelObstacle")
            {
               gameObj.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        /*if (!mainParticle.isPlaying && player.GetComponent<RBPlayerMoving>().isMoving)
            mainParticle.Play();
        //Move player PS
        mainParticle.transform.position = player.transform.position + new Vector3(0,0,15);*/
    }

}
