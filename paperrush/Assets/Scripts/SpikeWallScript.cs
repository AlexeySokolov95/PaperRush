using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

//добавить шобы ворота несмели закрываться если самолётик уже влетел в них.
enum ClosedGates { Even, Odd };
public class SpikeWallScript : LevelBlock
{
    float timerOpenGate = 0;
    public float timeOpenGate = 2;
    public float closingSpeed = 100;
    float crackLength = 3f;
    ClosedGates closeGates = ClosedGates.Even;
    bool toClose = false;
    float originalScaleX = 1.16f;
    List<GameObject> gates =  new List<GameObject>();
    // Use this for initialization
    void Start()
    {
        Initialization(25f);
        PutWall();
        PutGate();        
        OriginalCloseGate();
    }
    // Update is called once per frame
    void Update()
    {
        if (toClose)
        {
            ToOpenGates();
            ToCloseGates();
            if (GatesClosed())
            {
                toClose = false;
                SwitchClosedGates();
            }
        }
        else
        {
            if (timerOpenGate < timeOpenGate)
                timerOpenGate += Time.deltaTime;
            else
            {
                timerOpenGate = 0;
                toClose = true;
            }
        }

    }
    private void SwitchClosedGates()
    {
        switch (closeGates)
        {
            case ClosedGates.Even:
                closeGates = ClosedGates.Odd;
                break;
            case ClosedGates.Odd:
                closeGates = ClosedGates.Even;
                break;
        }
    }
    private void PutGate()
    {        
        for (int i = 1; i <= 6; i++)
        {
            gates.Add(Instantiate(GameObject.Find("Gate" + i.ToString())));
            Vector3 oldPosition = gates[i-1].transform.position;
            gates[i-1].transform.position = new Vector3(oldPosition.x, oldPosition.y, zCoordinateBeginningOfBlock);
        }
        for (int i = 0; i < 6; i++)
        {
            Vector3 oldPosition = gates[i].transform.position;
            gates[i].transform.position = new Vector3(GetMinPositionXForGates(i), oldPosition.y, zCoordinateBeginningOfBlock);
        }
    }
    private void ToCloseGates()
    {
        switch (closeGates)
        {
            case ClosedGates.Even:
                for (int i = 0; i < 6; i += 2)
                {
                    Vector3 oldScale = gates[i].transform.localScale;
                    float newScaleX = oldScale.x + (closingSpeed * Time.deltaTime);
                    gates[i].transform.localScale = new Vector3(newScaleX, oldScale.y, oldScale.z);
                    if (gates[i].transform.localScale.x > GetMaxScaleXForGates())
                        gates[i].transform.localScale = new Vector3(GetMaxScaleXForGates(), oldScale.y, oldScale.z);
                    float deltaX = closingSpeed * Time.deltaTime / 2;
                    gates[i].transform.Translate(new Vector3(deltaX, 0, 0));
                    if (gates[i].transform.position.x > GetMaxPositionXForGates(i))
                    {
                        Vector3 oldPosition = gates[i].transform.position;
                        gates[i].transform.position = new Vector3(GetMaxPositionXForGates(i), oldPosition.y, oldPosition.z);
                    }
                }
                break;
            case ClosedGates.Odd:
                for (int i = 1; i < 6; i += 2)
                {
                    Vector3 oldScale = gates[i].transform.localScale;
                    float newScaleX = oldScale.x + (closingSpeed * Time.deltaTime);
                    gates[i].transform.localScale = new Vector3(newScaleX, oldScale.y, oldScale.z);
                    if (gates[i].transform.localScale.x > GetMaxScaleXForGates())
                        gates[i].transform.localScale = new Vector3(GetMaxScaleXForGates(), oldScale.y, oldScale.z);
                    float deltaX = closingSpeed * Time.deltaTime / 2;
                    gates[i].transform.Translate(new Vector3(deltaX, 0, 0));
                    if (gates[i].transform.position.x > GetMaxPositionXForGates(i))
                    {
                        Vector3 oldPosition = gates[i].transform.position;
                        gates[i].transform.position = new Vector3(GetMaxPositionXForGates(i), oldPosition.y, oldPosition.z);
                    }
                }
                break;
        }
    }
    private void ToOpenGates()
    {
        switch (closeGates)
        {
            case ClosedGates.Even:
                for (int i = 1; i < 6; i += 2)
                {
                    Vector3 oldScale = gates[i].transform.localScale;
                    float newScaleX = oldScale.x - (closingSpeed * Time.deltaTime);
                    gates[i].transform.localScale = new Vector3(newScaleX, oldScale.y, oldScale.z);
                    if (gates[i].transform.localScale.x <= GetMinScaleXForGates())
                        gates[i].transform.localScale = new Vector3(GetMinScaleXForGates(), oldScale.y, oldScale.z);
                    float deltaX = - (closingSpeed * Time.deltaTime / 2);
                    gates[i].transform.Translate(new Vector3(deltaX, 0, 0));
                    if (gates[i].transform.position.x <= GetMinPositionXForGates(i))
                    {
                        Vector3 oldPosition = gates[i].transform.position;
                        gates[i].transform.position = new Vector3(GetMinPositionXForGates(i), oldPosition.y, oldPosition.z);
                    }
                }
                break;
            case ClosedGates.Odd:
                for (int i = 0; i < 6; i += 2)
                {
                    Vector3 oldScale = gates[i].transform.localScale;
                    float newScaleX = oldScale.x - (closingSpeed * Time.deltaTime);
                    gates[i].transform.localScale = new Vector3(newScaleX, oldScale.y, oldScale.z);
                    if (gates[i].transform.localScale.x <= GetMinScaleXForGates())
                        gates[i].transform.localScale = new Vector3(GetMinScaleXForGates(), oldScale.y, oldScale.z);
                    float deltaX = -(closingSpeed * Time.deltaTime / 2);
                    gates[i].transform.Translate(new Vector3(deltaX, 0, 0));
                    if (gates[i].transform.position.x <= GetMinPositionXForGates(i))
                    {
                        Vector3 oldPosition = gates[i].transform.position;
                        gates[i].transform.position = new Vector3(GetMinPositionXForGates(i), oldPosition.y, oldPosition.z);
                    }
                }
                break;
        }
    }
    private void OriginalCloseGate()
    {
        for (int i = 1; i < 6; i+=2)
        {
            Transform oldTransform = gates[i].transform;
            gates[i].transform.localScale = new Vector3(GetMaxScaleXForGates(), oldTransform.localScale.y, oldTransform.localScale.z);
            gates[i].transform.position = new Vector3(GetMaxPositionXForGates(i), oldTransform.position.y, oldTransform.position.z);
        }
    }
    private bool GatesClosed()

