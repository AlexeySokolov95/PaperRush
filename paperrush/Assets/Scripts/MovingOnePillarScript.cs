using System;
using UnityEngine;
using Assets.Class;
using DG.Tweening;

public class MovingOnePillarScript : LevelBlock
{

    public float movingDuration = 1;
    public float pillarWidth = 5;
    public float pillarLength = 5;
    public float blockLength = 50;
    public GameObject onePillar;
    void Start()
    {
        Initialization(blockLength);
        PutWall();
        onePillar = Instantiate(onePillar);
        onePillar.transform.localScale = new Vector3(pillarWidth, heightWall, pillarLength);
        float pillarZPosition = zCoordinateBeginningOfBlock + (blockLength / 2);
        float leftPosition = (-widthWall / 2) + (pillarWidth / 2);
        float rightPosition = widthWall / 2 - (pillarWidth / 2);
        Sequence pillarSequence = DOTween.Sequence();
        onePillar.transform.position = new Vector3(leftPosition, heightWall / 2, pillarZPosition);
        pillarSequence.Append(onePillar.transform.DOMoveX(rightPosition, movingDuration, false));
        pillarSequence.Append(onePillar.transform.DOMoveX(leftPosition, movingDuration, false));
        pillarSequence.SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
    void Update()
    {

    }
}
