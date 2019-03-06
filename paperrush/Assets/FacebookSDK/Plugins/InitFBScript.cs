using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class InitFBScript : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() => {
                //FB.Mobile.SetAutoLogAppEventsEnabled(true);
                FB.ActivateApp();
            });
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
