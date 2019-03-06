using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlanerColorSkinScript : MonoBehaviour
{
    public string materialName;
    public Sprite mainImage;
    bool isSelected = false;
    bool isPurchased = false;
    private MeshRenderer playerMeshRender;
    private Sprite closedMaterial;
    CustomizePlanerManagerScript customizeManager;
    CustomizePlaner customizePlanerGUI;
    void Start ()
    {
        //playerMeshRender = GameObject.FindGameObjectWithTag("Player").GetComponent<MeshRenderer>();
        customizeManager = GameObject.Find("CustomizePlanerManager").GetComponent<CustomizePlanerManagerScript>() ;
        customizePlanerGUI = GameObject.Find("GUIController").GetComponent<CustomizePlaner>();
        isPurchased = customizeManager.purchasedMaterials.Find(x => x.Name == materialName).IsPurchased;
        closedMaterial = Resources.Load("LockedMat4", typeof(Sprite)) as Sprite;
        if(isPurchased)
        {
            gameObject.GetComponent<Button>().image.sprite = mainImage;
        }
        else
        {
            gameObject.GetComponent<Button>().image.sprite = closedMaterial;
        }

    }
	
	void Update ()
    {
		
	}
    public void SelectSkin()
    {
        if (isPurchased)
        {
            customizeManager.SelectSkin(materialName);
            customizePlanerGUI.ShowSelectedMaterial(this.GetComponent<RectTransform>());
        }
        else
        {
            bool purchaseIsSuccess = customizeManager.PurchaseSkin(materialName);
            if(purchaseIsSuccess)
            {
                isPurchased = true;
                gameObject.GetComponent<Button>().image.sprite = mainImage;
                customizePlanerGUI.ShowSelectedMaterial(this.GetComponent<RectTransform>());
                customizePlanerGUI.ShowNumberPurchasedMaterials();
            }
            else
            {
                GameObject.Find("txt_customizeNumberOfCrystal").GetComponent<RectTransform>().DOShakeAnchorPos(0.5f, new Vector2(44, 0), randomness: 0);
                GameObject.Find("customize_image_crystal").GetComponent<RectTransform>().DOShakeAnchorPos(0.5f, new Vector2(44, 0), randomness: 0);
            }
        }
    }
}
