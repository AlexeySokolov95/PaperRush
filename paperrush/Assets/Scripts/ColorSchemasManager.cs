using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;
using DG.Tweening;

public class ColorSchemasManager : MonoBehaviour, IGameManager
{
    public float distanceForChangingSchema = 200;
    public Material waveMat1;
    public Material waveMat2;
    public Material obstacleMat;
    public Material angleMat;
    public Material downMat;

    public Material[] blueSchema;
    public Material[] greenSchema;
    public Material[] purpleSchema;

    public GameObject[] levelObstacles;
    public GameObject Down;

    public int maxCatchedClimbsToChangeSchema = 3;
    private int clibmsIsCatched = 0;
    public float changingColorDuration = 0.5f;

    private GameObject player;
    public ColorSchema curentSchema;
    private List<ColorSchema> colorSchemas;
    private Chooser<ColorSchema> schemaChooser;
    private float distance = 0;

    void Start()
    { 
        player = GameObject.FindGameObjectWithTag("Player");
        Messenger.AddListener(GameEvent.ClimbBonusCatched, TryToChangeSchema);
        colorSchemas = new List<ColorSchema>() { ColorSchema.Blue, ColorSchema.Green, ColorSchema.Purple };
        schemaChooser = new Chooser<ColorSchema>(colorSchemas, 0.25, 1);
        ChangeSchema(); 
    }
    void Update()
    {
        if (player != null && player.transform.position.z - distance > distanceForChangingSchema)
        {
            distance = player.transform.position.z;
            //ChangeSchema();
        }
    }
    public void Restart()
    {
        // SetSchema();
    }
    public void Startup()
    {

    }

    public void TryToChangeSchema()
    {
        clibmsIsCatched++;
        if (clibmsIsCatched >= maxCatchedClimbsToChangeSchema)
        {
            clibmsIsCatched = 0;
            ChangeSchema();
        }
    }
    public void ChangeSchema()
    {
        curentSchema = schemaChooser.Next();//(ColorSchema)Random.Range(0, 3);
        //curentSchema = ColorSchema.Purple;
        ChangeCurentMaterials();
        ChangeResources1();
        SetSchema1();
    }
   /* private void SetSchema()
    {
        var obstacles = GameObject.FindGameObjectsWithTag("LevelObstacle");
        foreach (var obstacle in obstacles)
        {
            if (obstacle.transform.position.z + 20 > player.transform.position.z)
                obstacle.GetComponent<MeshRenderer>().material = obstacleMat;
        }
        var downs = GameObject.FindGameObjectsWithTag("DownRoad");
        foreach (var down in downs)
        {
            if (down.transform.position.z + (down.transform.localScale.z / 2) + 60 > player.transform.position.z)
                down.GetComponent<MeshRenderer>().material = downMat;
        }
        var waveBlocks1 = GameObject.FindGameObjectsWithTag("WaveBlock1");
        foreach (var waveBlock in waveBlocks1)
        {
            if (waveBlock.transform.position.z + (waveBlock.transform.localScale.z / 2) + 60 > player.transform.position.z)
                waveBlock.GetComponent<MeshRenderer>().material = waveMat1;
        }
        var waveBlocks2 = GameObject.FindGameObjectsWithTag("WaveBlock2");
        foreach (var waveBlock in waveBlocks2)
        {
            if (waveBlock.transform.position.z + (waveBlock.transform.localScale.z / 2) + 60 > player.transform.position.z)
                waveBlock.GetComponent<MeshRenderer>().material = waveMat2;
        }
        var angles = GameObject.FindGameObjectsWithTag("Angle");
        foreach (var angle in angles)
        {
            if (angle.transform.position.z + (angle.transform.localScale.z / 2) + 60 > player.transform.position.z)
                angle.GetComponent<MeshRenderer>().material = angleMat;
        }
    }*/
    private void SetSchema1()
    {
        if (player != null)
        {
            var obstacles = GameObject.FindGameObjectsWithTag("LevelObstacle");
            foreach (var obstacle in obstacles)
            {
                if (obstacle.transform.position.z + 20 > player.transform.position.z)
                    obstacle.GetComponent<MeshRenderer>().material.DOColor(obstacleMat.color, changingColorDuration);
            }
            var downs = GameObject.FindGameObjectsWithTag("DownRoad");
            foreach (var down in downs)
            {
                if (down.transform.position.z + (down.transform.localScale.z / 2) + 60 > player.transform.position.z)
                    down.GetComponent<MeshRenderer>().material= downMat;
            }
            var waveBlocks1 = GameObject.FindGameObjectsWithTag("WaveBlock1");
            foreach (var waveBlock in waveBlocks1)
            {
                if (waveBlock.transform.position.z + (waveBlock.transform.localScale.z / 2) + 60 > player.transform.position.z)
                    waveBlock.GetComponent<MeshRenderer>().material.DOColor(waveMat1.color, changingColorDuration);
            }
            var waveBlocks2 = GameObject.FindGameObjectsWithTag("WaveBlock2");
            foreach (var waveBlock in waveBlocks2)
            {
                if (waveBlock.transform.position.z + (waveBlock.transform.localScale.z / 2) + 60 > player.transform.position.z)
                    waveBlock.GetComponent<MeshRenderer>().material.DOColor(waveMat2.color, changingColorDuration);
            }
            var angles = GameObject.FindGameObjectsWithTag("Angle");
            foreach (var angle in angles)
            {
                if (angle.transform.position.z + (angle.transform.localScale.z / 2) + 60 > player.transform.position.z)
                    angle.GetComponent<MeshRenderer>().material.DOColor(angleMat.color, changingColorDuration);
            }
        }
    }
  /*  private void ChangeResources()
    {
        foreach (var obstacle in levelObstacles)
            obstacle.GetComponent<MeshRenderer>().material = obstacleMat;
        Down.transform.Find("Road").gameObject.GetComponent<MeshRenderer>().material = downMat;
        Down.transform.Find("pref_semicircle3").gameObject.GetComponent<MeshRenderer>().material = angleMat;
        Down.transform.Find("pref_semicircle3 (1)").gameObject.GetComponent<MeshRenderer>().material = angleMat;
    }
    */
    private void ChangeResources1()
    {
        foreach (var obstacle in levelObstacles)
            obstacle.GetComponent<MeshRenderer>().material = obstacleMat;
        Down.transform.Find("Road").gameObject.GetComponent<MeshRenderer>().material = downMat;
        Down.transform.Find("pref_semicircle3").gameObject.GetComponent<MeshRenderer>().material = angleMat;
        Down.transform.Find("pref_semicircle3 (1)").gameObject.GetComponent<MeshRenderer>().material = angleMat;
    }
    private void ChangeCurentMaterials()
    {
        switch (curentSchema)
        {
            case ColorSchema.Blue:
                waveMat1 = blueSchema[0];
                waveMat2 = blueSchema[1];
                obstacleMat = blueSchema[2];
                angleMat = blueSchema[3];
                downMat = blueSchema[4];
                break;
            case ColorSchema.Green:
                waveMat1 = greenSchema[0];
                waveMat2 = greenSchema[1];
                obstacleMat = greenSchema[2];
                angleMat = greenSchema[3];
                downMat = greenSchema[4];
                break;
            case ColorSchema.Purple:
                waveMat1 = purpleSchema[0];
                waveMat2 = purpleSchema[1];
                obstacleMat = purpleSchema[2];
                angleMat = purpleSchema[3];
                downMat = purpleSchema[4];
                break;
        }
    }       
}
public enum ColorSchema { Blue, Green, Purple }

