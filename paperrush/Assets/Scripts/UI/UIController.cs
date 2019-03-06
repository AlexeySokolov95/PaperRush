using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Class;
using Assets.Scripts.UI;

[RequireComponent(typeof(RestartMenu))]
[RequireComponent(typeof(InGameGUI))]
[RequireComponent(typeof(MainMenu))]
[RequireComponent(typeof(CustomizePlanerScript))]
public class UIController : MonoBehaviour, IGameManager
{
    public MainMenu MainMenuScreen { get; private set; }
    public RestartMenu RestartGUI { get; private set; }
    public InGameGUI GameGUI { get; private set; }
    public CorrectionMenu CorrectionGUI { get; private set; }
    public CustomizePlaner CustomizePlaner { get; private set; }
    private List<IGUIScreen> Screens;
    void Awake()
    {
        RestartGUI = GetComponent<RestartMenu>();
        GameGUI = GetComponent<InGameGUI>();
        MainMenuScreen = GetComponent<MainMenu>();
        CustomizePlaner = GetComponent<CustomizePlaner>();
    }
    void Start()
    {
        Messenger.AddListener(GameEvent.PlaneIsBroken, ShowRestartUI);
        
        //Time.timeScale = 0.01f;
        Screens = new List<IGUIScreen>() { RestartGUI, GameGUI, MainMenuScreen, CustomizePlaner };
        ShowLayer(MainMenuScreen);
    }

    void Update()
    {

    }
    public void Restart()
    {

    }
    public void Startup()
    {

    }
    public void ShowLayer(IGUIScreen layer)
    {
        foreach(var lay in Screens)
        {
            if (lay == layer)
                lay.ShowScreen();
            else
            { 
                lay.HideScreen();
            }
        }
    }
    private void ShowRestartUI()
    {
        ShowLayer(RestartGUI);
    }
}
