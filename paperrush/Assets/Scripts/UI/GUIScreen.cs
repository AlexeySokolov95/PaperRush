using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;


public class GUIScreen : MonoBehaviour, IGUIScreen
{
    public bool IsVisible { get; protected set; }
    [SerializeField]
    protected GameObject generalGameObject;
    protected UIController GUIController;
    void Start()
    {
        GUIController = GameObject.Find("GUIController").GetComponent<UIController>();
    }
    public virtual void ShowScreen()
    {
        generalGameObject.SetActive(true);
        IsVisible = true;
    }
    public virtual void HideScreen()
    {
        generalGameObject.SetActive(false);
        IsVisible = false;
    }
}
