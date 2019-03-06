using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyDownScript : MonoBehaviour
{
    public GameObject downBlock;
    private float widthWall;
    private float heightWall;
    private float currentPosition = 0;
    private LevelCreater LevelManager;
    
    void Awake()
    {
        LevelManager = GameObject.Find("LevelManager").GetComponent<LevelCreater>();
        widthWall = LevelManager.WidthWall;
        heightWall = LevelManager.HeightWall;
    }
    void Start()
    {
    }

    void Update()
    {
        if (LevelManager.player.transform.position.z + 300 > currentPosition)
        {
            GameObject newDownBlock = downBlock;
            newDownBlock.transform.localScale = new Vector3(widthWall / 100, heightWall / 100, 1);
            newDownBlock.transform.position = new Vector3(0, 0, currentPosition);
            Instantiate(newDownBlock);
            currentPosition = currentPosition + 100;
        }
        TruToDelArtifacts();
    }
    void TruToDelArtifacts()
    {
        GameObject[] allDownBlock = GameObject.FindGameObjectsWithTag("Down");
        foreach(var block in allDownBlock)
        {
            if (block.transform.localScale.z > 1)
                Destroy(block);
        }
    }
}
