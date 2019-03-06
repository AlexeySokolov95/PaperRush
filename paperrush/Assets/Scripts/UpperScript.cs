using UnityEngine;
using System.Collections;
using Assets.Class;
using DG.Tweening;

public class UpperScript : MonoBehaviour
{
    public ParticleSystem mainParticle;
    private RBPlayerMoving player;
    private MeshRenderer meshRender;
    private CapsuleCollider collider;
    private Vector3 startCoord;
    private bool messageIsCalled = false;
    private AudioSource successSound;
    public Material columnMaterial;
    public float showDistance = 160;
    public float fadeDuration = 0.3f;
    private bool isShown = false;
    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<RBPlayerMoving>() ;
        meshRender = GetComponent<MeshRenderer>();
        collider = GetComponent<CapsuleCollider>();
        successSound = GetComponent<AudioSource>();
        columnMaterial = GetComponent<Renderer>().materials[0];
    }
    void Start()
    {
        Vector3 startCoord = transform.position;
        columnMaterial.color = new Color(columnMaterial.color.r, columnMaterial.color.g, columnMaterial.color.b, 0f);
    }

    void Update()
    {
        if (!messageIsCalled && startCoord != transform.position)
        {
            Messenger.Broadcast(GameEvent.ClimbBonusIsSet);
            messageIsCalled = true;
        }
        if (!isShown && transform.position.z - player.transform.position.z < showDistance)
        {

            isShown = true;
            columnMaterial.DOFade(0.7f, fadeDuration);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(BonusIsCatchedCoroutine());
    }
    private IEnumerator BonusIsCatchedCoroutine()
    {
        Messenger.Broadcast(GameEvent.ClimbBonusCatched);
        player.ClimbBonusIsCatched();
        successSound.Play();
        collider.enabled = false;
        meshRender.enabled = false;
        mainParticle.Play();
        yield return new WaitForSeconds(mainParticle.main.duration);
        Destroy(this);
    }
}
