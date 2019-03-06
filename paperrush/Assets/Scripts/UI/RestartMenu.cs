using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;
using UnityEngine.UI;
using DG.Tweening;

public class RestartMenu : GUIScreen
{
    public Text currentEndPoints;
    public Text recordPoints;
    public Sprite[] blueSchema;
    public Sprite[] greenSchema;
    public Sprite[] purpleSchema;
    private ColorSchemasManager colorManager;
    private ColorSchema curentSchema;
    public List<Image> colorButtonsImages;
    public List<RectTransform> uiRects = new List<RectTransform>();
    public float animDuration = 0.5f;
    void Awake()
    {
        Messenger.AddListener(GameEvent.PlaneIsBroken, SetPoints);
        colorManager = GameObject.Find("ColorSchemasManager").GetComponent<ColorSchemasManager>();
        curentSchema = colorManager.curentSchema;
        ChangeColorIcons();
    }
    public override void ShowScreen()
    {
        generalGameObject.SetActive(true);
        IsVisible = true;
        
        foreach (var rectUI in uiRects)
        {
            Vector2 scale = rectUI.sizeDelta;
            rectUI.sizeDelta = new Vector2(0,0);
            rectUI.DOSizeDelta(scale,animDuration);
        }
        ChangeColorIcons();
    }
    void Update()
    {
        bool colorChemaIsChanged = curentSchema != colorManager.curentSchema;
        if (colorChemaIsChanged)
        {
            curentSchema = colorManager.curentSchema;
            ChangeColorIcons();
        }
        currentEndPoints.text = Managers.Records.CurrentGamePoints.ToString();
        recordPoints.text = Managers.Records.RecordPoints.ToString();
    }
    void SetPoints()
    {
        currentEndPoints.text = Managers.Records.CurrentGamePoints.ToString();
        recordPoints.text = Managers.Records.RecordPoints.ToString();
    }
    private void ChangeColorIcons()
    {
        switch (curentSchema)
        {
            case ColorSchema.Blue:
                for (int i = 0; i < colorButtonsImages.Count; i++)
                    colorButtonsImages[i].sprite = blueSchema[i];
                break;
            case ColorSchema.Green:
                for (int i = 0; i < colorButtonsImages.Count; i++)
                    colorButtonsImages[i].sprite = greenSchema[i];
                break;
            case ColorSchema.Purple:
                for (int i = 0; i < colorButtonsImages.Count; i++)
                    colorButtonsImages[i].sprite = purpleSchema[i];
                break;
            default:
                break;
        }
    }
}
