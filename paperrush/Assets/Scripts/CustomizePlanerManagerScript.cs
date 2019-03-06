using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Class;

public class CustomizePlanerManagerScript : MonoBehaviour
{
    public List<Material> planerMaterials = new List<Material>();
    public string currentMaterial;
    public string currentModel;
    public Material defaultMaterial;
    [SerializeField]
    public List<PurchasedPlanerMaterial> purchasedMaterials;
    public List<PurchasedPlanerMaterial> purchasedModels = new List<PurchasedPlanerMaterial>();
    public int materialPrice = 1;
    public int NumberPurchasedMaterials
    {
        get
        {
            return purchasedMaterials.FindAll(x => x.IsPurchased).Count();
        }
        private set { }
    }
    public int NumberPurchasedModels
    {
        get
        {
            return purchasedModels.FindAll(x => x.IsPurchased).Count();
        }
        private set { }
    }
    void Awake()
    {
        //Read purchased materials from disc
        if (PlayerPrefs.HasKey(SaveKeys.PurchasedMaterials))
        {
            string serilPurchasedMaterials = PlayerPrefs.GetString(SaveKeys.PurchasedMaterials);
            ListContainer container = JsonUtility.FromJson<ListContainer>(serilPurchasedMaterials);
            purchasedMaterials = container.dataList;
            //Check than number of saved materials equal number of materials in scene
            /*if(purchasedMaterials.Count != planerMaterials.Count)
            {
                if (purchasedMaterials.Count < planerMaterials.Count)
                {
                    var newPlanerMaterials = planerMaterials.FindAll(x => purchasedMaterials.Any(y => y.Name == x.name));
                    foreach (var mat in newPlanerMaterials)
                        purchasedMaterials.Add(new PurchasedPlanerMaterial(mat.name, false));
                }
                else if(purchasedMaterials.Count > planerMaterials.Count)
                {
                    purchasedMaterials.RemoveAll(x => );
                }
            }*/
            //Check that saved material equal with materials in scene
            bool materialsIsEqual = true;
            if (purchasedMaterials.Count != planerMaterials.Count)
                materialsIsEqual = false;
            else
            {
                for (int i = 0; i < planerMaterials.Count; i++)
                {
                    if (!purchasedMaterials.Any(x => x.Name == planerMaterials[i].name))
                        materialsIsEqual = false;
                    if (planerMaterials.Any(x => x.name == purchasedMaterials[i].Name))
                        materialsIsEqual = false;
                }
            }
            if (!materialsIsEqual)
            {
                List<PurchasedPlanerMaterial> newPurchasedMaterials = new List<PurchasedPlanerMaterial>();
                for (int i = 0; i < planerMaterials.Count; i++)
                {
                    if (purchasedMaterials.Any(x => x.Name == planerMaterials[i].name))
                        newPurchasedMaterials.Add(purchasedMaterials.Find(x => x.Name == planerMaterials[i].name));
                    else
                        newPurchasedMaterials.Add(new PurchasedPlanerMaterial(planerMaterials[i].name, false));
                }
                purchasedMaterials = newPurchasedMaterials;
                purchasedMaterials[0].IsPurchased = true;
                SavePurchasedMaterials();
            }
        }
        else
        {
            purchasedMaterials = new List<PurchasedPlanerMaterial>();
            foreach (var mat in planerMaterials)
                purchasedMaterials.Add(new PurchasedPlanerMaterial(mat.name, false));
            purchasedMaterials.Find(x => x.Name == "mat_plane").IsPurchased = true;
        }
        //Read current material from disc
        if (PlayerPrefs.HasKey(SaveKeys.PlanerMaterial))
        {
            currentMaterial = PlayerPrefs.GetString(SaveKeys.PlanerMaterial);
            SelectSkin(currentMaterial);
        }
        else
        {
            PlayerPrefs.SetString(SaveKeys.PlanerMaterial, purchasedMaterials[0].Name);
            currentMaterial = purchasedMaterials[0].Name;
            SelectSkin(currentMaterial);
        }
    }
    void Start()
    {
        
    }
    void Update()
    {

    }
    public void SavePurchasedMaterials()
    {
        ListContainer container = new ListContainer(purchasedMaterials);
        var serPurchasedMaterials = JsonUtility.ToJson(container);
        PlayerPrefs.SetString(SaveKeys.PurchasedMaterials, serPurchasedMaterials);
    }
    public void SelectSkin(string materialName)
    {
        if (purchasedMaterials.Any(x => x.Name == materialName))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<MeshRenderer>().material = planerMaterials.Find(x => x.name == materialName);
            currentMaterial = materialName;
            PlayerPrefs.SetString(SaveKeys.PlanerMaterial, materialName);
        }
        /*else
            GameObject.FindGameObjectWithTag("Player").GetComponent<MeshRenderer>().material = defaultMaterial;
        PlayerPrefs.SetString(SaveKeys.PlanerMaterial, materialName);*/
    }
    public bool PurchaseSkin(string materialName)
    {

        bool isSuccess = false;
        if (Managers.Bonuses.NumberCrystalBonus >= materialPrice && purchasedMaterials.Any(x => x.Name == materialName))
        {
            Managers.Bonuses.SubstractCrystals(materialPrice);
            purchasedMaterials.Find(x => x.Name == materialName).IsPurchased = true;
            SelectSkin(materialName);
            SavePurchasedMaterials();
            isSuccess = true;
        }
        return isSuccess;
    }
}
public struct ListContainer
{
    public List<PurchasedPlanerMaterial> dataList;

    /// <summary>
	/// Constructor
	/// </summary>
	/// <param name="_dataList">Data list value</param>
	public ListContainer(List<PurchasedPlanerMaterial> _dataList)
    {
        dataList = _dataList;
    }
}
