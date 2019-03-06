using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizePlaner : GUIScreen
{
    public ScrollRect sv_materials;
    public ScrollRect sv_models;
    public Text numberOfCrystals;
    public Image selecter;
    public Image img_Model;
    public Image img_Material;
    public Sprite spr_btnModel;
    public Sprite spr_disableBtnModel;
    public Sprite spr_btnMaterial;
    public Sprite spr_disableBtnMaterial;
    public Text purchaseMats;
    public Text purchaseModels;
    CustomizePlanerManagerScript customizeManager;
    // Use this for initialization
    void Start()
    {
        customizeManager = GameObject.Find("CustomizePlanerManager").GetComponent<CustomizePlanerManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(numberOfCrystals.text != Managers.Bonuses.NumberCrystalBonus.ToString())
            numberOfCrystals.text =  Managers.Bonuses.NumberCrystalBonus.ToString();
    }
    public void Back()
    {
        GUIController = GameObject.Find("GUIController").GetComponent<UIController>();
        GUIController.ShowLayer(GUIController.MainMenuScreen);
    }
    public void ShowModels()
    {
        img_Material.sprite = spr_disableBtnMaterial;
        img_Model.sprite = spr_btnModel;
        sv_materials.gameObject.SetActive(false);
        sv_models.gameObject.SetActive(true);
        ShowNumberPurchasedModels();
    }
    public void ShowMaterials()
    {
        img_Material.sprite = spr_btnMaterial;
        img_Model.sprite = spr_disableBtnModel;
        sv_materials.gameObject.SetActive(true);
        sv_models.gameObject.SetActive(false);
        //Show material selecter
        string selectedMaterial = customizeManager.currentMaterial;
        var planerColorSkinScripts = FindObjectsOfType<PlanerColorSkinScript>();
        foreach (var script in planerColorSkinScripts)
        {
            if (script.materialName == selectedMaterial)
                ShowSelectedMaterial(script.GetComponent<RectTransform>());
        }
        ShowNumberPurchasedMaterials();  
    }
    public void ShowNumberPurchasedMaterials()
    {
        purchaseMats.gameObject.SetActive(true);
        purchaseModels.gameObject.SetActive(false);
        purchaseMats.text = customizeManager.NumberPurchasedMaterials.ToString() + " / " + customizeManager.purchasedMaterials.Count.ToString();
    }
    public void ShowNumberPurchasedModels()
    {
        purchaseModels.gameObject.SetActive(true);
        purchaseMats.gameObject.SetActive(false);
        purchaseModels.text = customizeManager.NumberPurchasedModels.ToString() + " / " + customizeManager.purchasedModels.Count.ToString();
        
    }
    public void ShowSelectedMaterial(RectTransform btn)
    {   
        selecter.transform.SetParent(btn);
        selecter.GetComponent<RectTransform>().anchoredPosition = new Vector2(96, -96);
    }
    public override void ShowScreen()
    {
        generalGameObject.SetActive(true);
        IsVisible = true;
        ShowMaterials();
        
    }
}
