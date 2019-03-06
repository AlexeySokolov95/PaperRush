using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;

public class InitGAScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        GameAnalytics.Initialize();
        SceneManager.LoadScene("SampleScene");
    }

    // Update is called once per frame
    void Update()
    {

    }
}