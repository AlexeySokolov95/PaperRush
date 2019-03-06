using System;
using UnityEngine;
using Assets.Class;
using DG.Tweening;

public class SeveralMovingPillarScript : LevelBlock
{
    public int numberPillars = 3;
    public float movingDuration = 1;
    public float pillarWidth = 5;
    public float pillarLength = 5;
    public float distanceBeetwenBlock = 3;
    public float blockLength = 50;
    private bool isStarted = false;
    private GameObject[] pillars;
    public GameObject onePillar;
    void Start()
    {
        Initialization(blockLength);
        PutWall();
        pillars = new GameObject[numberPillars];
        GameObject pillar = Instantiate(onePillar);
        pillar.transform.localScale = new Vector3(pillarWidth, heightWall, pillarLength);
        float startZPosition = zCoordinateBeginningOfBlock + (blockLength / 2) - ((numberPillars/2) * (pillarLength + distanceBeetwenBlock));
        for (int i = 0; i < numberPillars; i++)
        {
            GameObject newPillar = Instantiate(pillar);
            float positionX = (-widthWall/2) + (3 * i);
            float positionZ = startZPosition + (i * (pillarLength + distanceBeetwenBlock));
            newPillar.transform.position = new Vector3(positionX, heightWall/2, positionZ);
            pillars[i] = newPillar;
        }
        pillar.SetActive(false);
    }
	
	void Update ()
    {
		if(!isStarted && zCoordinateBeginningOfBlock - LevelManager.player.transform.position.z <= 150)
        {
            isStarted = true;
            float leftPosition = (-widthWall / 2) + (pillarWidth / 2);
            float rightPosition = widthWall / 2 - (pillarWidth / 2);
            for (int i = 0; i < numberPillars; i++)
            {
                Sequence pillarSequence = DOTween.Sequence();
                pillarSequence.Append(pillars[i].transform.DOMoveX(leftPosition, movingDuration, false));
                pillarSequence.Append(pillars[i].transform.DOMoveX(rightPosition, movingDuration, false));              
                pillarSequence.SetLoops(50, LoopType.Restart).SetEase(Ease.Linear);
                Sequence externalSequence = DOTween.Sequence();
                float firstDuration = movingDuration - (i * 0.2f);
                externalSequence.Append(pillars[i].transform.DOMoveX(rightPosition, firstDuration, false));
                externalSequence.Append(pillarSequence);
            }
        }
	}
}
