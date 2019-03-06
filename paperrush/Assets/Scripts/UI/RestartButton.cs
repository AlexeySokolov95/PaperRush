using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RestartButton : MonoBehaviour
{

    void Start()
    {
        

    }

    void Update()
    {

    }
    void OnMouseDown()
    {
        Managers.Restart();
    }
}
