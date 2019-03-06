using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class BonusManager : MonoBehaviour, IGameManager
{
    [SerializeField]
    private int numberCrystalBonus = 0;

    public int NumberCrystalBonus
    {
        get { return numberCrystalBonus; }
        private set { numberCrystalBonus = value; }
    }
    void Awake()
    {
        //Read number of crystal bonuses from disc
        if (PlayerPrefs.HasKey(SaveKeys.NumberOfCrystalBonuses))
            numberCrystalBonus = PlayerPrefs.GetInt(SaveKeys.NumberOfCrystalBonuses);
        else
        {
            PlayerPrefs.SetInt(SaveKeys.NumberOfCrystalBonuses, 120);
            numberCrystalBonus = 120;
        }
    }
    void Start()
    {
        Messenger.AddListener(GameEvent.CrystalBonusCatched, CrystalBonusCatched);
        Messenger.AddListener(GameEvent.PlaneIsBroken, SaveCrystalBonuses);

    }

    void Update()
    {

    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.CrystalBonusCatched, CrystalBonusCatched);
        Messenger.RemoveListener(GameEvent.PlaneIsBroken, SaveCrystalBonuses);
    }
    void CrystalBonusCatched()
    {
        numberCrystalBonus ++;
    }
    public void SaveCrystalBonuses()
    {
        PlayerPrefs.SetInt(SaveKeys.NumberOfCrystalBonuses, numberCrystalBonus);
    }
    public void AddCrystals(int number)
    {
        numberCrystalBonus += number;
        SaveCrystalBonuses();
    }
    public void SubstractCrystals(int number)
    {
        numberCrystalBonus -= number;
        if (numberCrystalBonus < 0)
            numberCrystalBonus = 0;
        SaveCrystalBonuses();
    }
    public void Restart()
    {

    }
    public void Startup()
    {

    }
}
