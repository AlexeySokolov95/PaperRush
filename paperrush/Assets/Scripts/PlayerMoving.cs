using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Assets.Class;


public class PlayerMoving : MonoBehaviour
{
    private CapsuleCollider _capsCollider;
    public float movingSpeed = 30.0f;
    public float gorizontalSpeed = 1f;
    public float fallingSpeed = 0f;
    public float distanceOfFall = 5;
    public float climbHeight = 6.80f;
    public float climbSpeed = 10.0f;
    public float rotationXSpeed = 10.0f;
    public float waitRotationOffset = 0.5f;
    public float maxRotation = 45.0f;
    public float minRotation = -45.0f;   
    private float rotationX = 0;
    private bool climbIsСompleted = true;
    private float climbStartYCoordinates = 0;
    private float climbEndYCoordinates = 0;
    public float maxPlayerYCordinate = 10f;
    public ParticleSystem dieParticle;
    private bool firstClimbBonusIsFound = false;
    protected LevelCreater LevelManager;
    protected float widthWall;
    protected float heightWall;
    private bool isMoving = true;
    void Start()
    {
        _capsCollider = GetComponent<CapsuleCollider>();
        LevelManager = GameObject.FindWithTag("Level Creater").GetComponent<LevelCreater>(); widthWall = LevelManager.WidthWall;
        heightWall = LevelManager.HeightWall;
        maxPlayerYCordinate = heightWall - 2;
        distanceOfFall = maxPlayerYCordinate*0.9f;
        climbHeight = maxPlayerYCordinate;
        transform.position = new Vector3(transform.position.x, maxPlayerYCordinate, transform.position.z);
        isMoving = false;
    }
    void Update()
    {
        if (isMoving)
        {
            float deltaX = Input.GetAxis("Horizontal") * 40; ;
            if (Input.touches.Length > 0)
                deltaX = Input.touches[0].deltaPosition.x * gorizontalSpeed;
            float yDelta = FindPlayerDeltaNewYCoordinate();
            transform.Translate(deltaX * Time.deltaTime, yDelta, movingSpeed * Time.deltaTime, Space.World);
            if (Input.GetAxis("Horizontal") != 0)
            {
                rotationX += Input.GetAxis("Horizontal") * rotationXSpeed * Time.deltaTime;
                rotationX = Mathf.Clamp(rotationX, minRotation, maxRotation);

            }
            else
            {
                if (rotationX < 0)
                {
                    rotationX += waitRotationOffset * Time.deltaTime;
                    if (rotationX >= 0)
                        rotationX = 0;
                }
                else if (rotationX > 0)
                {
                    rotationX -= waitRotationOffset * Time.deltaTime;
                    if (rotationX <= 0)
                        rotationX = 0;
                }
            }
            transform.localEulerAngles = new Vector3(-90 + rotationX, 90, 90);
            if (IsLost())
                Restart();
            /*if (BonusIsMissed())
                AddNewFallingSpeed();*/
            if (!firstClimbBonusIsFound)
                PutStartingFallingSpeed();
        }
        
    }
    /*void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * 40; ;
        if (Input.touches.Length > 0)
            deltaX = Input.touches[0].deltaPosition.x * gorizontalSpeed;
        float yDelta = FindPlayerDeltaNewYCoordinate();
        transform.Translate(deltaX * Time.deltaTime, yDelta, movingSpeed * Time.deltaTime, Space.World);
        if (Input.GetAxis("Horizontal") != 0)
        {
            rotationX += Input.GetAxis("Horizontal") * rotationXSpeed * Time.deltaTime;
            rotationX = Mathf.Clamp(rotationX, minRotation, maxRotation);

        }
        else
        {
            if (rotationX < 0)
            {
                rotationX += waitRotationOffset * Time.deltaTime;
                if (rotationX >= 0)
                    rotationX = 0;
            }
            else if (rotationX > 0)
            {
                rotationX -= waitRotationOffset * Time.deltaTime;
                if (rotationX <= 0)
                    rotationX = 0;
            }
        }
        transform.localEulerAngles = new Vector3(-90 + rotationX, 90, 90);
    }*/
    void FixedUpdate()
    {/*
        float deltaX = Input.GetAxis("Horizontal") * 40; ;
        if (Input.touches.Length > 0)
            deltaX = Input.touches[0].deltaPosition.x * gorizontalSpeed;
        float yDelta = FindPlayerDeltaNewYCoordinate();
        transform.Translate(deltaX * Time.deltaTime, yDelta, movingSpeed * Time.deltaTime, Space.World);
        if (Input.GetAxis("Horizontal") != 0)
        {
            rotationX += Input.GetAxis("Horizontal") * rotationXSpeed * Time.deltaTime;
            rotationX = Mathf.Clamp(rotationX, minRotation, maxRotation);

        }
        else
        {
            if (rotationX < 0)
            {
                rotationX += waitRotationOffset * Time.deltaTime;
                if (rotationX >= 0)
                    rotationX = 0;
            }
            else if (rotationX > 0)
            {
                rotationX -= waitRotationOffset * Time.deltaTime;
                if (rotationX <= 0)
                    rotationX = 0;
            }
        }
        transform.localEulerAngles = new Vector3(-90 + rotationX, 90, 90);*/
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Climb Bonus")
        {
            col.gameObject.SetActive(false);
            TakeBonusFromUpper();
        }
        else if (col.transform.tag == "Obstacle")
        {
            StartCoroutine(Die());
        }
        else if (col.transform.tag == "Crystal Bonuses")
        {
              
        }
    }
    private void TakeBonusFromUpper()
    {
        climbIsСompleted = false;
        climbEndYCoordinates = climbHeight;
        if (climbEndYCoordinates > maxPlayerYCordinate)
            climbEndYCoordinates = maxPlayerYCordinate;
        AddNewFallingSpeed();
        //AddCollection
    }
    private void CheckClimbIsСompleted()
    {
        if (transform.position.y >= climbEndYCoordinates)
        {
            climbIsСompleted = true;
            transform.position = new Vector3(transform.position.x, climbEndYCoordinates, transform.position.z);
        }
    }
    private GameObject FindNearestClimbBonus( )
    {
        GameObject[] allClimbBonus = GameObject.FindGameObjectsWithTag("Climb Bonus");
        GameObject nearClimbBonus = allClimbBonus.First(bonus => bonus.transform.position.z == allClimbBonus.Where(x => transform.position.z + 5 < x.transform.position.z).Min(y => y.transform.position.z));
        return nearClimbBonus;
    }
    void AddNewFallingSpeed()
    {
        GameObject nearClimbBonus = FindNearestClimbBonus();
        float distanceToNearestClimbBonus = nearClimbBonus.transform.position.z - transform.position.z;
        fallingSpeed = distanceOfFall / (distanceToNearestClimbBonus / movingSpeed);
    }
    private float FindPlayerDeltaNewYCoordinate()
    {
        float yDelta = 0;
        if (!climbIsСompleted)
        {
            CheckClimbIsСompleted();
            yDelta = climbSpeed * Time.deltaTime;
            if (transform.position.y + yDelta > climbEndYCoordinates)
                yDelta = climbEndYCoordinates - transform.position.y;
        }
        else
            yDelta = -fallingSpeed * Time.deltaTime;
        return yDelta;
    }
    void PutStartingFallingSpeed()
    {
        GameObject[] allClimbBonus = GameObject.FindGameObjectsWithTag("Climb Bonus");
        if (allClimbBonus.Length > 0)
        {
            AddNewFallingSpeed();
            fallingSpeed = fallingSpeed * 0.7f;
            firstClimbBonusIsFound = true;
        }
    }
    bool BonusIsMissed()
    {
        GameObject[] allClimbBonus = GameObject.FindGameObjectsWithTag("Climb Bonus");
        return allClimbBonus.Any(bonus => transform.position.z - bonus.transform.position.z > 2);
    }
    bool IsLost()
    {
        bool answ = false;
        if (transform.position.x < -LevelManager.WidthWall / 2 ||
            transform.position.x > LevelManager.WidthWall / 2 ||
            transform.position.y < 0 ||
            transform.position.y > LevelManager.HeightWall)
            answ = true;
        return answ;
    }
    IEnumerator Die()
    {
        isMoving = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        dieParticle.Play();
        yield return new WaitForSeconds(dieParticle.main.duration);
        Messenger.Broadcast<int>(GameEvent.TryAddNewRecord, (int)transform.position.z);
        Messenger.Broadcast(GameEvent.PlaneIsBroken);
        gameObject.SetActive(false);
    }
    void Restart()
    {

    }
}
