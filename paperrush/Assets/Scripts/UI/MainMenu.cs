using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Class;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class MainMenu : GUIScreen
    {
        public float animationDuration = 0.3f;
        public float canvasHeight = 1426f;
        public float canvasWidth = 800f;
        private ColorSchemasManager colorManager;
        private ColorSchema curentSchema;
        public Sprite[] blueSchema;
        public Sprite[] greenSchema;
        public Sprite[] purpleSchema;
        private List<Image> colorButtonsImages;
        [SerializeField]
        private Text numberOfCrystals;
        [SerializeField]
        private Text numberOfPoints;
        int oldNumberOfCrystalBonus = 0;
        float oldRecordGamePoints = 0;

        RectTransform logo;
        RectTransform PaperCrafts;
        RectTransform Stars5;
        RectTransform Settings;
        RectTransform Stats;

        public RectTransform Finger;
        public RectTransform FingerBack;
        public float fingerDuration = 0.7f;
        Sequence fingerSequence;
        Sequence paperCraftsSequence;

        private Vector2 logoPos;
        private Vector2 paperCraftPos;
        private Vector2 stars5Pos;
        private Vector2 settingsPos;
        private Vector2 statsPos;

        private bool soundIsMut;
        public GameObject mutUnmutButton;
        public Sprite[] mutUnmutSchemas;


        public override void ShowScreen()
        {
            numberOfCrystals.text = Managers.Bonuses.NumberCrystalBonus.ToString();
            numberOfPoints.text = Managers.Records.RecordPoints.ToString();
            generalGameObject.SetActive(true);
            IsVisible = true;
            //Logo

            float pozY = logoPos.y;
            float startLogoYPoz = canvasHeight / 2 + logo.rect.height;
            logo.anchoredPosition = new Vector3(0, startLogoYPoz);
            logo.DOAnchorPosY(pozY, animationDuration).SetEase(Ease.OutBack);
            //PaperCrafts

            float endPozX = paperCraftPos.x;
            float startXPoz = -(canvasWidth / 2 + PaperCrafts.rect.width);
            PaperCrafts.anchoredPosition = new Vector3(startXPoz, PaperCrafts.anchoredPosition.y);
            PaperCrafts.DOAnchorPosX(endPozX, animationDuration).SetEase(Ease.OutQuart);
            //Stars5

            endPozX = stars5Pos.x;
            startXPoz = -(canvasWidth / 2 + Stars5.rect.width);
            Stars5.anchoredPosition = new Vector3(startXPoz, Stars5.anchoredPosition.y);
            Stars5.DOAnchorPosX(endPozX, animationDuration).SetEase(Ease.OutQuart);
            //Settings

            endPozX = settingsPos.x;
            startXPoz = canvasWidth / 2 + Settings.rect.width;
            Settings.anchoredPosition = new Vector3(startXPoz, Settings.anchoredPosition.y);
            Settings.DOAnchorPosX(endPozX, animationDuration).SetEase(Ease.OutQuart);
            //Stats

            endPozX = statsPos.x;
            startXPoz = canvasWidth / 2 + Stats.rect.width;
            Stats.anchoredPosition = new Vector3(startXPoz, Stats.anchoredPosition.y);
            Stats.DOAnchorPosX(endPozX, animationDuration).SetEase(Ease.OutQuart);

            generalGameObject.transform.Find("image_crystal").gameObject.SetActive(true);
            generalGameObject.transform.Find("txt_numberOfCrystal").gameObject.SetActive(true);
            generalGameObject.transform.Find("txt_recordOfPoints").gameObject.SetActive(true);
            generalGameObject.transform.Find("RecordImage").gameObject.SetActive(true);

            Finger.gameObject.SetActive(true);
            Finger.anchoredPosition = new Vector2(-205, Finger.anchoredPosition.y);
            FingerBack.gameObject.SetActive(true);
            fingerSequence = DOTween.Sequence();
            fingerSequence.Append(Finger.DOAnchorPosX(222,fingerDuration).SetEase(Ease.OutCubic));
            fingerSequence.Append(Finger.DOAnchorPosX(-205, fingerDuration).SetEase(Ease.OutCubic));
            fingerSequence.SetLoops(-1);

            if (Managers.Bonuses.NumberCrystalBonus >= GameObject.Find("CustomizePlanerManager").GetComponent<CustomizePlanerManagerScript>().materialPrice) 
            {
                PaperCrafts.gameObject.SetActive(true);
                PaperCrafts.sizeDelta = new Vector2(150, 150);
                paperCraftsSequence = DOTween.Sequence();
                paperCraftsSequence.Append(PaperCrafts.DOSizeDelta(new Vector2(180, 180), 1f));
                paperCraftsSequence.Append(PaperCrafts.DOSizeDelta(new Vector2(150, 150), 1f));
                paperCraftsSequence.SetLoops(-1);
            }

            //MutUnmutButton
            mutUnmutButton.SetActive(false);
            int trueFalse = 0;
            if (PlayerPrefs.HasKey(SaveKeys.SoundIsMuted))
                trueFalse = PlayerPrefs.GetInt(SaveKeys.SoundIsMuted);
            else
                PlayerPrefs.SetInt(SaveKeys.SoundIsMuted, 0);
            if (trueFalse == 1)
            {
                soundIsMut = true;
                AudioListener.volume = 0.0f;
            }
            else
            {
                soundIsMut = false;
                AudioListener.volume = 1f;
            }
        }
        public override void HideScreen()
        {
            //Logo
            RectTransform logo = generalGameObject.transform.Find("Logo").GetComponent<RectTransform>();
            float endLogoYPoz = canvasHeight / 2 + logo.rect.height;
            logo.DOAnchorPosY(endLogoYPoz, animationDuration).SetEase(Ease.InQuart);
            //PaperCrafts
            RectTransform PaperCrafts = generalGameObject.transform.Find("PaperCrafts").GetComponent<RectTransform>();
            float endPozX = -(canvasWidth / 2 + PaperCrafts.rect.width);
            PaperCrafts.DOAnchorPosX(endPozX, animationDuration).SetEase(Ease.OutQuart);
            //Stars5
            RectTransform Stars5 = generalGameObject.transform.Find("Stars5").GetComponent<RectTransform>();
            endPozX = -(canvasWidth / 2 + Stars5.rect.width);
            Stars5.DOAnchorPosX(endPozX, animationDuration).SetEase(Ease.OutQuart);
            //Settings
            RectTransform Settings = generalGameObject.transform.Find("Settings").GetComponent<RectTransform>();
            endPozX = canvasWidth / 2 + Settings.rect.width;
            Settings.DOAnchorPosX(endPozX, animationDuration).SetEase(Ease.OutQuart);
            //Settings
            RectTransform Stats = generalGameObject.transform.Find("Stats").GetComponent<RectTransform>();
            endPozX = canvasWidth / 2 + Stats.rect.width;
            Stats.DOAnchorPosX(endPozX, animationDuration).SetEase(Ease.OutQuart);

            generalGameObject.transform.Find("image_crystal").gameObject.SetActive(false);
            generalGameObject.transform.Find("txt_numberOfCrystal").gameObject.SetActive(false);
            generalGameObject.transform.Find("txt_recordOfPoints").gameObject.SetActive(false);
            generalGameObject.transform.Find("RecordImage").gameObject.SetActive(false);
            //Hide Finger and FingerBack
            Finger.gameObject.SetActive(false);
            FingerBack.gameObject.SetActive(false);
            fingerSequence.Kill();
            paperCraftsSequence.Kill();
            mutUnmutButton.SetActive(false);
            StartCoroutine(WaitAnimation());
        }
        public void Play()
        {
            GUIController = GameObject.Find("GUIController").GetComponent<UIController>();
            GUIController.ShowLayer(GUIController.GameGUI);
            //Time.timeScale = 1f;
            GameObject.FindGameObjectWithTag("Player").GetComponent<RBPlayerMoving>().isMoving = true;
        }
        public void ShowSettings()
        {
            float animDuration = 0.5f;
            if (!mutUnmutButton.activeSelf)
            {
                mutUnmutButton.SetActive(true);
                RectTransform mutUnmutRect = mutUnmutButton.GetComponent<RectTransform>();
                mutUnmutRect.DOAnchorPosY(Settings.anchoredPosition.y + 200, animDuration);
            }
            else
            {
                StartCoroutine(HideMutunmutSettings(animDuration));
            }
        }
        private IEnumerator HideMutunmutSettings(float animDuration)
        {
            RectTransform mutUnmutRect = mutUnmutButton.GetComponent<RectTransform>();
            mutUnmutRect.DOAnchorPosY(Settings.anchoredPosition.y, animDuration);
            yield return new WaitForSeconds(animDuration);
            mutUnmutButton.SetActive(false);
        }
        public void MutUnmutSound()
        {
            soundIsMut = !soundIsMut;
            if (soundIsMut)
            {
                AudioListener.volume = 0f;
                ChangeMutUnmutIcon();
                PlayerPrefs.SetInt(SaveKeys.SoundIsMuted, 1);
            }
            else
            {
                AudioListener.volume = 1f;
                ChangeMutUnmutIcon();
                PlayerPrefs.SetInt(SaveKeys.SoundIsMuted, 0);
            }
        }
        private void ChangeMutUnmutIcon()
        {
            Image mutUnmutImage = mutUnmutButton.GetComponent<Image>();
            switch (curentSchema)
            {
                case ColorSchema.Blue:
                    if (soundIsMut)
                        mutUnmutImage.sprite = mutUnmutSchemas[0];
                    else
                        mutUnmutImage.sprite = mutUnmutSchemas[1];
                    break;
                case ColorSchema.Green:
 
                    if (soundIsMut)
                        mutUnmutImage.sprite = mutUnmutSchemas[2];
                    else
                        mutUnmutImage.sprite = mutUnmutSchemas[3];
                    break;
                case ColorSchema.Purple:
                    if (soundIsMut)
                        mutUnmutImage.sprite = mutUnmutSchemas[4];
                    else
                        mutUnmutImage.sprite = mutUnmutSchemas[5];
                    break;
                default:
                    break;
            }
        }
        public void ShowFiveStars()
        {
            if(Managers.Bonuses.NumberCrystalBonus != 0)
                PlayerPrefs.SetInt(SaveKeys.NumberOfCrystalBonuses, 0);
            else
                PlayerPrefs.SetInt(SaveKeys.NumberOfCrystalBonuses, 623);
        }
        public void CustomizePlanes()
        {
            GUIController = GameObject.Find("GUIController").GetComponent<UIController>();
            GUIController.ShowLayer(GUIController.CustomizePlaner);
        }
        public void ShowStats()
        {
            if (Managers.Records.RecordPoints != 0)
                PlayerPrefs.SetInt(SaveKeys.RecordPoints, 0);
            else
                PlayerPrefs.SetInt(SaveKeys.RecordPoints, 854);
        }
        private IEnumerator WaitAnimation()
        {
            IsVisible = false;
            yield return new WaitForSeconds(animationDuration);
            if (!IsVisible)
            {
                generalGameObject.SetActive(false);
                IsVisible = false;
            }
        }
        void Update()
        {
            bool colorChemaIsChanged = curentSchema != colorManager.curentSchema;
            if (colorChemaIsChanged)
            {
                curentSchema = colorManager.curentSchema;
                ChangeColorIcons();
            }
            if (gameObject != null)
            {
                if (oldNumberOfCrystalBonus != Managers.Bonuses.NumberCrystalBonus)
                {
                    numberOfCrystals.text = Managers.Bonuses.NumberCrystalBonus.ToString();
                    oldNumberOfCrystalBonus = Managers.Bonuses.NumberCrystalBonus;
                }
                if (oldRecordGamePoints != Managers.Records.RecordPoints)
                {
                    numberOfPoints.text = Managers.Records.RecordPoints.ToString();
                    oldRecordGamePoints = Managers.Records.RecordPoints;
                }
            }
        }
        void Awake()
        {
            GUIController = GameObject.Find("GUIController").GetComponent<UIController>();
            colorManager = GameObject.Find("ColorSchemasManager").GetComponent<ColorSchemasManager>();
            curentSchema = colorManager.curentSchema;
            colorButtonsImages = new List<Image>();
            colorButtonsImages.Add(generalGameObject.transform.Find("PaperCrafts").GetComponent<Image>());
            colorButtonsImages.Add(generalGameObject.transform.Find("Stars5").GetComponent<Image>());
            colorButtonsImages.Add(generalGameObject.transform.Find("Stats").GetComponent<Image>());
            colorButtonsImages.Add(generalGameObject.transform.Find("Settings").GetComponent<Image>());
            colorButtonsImages.Add(generalGameObject.transform.Find("Logo").GetComponent<Image>());

            logo = generalGameObject.transform.Find("Logo").GetComponent<RectTransform>();
            PaperCrafts = generalGameObject.transform.Find("PaperCrafts").GetComponent<RectTransform>();
            Stars5 = generalGameObject.transform.Find("Stars5").GetComponent<RectTransform>();
            Settings = generalGameObject.transform.Find("Settings").GetComponent<RectTransform>();
            Stats = generalGameObject.transform.Find("Stats").GetComponent<RectTransform>();

            logoPos = logo.anchoredPosition;
            paperCraftPos = PaperCrafts.anchoredPosition;
            stars5Pos = Stars5.anchoredPosition;
            settingsPos = Settings.anchoredPosition;
            statsPos = Stats.anchoredPosition;

        }
        void Start()
        {
            GUIController = GameObject.Find("GUIController").GetComponent<UIController>();
        }
        private void ChangeColorIcons()
        {
            Image mutUnmutImage = mutUnmutButton.GetComponent<Image>();
            switch (curentSchema)
            {
                case ColorSchema.Blue:
                    for (int i = 0; i < colorButtonsImages.Count; i++)
                        colorButtonsImages[i].sprite = blueSchema[i];
                    if (soundIsMut)
                        mutUnmutImage.sprite = mutUnmutSchemas[0];
                    else
                        mutUnmutImage.sprite = mutUnmutSchemas[1];
                    break;
                case ColorSchema.Green:
                    for (int i = 0; i < colorButtonsImages.Count; i++)
                        colorButtonsImages[i].sprite = greenSchema[i];
                    if (soundIsMut)
                        mutUnmutImage.sprite = mutUnmutSchemas[2];
                    else
                        mutUnmutImage.sprite = mutUnmutSchemas[3];
                    break;
                case ColorSchema.Purple:
                    for (int i = 0; i < colorButtonsImages.Count; i++)
                        colorButtonsImages[i].sprite = purpleSchema[i];
                    if (soundIsMut)
                        mutUnmutImage.sprite = mutUnmutSchemas[4];
                    else
                        mutUnmutImage.sprite = mutUnmutSchemas[5];
                    break;
                default:
                    break;
            }
        }
    }
}

