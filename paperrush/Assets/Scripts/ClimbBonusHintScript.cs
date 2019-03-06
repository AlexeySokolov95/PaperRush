using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;


public class ClimbBonusHintScript : MonoBehaviour
{
    public bool isWasShown = false;
    public int numberOfShowing = 0;


    void Start ()
    {

        if (PlayerPrefs.HasKey(SaveKeys.NumberOfShowingClimbBonusHint))
            numberOfShowing = PlayerPrefs.GetInt(SaveKeys.NumberOfShowingClimbBonusHint);
        else
            PlayerPrefs.SetInt(SaveKeys.NumberOfShowingClimbBonusHint, 0);
        DontDestroyOnLoad(this.gameObject);
	}

	void Update ()
    {
		
	}
    public void IsSwown()
    {
        isWasShown = true;
        numberOfShowing++;
        PlayerPrefs.SetInt(SaveKeys.NumberOfShowingClimbBonusHint, numberOfShowing);

    }
}
