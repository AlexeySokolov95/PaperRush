using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;

public class BonusCrystalScript : MonoBehaviour
{
    public ParticleSystem mainParticle;
    private LevelCreater levelManager;
    private GameObject player;
    private float widthWall;
    private float heightWall;
    private float startPosZ = 0;
    private MeshRenderer meshRender;
    private CapsuleCollider collider;
    private RBPlayerMoving RBP;
    private AudioSource crushSound;
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        meshRender = GetComponent<MeshRenderer>();
        collider = GetComponent<CapsuleCollider>();
        RBP = player.GetComponent<RBPlayerMoving>();
        Messenger.AddListener(GameEvent.ClimbBonusIsSet, SetYCoordinate);
        crushSound = GetComponent<AudioSource>();
    }
    void Start()
    {

    }
    void Update()
    {
        //if (player.transform.position.z + 200 > transform.position.z)
        //transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        if (transform.position.y < 0 && !player.GetComponent<RBPlayerMoving>().isMoving)
            SetYCoordinate();
    }

    private IEnumerator BonusIsCatchedCoroutine()
    {
        mainParticle.Play();
        Messenger.Broadcast(GameEvent.CrystalBonusCatched);
        collider.enabled = false;
        meshRender.enabled = false;
        crushSound.Play();
        yield return new WaitForSeconds(mainParticle.main.duration);
        Destroy(this);
    }
    private void BonusIsCatched()
    {
        StartCoroutine(BonusIsCatchedCoroutine());
    }
    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(BonusIsCatchedCoroutine());
    }
    public void SetYCoordinate()
    {
        if (this != null)// Баг связанный с тем что обьект уже уничтожен.
        {
            bool onLevel = transform.position.y > 0;
            if (!onLevel)
            {
                GameObject player = GameObject.FindWithTag("Player");
                // Находим все Клаймбы
                GameObject[] allClimbBonuses = GameObject.FindGameObjectsWithTag("Climb Bonus");
                float yCoord = -5;
                if (allClimbBonuses.Length > 0)
                {
                    if (transform.position.z < allClimbBonuses.Max(x => x.transform.position.z))
                    {
                        GameObject backBonus;
                        if (allClimbBonuses.Any(x => x.transform.position.z < transform.position.z))
                            backBonus = allClimbBonuses.First(x => x.transform.position.z == allClimbBonuses.Where(y => y.transform.position.z < transform.position.z).Max(z => z.transform.position.z));
                        else
                        {
                            backBonus = new GameObject();
                            float movingUpZDistance = (RBP.GetMovingUpDuration() * RBP.VelocityOnSegment(0, 15));
                            if (float.IsNaN(movingUpZDistance))
                                backBonus.transform.position = new Vector3(0, 0, -10);
                            else
                                backBonus.transform.position = new Vector3(0, 0, 0 - movingUpZDistance);
                        }
                        GameObject frontBonus = allClimbBonuses.First(x => x.transform.position.z == allClimbBonuses.Where(y => y.transform.position.z > transform.position.z).Min(z => z.transform.position.z));
                        float segmentDistance = frontBonus.transform.position.z - backBonus.transform.position.z;
                        float speedBeetwenClimbs = RBP.VelocityOnSegment(backBonus.transform.position.z, frontBonus.transform.position.z);
                        float movingUpDuration = RBP.GetMovingUpDuration();
                        float segmentDuration = segmentDistance / speedBeetwenClimbs;
                        float minCrystalYCoord = 2.5f;
                        //If on moving up stage
                        if (((transform.position.z - backBonus.transform.position.z) / segmentDistance) < (movingUpDuration / segmentDuration))
                        {
                            float crystalPositionOnUpStage = transform.position.z - backBonus.transform.position.z;
                            float upStageDistance = movingUpDuration * speedBeetwenClimbs;
                            float durationOnUpStage = crystalPositionOnUpStage / upStageDistance * (movingUpDuration / movingUpDuration);
                            yCoord = minCrystalYCoord + (crystalPositionOnUpStage / upStageDistance * (RBP.maxYCoord - minCrystalYCoord));
                            /// yCoord = minCrystalYCoord + ((transform.position. z - backBonus.transform.position.z * speedBeetwenClimbs) / movingUpDuration * (RBP.maxYCoord - minCrystalYCoord));
                        }
                        else
                        {
                            float allDurationOnDownStage = segmentDuration - movingUpDuration;
                            float downStageDistance = segmentDistance - (movingUpDuration * speedBeetwenClimbs);
                            float crystalPositionOnDownStage = transform.position.z - backBonus.transform.position.z - (movingUpDuration * speedBeetwenClimbs);
                            float durationOnDownStage = transform.position.z - backBonus.transform.position.z - (movingUpDuration * speedBeetwenClimbs) / speedBeetwenClimbs;

                            durationOnDownStage = crystalPositionOnDownStage / downStageDistance * allDurationOnDownStage;
                            yCoord = RBP.maxYCoord - (durationOnDownStage / allDurationOnDownStage * (RBP.maxYCoord - minCrystalYCoord));
                        }
                    }
                }
                if (float.IsNaN(yCoord))
                    yCoord = -10;
                transform.position = new Vector3(transform.position.x, yCoord, transform.position.z);
            }
        }

        /* Если кристал между клаймбами
         *   находим между какими клаймбами и расстояние между ними
         *   определяем стадию(подьём или спуск)
         *   на стадии определяем координату(не слишком низко)
         * иначе
         *   координата под уровнеми состояние спрятана устанавливаем слушителя
        */
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "LevelObstacle")
        {
            Destroy(this);
        }
    }
}
