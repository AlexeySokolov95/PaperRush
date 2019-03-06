using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float maxRotationZ = 45;
    public float rotationZSpeed = 1400;
    public float stabilizeRotZSpeed = 15;
    public float maxRotationX = 25;
    public float rotationXSpeed = 180;
    public float stabilizeRotXSpeed = 10;

    private float yPoz = 0;
    private float xPoz = 0;
    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Made rotation only positive
        float deltaPozX = xPoz - transform.position.x;
        xPoz = transform.position.x;
        float deltaRotationZ = 0;
        //Culk the rotation Z
        float trueAngleZ = 0;
        if (transform.localEulerAngles.z > 0 && transform.localEulerAngles.z < 180)
            trueAngleZ = transform.localEulerAngles.z;
        else
            trueAngleZ = -360 + transform.localEulerAngles.z;
        if (trueAngleZ <= -360)
            trueAngleZ = trueAngleZ + 360;
        if (deltaPozX != 0)
            deltaRotationZ = ((rotationZSpeed * deltaPozX) + (-trueAngleZ * stabilizeRotZSpeed)) * Time.deltaTime;
        else
            deltaRotationZ = -trueAngleZ * stabilizeRotZSpeed * Time.deltaTime;
        //Culk the rotation Y
        float deltaPozY = yPoz - transform.position.y;
        yPoz = transform.position.y;
        float deltaRotationX = 0;
        float trueAngleX = 0;
        if (transform.localEulerAngles.x > 0 && transform.localEulerAngles.x < 180)
            trueAngleX = transform.localEulerAngles.x;
        else
            trueAngleX = -360 + transform.localEulerAngles.x;
        if(trueAngleX <= -360)
            trueAngleX = trueAngleX + 360;
        if (deltaPozY < 0)
            deltaRotationX = (-rotationXSpeed + (-trueAngleX * stabilizeRotXSpeed)) * Time.deltaTime;
        else
            deltaRotationX = -trueAngleX * stabilizeRotXSpeed * Time.deltaTime;
        
        transform.Rotate(new Vector3(deltaRotationX, 0, deltaRotationZ));
        //If the rotation have exceeded the limit
        if (transform.localEulerAngles.z > maxRotationZ && transform.localEulerAngles.z < 180)
             transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, maxRotationZ);
         if ((transform.localEulerAngles.z < 360 - maxRotationZ && transform.localEulerAngles.z > 180) || transform.localEulerAngles.z < -maxRotationZ)
             transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 360 - maxRotationZ);
        if (trueAngleX < -maxRotationX)
             transform.localEulerAngles = new Vector3(360 - maxRotationX, transform.localEulerAngles.y, transform.localEulerAngles.z);
        if (trueAngleX > 0)
             transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
        if (transform.localEulerAngles.y != 0)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);

    }
    private float RotationMultiplier()
    {
        float rotationMultiplier = 1;
        float deltaRotation = 0;
        float maxRotationMultiplier = 5;
        float absRotationZAngle = Mathf.Abs(transform.localEulerAngles.z);
        rotationMultiplier = absRotationZAngle / maxRotationZ * maxRotationMultiplier;

        if (deltaRotation > 0)
        {
            if (transform.localEulerAngles.z > 0)
            {

            }
        }
        return rotationMultiplier;
    }
    private float MakeRotationPositive(float rotation)
    {
        float positiveRotation = 0;
        if (rotation < -360)
            rotation = rotation + 360;
        positiveRotation = 360 + rotation;
        return positiveRotation;
    }
    private float StabilizeRotation()
    {
        float stabilizeRotation = 0;

        return stabilizeRotation;
    }
}
