using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Assets.Class;
public class RBPlayerMoving : MonoBehaviour
{
    public float gorizontalForce;
    public float strightForce;

    public float periodChanges = 0;
    public float deltaStrightForce = 0;
    public ParticleSystem dieParticle;
    public float maxYCoord = 12; //Изменить на 12
    public float movingUpSpeed = 100;
    private Rigidbody playerRB;
    public bool isMoving = false;
    private VerticalMovingDirection playerYDirection;
    private float movingDownSpeed = 0;
    public float dragDelta = 450.7379f;// 9.89f
    private bool nextClimbBonusIsFounded = false;
    private float maxY = 0;
    private bool firstDownMoving = true;
    private AudioSource crushSound;
    public ParticleSystem ps_climbBlue;
    public ParticleSystem ps_climbPurple;

    public float DeltaSpeed
    {
        set { deltaStrightForce = value; }
        get { return deltaStrightForce; }
    }
    void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        crushSound = GetComponent<AudioSource>();
    }
    void Start()
    {

        playerYDirection = VerticalMovingDirection.Down;
        Time.timeScale = 1f;
        isMoving = false;
        //GetComponent<MeshFilter>().sharedMesh = (Resources.Load("pln1", typeof(GameObject)) as GameObject).GetComponent<MeshFilter>().sharedMesh;
        //playerRB.AddForce(Vector3.forward * strightForce, ForceMode.VelocityChange);
        //transform.position = new Vector3(0, maxYCoord, maxZCoord);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.timeScale == 1)
            Time.timeScale = 0.01f;
        else if (Input.GetKeyDown(KeyCode.Space))
            Time.timeScale = 1f;
    }
    void FixedUpdate()
    {
        
        //float deltaX = Input.GetAxis("Horizontal");
        //playerRB.AddForce(Vector3.right * gorizontalForce * Input.GetAxis("Horizontal")*Time.deltaTime, ForceMode.Force);
        Vector3 movingX = Vector3.zero;
        Vector3 movingY = Vector3.zero;
        Vector3 movingZ = Vector3.zero;
        if (IsOutsideLevel())
            Die();
        if (isMoving)
        {
            movingX = GetMovingX();
            movingY = GetMovingY();
            movingZ = GetMovingZ();               
            playerRB.AddForce(movingX + movingY + movingZ, ForceMode.Force);
            strightForce += deltaStrightForce;
        }
        
    }
    float DeltaXModifier(float deltaX)
    {
        float newDeltaX = 0;
        newDeltaX = deltaX + (deltaX * Mathf.Cos(deltaX * Mathf.PI / 180));
        return newDeltaX;
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Climb Bonus")
        {
            col.gameObject.SetActive(false);
            //TakeBonusFromUpper();
        }
        else if (col.transform.tag == "Obstacle" || col.transform.tag == "LevelObstacle" || col.transform.tag == "DownRoad" || col.transform.tag == "Angle")
        {
            StartCoroutine(Die());
        }
        else if (col.transform.tag == "Crystal Bonuses")
        {
            col.gameObject.SendMessage("BonusIsCatched");
            Messenger.Broadcast(GameEvent.CrystalBonusCatched);
        }
    }
    Vector3 GetMovingX()
    {
        Vector3 movingX = Vector3.zero;
        if (Input.touches.Length > 0 && Input.touches[0].deltaPosition.x != 0)
            movingX = Vector3.right * gorizontalForce * Time.deltaTime * Input.touches[0].deltaPosition.x;
        float deltaX = Input.GetAxis("Horizontal") * 40;
        transform.Translate(deltaX * Time.deltaTime, 0, 0, Space.World);
        return movingX;
    }
    Vector3 GetMovingY()
    {
        Vector3 movingY = Vector3.zero;
        if(playerYDirection == VerticalMovingDirection.Down)
        {
            if (!nextClimbBonusIsFounded)
                SetPlayerDownMovingSpeed();
            movingY = Vector3.down * movingDownSpeed;
        }
        else if(playerYDirection == VerticalMovingDirection.Up)
        {
            if (transform.position.y < maxYCoord)
                movingY = Vector3.up * movingUpSpeed;
            else
                ClimbIsCompleted();
        }
        return movingY;
    }
    public void ClimbBonusIsCatched()
    {
        playerYDirection = VerticalMovingDirection.Up;
        if (firstDownMoving)
            firstDownMoving = false;
        ps_climbBlue.Play();
        ps_climbPurple.Play();
    }
    void ClimbIsCompleted()
    {
        playerYDirection = VerticalMovingDirection.Down;
        SetPlayerDownMovingSpeed();
    }
    void SetPlayerDownMovingSpeed()
    {
        //Try find Z coordinates of next Climb bonus
        GameObject[] allClimbBonuses = GameObject.FindGameObjectsWithTag("Climb Bonus");
        float deltaZ = 2;
        if (allClimbBonuses.Length > 0 && allClimbBonuses.Any(x => x.transform.position.z + deltaZ > transform.position.z))
        {
            if(firstDownMoving)
            {
                movingDownSpeed = GetMovingFirstDownSpeed(allClimbBonuses);
                nextClimbBonusIsFounded = true;
            }
            else
            {
                movingDownSpeed = GetMovingDownSpeed(allClimbBonuses);
                nextClimbBonusIsFounded = true;
            }
        }
        else
        {
            nextClimbBonusIsFounded = false;
            movingDownSpeed = 0;
        }
        
    }
    public float GetMovingFirstDownSpeed(GameObject[] allClimbBonuses)
    {
        float deltaZ = 2;
        float speed = 0;
        GameObject nearestClimbBonus = allClimbBonuses.First(x => x.transform.position.z == allClimbBonuses.Where(y => y.transform.position.z + deltaZ > transform.position.z).Min(z => z.transform.position.z));
        float distanceToNextClimbBonus = nearestClimbBonus.transform.position.z - transform.position.z;
        speed = ((transform.position.y * 0.80f) / GetMiddleTime(distanceToNextClimbBonus)) * dragDelta;
        return speed;
    }
    public float GetMovingDownSpeed(GameObject[] allClimbBonuses)
    {
        float deltaZ = 2;
        float speed = 0;
        GameObject nearestClimbBonus = allClimbBonuses.First(x => x.transform.position.z == allClimbBonuses.Where(y => y.transform.position.z + deltaZ > transform.position.z).Min(z => z.transform.position.z));
        float distanceToNextClimbBonus = nearestClimbBonus.transform.position.z - transform.position.z;
        speed = ((transform.position.y * 0.82f) / GetMiddleTime(distanceToNextClimbBonus)) * dragDelta;
        return speed;
    }
    private float GetMiddleTime(float distance)
    {
        float speed = 0;
        speed = distance / (strightForce / dragDelta);
        return speed;
    }
    Vector3 GetMovingZ()
    {
        Vector3 movingZ = Vector3.forward * strightForce;
        return movingZ;
    }

    IEnumerator Die()
    {
        isMoving = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        crushSound.Play();
        dieParticle.Play();
        yield return new WaitForSeconds(dieParticle.main.duration);
        //Messenger.Broadcast<int>(GameEvent.TryAddNewRecord, (int)transform.position.z);
        Managers.Records.CheckingForNewRecord((int)transform.position.z);
        Messenger.Broadcast(GameEvent.PlaneIsBroken);
        gameObject.SetActive(false);
    }
    public float GetMovingUpDuration()
    {
        float duration = 0;
        float trulyMovingUpSpeed = movingUpSpeed / (strightForce / playerRB.velocity.z);
        float heightWhenBonusIsPickedUp = 2.5f;
        duration = (maxYCoord - heightWhenBonusIsPickedUp) / trulyMovingUpSpeed;
        if (!isMoving)
            duration = 0.5f;
        return duration;
    }
    public float VelocityOnSegment(float a, float b)
    {
        float velocity = 0;
        velocity = playerRB.velocity.z;//Фейк
        if (!isMoving)
            velocity = 27f;
        return velocity;
    }
    private bool IsOutsideLevel()
    {
        bool iIsOutsideLevel = transform.position.x > 10.5f || transform.position.x < -10.5f ||
            transform.position.y > 20f || transform.position.y < -1f ||
            transform.position.z < -1f;
        return iIsOutsideLevel;
    }
    enum VerticalMovingDirection { Up, Down}
}
