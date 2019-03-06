using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;
using UnityEngine.UI;

public class CorrectionMenu : GUIScreen
{

    public InputField input1;

    public InputField input2;

    public InputField input3;

    RBPlayerMoving player;
    PlayerRotation playerRotation;
    void Start ()
    {
        player = GameObject.FindWithTag("Player").GetComponent<RBPlayerMoving>();
        playerRotation = GameObject.FindWithTag("Player").GetComponent<PlayerRotation>();
        input1.text = playerRotation.rotationZSpeed.ToString();
        input2.text = playerRotation.stabilizeRotZSpeed.ToString();
        input3.text = player.deltaStrightForce.ToString();
    }
    public override void ShowScreen()
    {
        generalGameObject.SetActive(true);
        IsVisible = true;
    }
    public override void HideScreen()
    {
        generalGameObject.SetActive(false);
        IsVisible = false;
    }
    // Update is called once per frame
    void Update ()
    {
		
	}
    void SetVariable()
    {
    }
}
