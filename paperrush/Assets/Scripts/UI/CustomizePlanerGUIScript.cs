using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CustomizePlanerScript : GUIScreen
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void Back()
        {
            GUIController = GameObject.Find("GUIController").GetComponent<UIController>();
            GUIController.ShowLayer(GUIController.MainMenuScreen);
        }
        
    }
}