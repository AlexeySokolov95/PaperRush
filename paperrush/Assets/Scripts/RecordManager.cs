using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class RecordManager : MonoBehaviour, IGameManager
{
    private float currentPoints = 0;
    private int recordPoints = 0;
    private int additivePointsForCatchingCrystal = 5;
    private int distanceDenimonator = 5;
    private int allPointsFromCrystal = 0;
    bool planeIsBroken = false;

    private GameObject player;
    public float CurrentGamePoints
    {
        get { return (int)currentPoints; }
        private set { currentPoints = value; }
    }
    public int RecordPoints
    {
        get { return (int)recordPoints; }
        private set { recordPoints = value; }
    }
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        //Read records from disc
        if (PlayerPrefs.HasKey(SaveKeys.RecordPoints))
            recordPoints = PlayerPrefs.GetInt(SaveKeys.RecordPoints);
        else
        {
            PlayerPrefs.SetInt(SaveKeys.RecordPoints, 0);
            recordPoints = 0;
        }
    }

    void Start()
    {
        Messenger.AddListener<int>(GameEvent.TryAddNewRecord, CheckingForNewRecord);
        Messenger.AddListener(GameEvent.CrystalBonusCatched, AddPointsForCatchingCrystal);
        Messenger.AddListener(GameEvent.PlaneIsBroken, DontChangeCurrentPoints);
    }

    void Update()
    {
        if (!planeIsBroken && player.gameObject != null && player.gameObject.activeSelf)
        {
            if (Input.touches.Length > 0 && Input.touches[0].deltaPosition.x > currentPoints)
            {
                currentPoints = Input.touches[0].deltaPosition.x;
                
            }
            currentPoints = player.transform.position.z / distanceDenimonator;
        }
            //currentPoints =  (int)player.transform.position.z / distanceDenimonator + allPointsFromCrystal;
    }
    public void CheckingForNewRecord(int distanceCrushedPlayer)
    {
        currentPoints = (float)distanceCrushedPlayer / (float)distanceDenimonator;
        if (currentPoints > recordPoints)
        {
            recordPoints = (int)currentPoints;
            PlayerPrefs.SetInt(SaveKeys.RecordPoints, recordPoints);
        }
        allPointsFromCrystal = 0;
    }
    void AddPointsForCatchingCrystal()
    {

        allPointsFromCrystal += additivePointsForCatchingCrystal;
    }
    void DontChangeCurrentPoints()
    {
        planeIsBroken = true;
    }
    public void Restart()
    {
        planeIsBroken = false;
    }
    public void Startup()
    {

    }
}
