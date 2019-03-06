using UnityEngine;
using Assets.Class;
using DG.Tweening;

public class ClosingWallInCascadeScript : MonoBehaviour
{
    public float distanceOfOpenWalls = 15;
    public float xPosEndOfClosing = 4;
    public float closingDuration = 0.4f;
    private Side wallSide;
    private LevelCreater LevelManager;
    bool isOpen = false;
    void Start()
    {
        LevelManager = GameObject.FindWithTag("Level Creater").GetComponent<LevelCreater>();
        wallSide = Side.Left;
        if (transform.localEulerAngles.y != 0)
            wallSide = Side.Right;
    }
    void Update()
    {
        if (!isOpen && transform.position.z - distanceOfOpenWalls <= LevelManager.player.transform.position.z)
        {
            if (wallSide == Side.Left)
                transform.DOMoveX(-transform.localScale.x + xPosEndOfClosing, closingDuration);
            else if (wallSide == Side.Right)
                transform.DOMoveX(transform.localScale.x - xPosEndOfClosing, closingDuration);
            isOpen = true;
        }
    }
}
