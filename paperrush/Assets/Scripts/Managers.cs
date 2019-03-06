using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Class;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BonusManager))]
[RequireComponent(typeof(RecordManager))]
public class Managers : MonoBehaviour
{
    static public BonusManager Bonuses { get; private set; }
    static public RecordManager Records { get; private set; }
    static private List<IGameManager> gameManagers;

    RBPlayerMoving player;
    PlayerRotation playerRotation;
    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        Bonuses = GetComponent<BonusManager>();
        Records = GetComponent<RecordManager>();
        gameManagers = new List<IGameManager>();
        gameManagers.Add(Bonuses);
        gameManagers.Add(Records);
    }
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<RBPlayerMoving>();
        playerRotation = GameObject.FindWithTag("Player").GetComponent<PlayerRotation>();
    }

    void Update()
    {

    }
    public static void Restart()
    {
        foreach (var manager in gameManagers)
        {
            manager.Restart();
        }
        SceneManager.LoadScene("SampleScene");
    }
    public void NonStaticRestart()
    {
        Restart();
    }
    public void Correct()
    {
        InputField input1 = GameObject.Find("InputField").GetComponent<InputField>() ;
        InputField input2 = GameObject.Find("InputField (1)").GetComponent<InputField>();
        InputField input3 = GameObject.Find("InputField (2)").GetComponent<InputField>();
        playerRotation.rotationZSpeed = System.Convert.ToSingle(input1.text);
        playerRotation.stabilizeRotZSpeed = System.Convert.ToSingle(input2.text);
        player.DeltaSpeed = System.Convert.ToSingle(input3.text);
        Time.timeScale = 1f;
        GameObject.Find("GUIController").GetComponent<UIController>().CorrectionGUI.HideScreen();
    }
}