    { 
        int numberClosedAndOpenGates = 0;
        switch (closeGates)
        {
            case ClosedGates.Even:
                for (int i = 0; i < 6; i += 2)
                {
                    if (gates[i].transform.localScale.x == GetMaxScaleXForGates() && gates[i].transform.position.x == GetMaxPositionXForGates(i))
                        numberClosedAndOpenGates++;
                }
                for (int i = 1; i < 6; i += 2)
                {
                    if (gates[i].transform.localScale.x == GetMinScaleXForGates() && gates[i].transform.position.x == GetMinPositionXForGates(i))
                        numberClosedAndOpenGates++;
                }
                    break;
            case ClosedGates.Odd:
                for (int i = 1; i < 6; i += 2)
                {
                    if (gates[i].transform.localScale.x == GetMaxScaleXForGates() && gates[i].transform.position.x == GetMaxPositionXForGates(i))
                        numberClosedAndOpenGates++;
                }
                for (int i = 0; i < 6; i += 2)
                {
                    if (gates[i].transform.localScale.x == GetMinScaleXForGates() && gates[i].transform.position.x == GetMinPositionXForGates(i))
                        numberClosedAndOpenGates++;
                }
                break;
        }
        bool answ = false;
        if (numberClosedAndOpenGates == 6)
            answ = true;
        return answ;
    }
    private float GetMaxScaleXForGates()
    {
        return originalScaleX + crackLength;
    }
    private float GetMaxPositionXForGates(int i)
    {
        float answ = -12.5f + ((crackLength / 2 + originalScaleX) + (crackLength + originalScaleX) * i) - originalScaleX / 2;
        return answ;
    }
    private float GetMinScaleXForGates()
    {
        return originalScaleX;
    }
    private float GetMinPositionXForGates(int i)
    {
        return -12.5f + (originalScaleX /2 * (i+1)) + crackLength * i;
    }
}
