using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;
using UnityEngine.UI;
using DG.Tweening;

public class InGameGUI : GUIScreen
{
    [SerializeField]
    private Text numberOfCrystals;
    [SerializeField]
    private Text numberOfPoints;
    public RectTransform climbBonusHint;
    int oldNumberOfCrystalBonus = 0;
    float oldGamePoints = 0;
    public ClimbBonusHintScript climbHintScript;
    public float animHintDuration = 0.3f;
    public float pauseAnimHintDuration = 3f;
    public int maxOfNumberOfShowingHints = 10;
    void Awake()
    {
        
    }
    void Start()
    {
        GUIController = GameObject.Find("GUIController").GetComponent<UIController>();
        numberOfCrystals.text = Managers.Bonuses.NumberCrystalBonus.ToString();

    }

    void Update()
    {
        if(gameObject != null)
        {
            if (oldNumberOfCrystalBonus != Managers.Bonuses.NumberCrystalBonus)
            {
                numberOfCrystals.text = Managers.Bonuses.NumberCrystalBonus.ToString();
                oldNumberOfCrystalBonus = Managers.Bonuses.NumberCrystalBonus;
            }
            if (oldGamePoints != Managers.Records.CurrentGamePoints)
            {
                numberOfPoints.text = Managers.Records.CurrentGamePoints.ToString();
                oldGamePoints = Managers.Records.CurrentGamePoints;
            }
        }
    }
    public override void ShowScreen()
    {
        generalGameObject.SetActive(true);
        IsVisible = true; 
        //Show hint
        int numberOfShowingHints = 0;
        if (PlayerPrefs.HasKey(SaveKeys.NumberOfShowingClimbBonusHint))
            numberOfShowingHints = PlayerPrefs.GetInt(SaveKeys.NumberOfShowingClimbBonusHint);
        else
        {
            PlayerPrefs.SetInt(SaveKeys.NumberOfShowingClimbBonusHint, 0);
            numberOfShowingHints = 0;
        }
        if(numberOfShowingHints <= maxOfNumberOfShowingHints)
        {
            climbBonusHint.sizeDelta = new Vector2(0, 0);
            Sequence hintSequence = DOTween.Sequence();
            hintSequence.Append(climbBonusHint.DOSizeDelta(new Vector2(510, 100), animHintDuration));
            hintSequence.Append(climbBonusHint.DOSizeDelta(new Vector2(510, 101), pauseAnimHintDuration));
            hintSequence.Append(climbBonusHint.DOSizeDelta(new Vector2(0, 0), animHintDuration));
            PlayerPrefs.SetInt(SaveKeys.NumberOfShowingClimbBonusHint, numberOfShowingHints + 1);
        }
        else
            climbBonusHint.sizeDelta = new Vector2(0, 0);
    }
}
