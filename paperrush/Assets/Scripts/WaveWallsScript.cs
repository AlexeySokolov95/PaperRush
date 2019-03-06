using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaveWallsScript : MonoBehaviour
{
    public GameObject lineOfWall;
    public int numberOfLines = 12;
    public float additionalHeight = 0.2f;
    public float lengthOfLines = 100;
    public float heightOfTurn = 10;
    public float heightOfBeginninig = -2;
    float riseTime = 2;
    float turningTime = 0;
    float endTime = 0;
    public float movingDuration = 3;
    public float circelRadius = 5; //Around which the wall turns.
    public int numberOfSegments = 5;
    public float endingDistance = 3;
    private float wallWidth = 0;
    private float wallHeight = 0;
    private float currentWallScale = 200;
    private float lineHeight = 0;
    public Material mat1;
    public Material mat2;
    private LevelCreater LevelManager;
    private float timeOfRotateStart = 0;
    private float segmentDistance = 0;
    private float movingDelay = 0;
    private GameObject[] wallLines;
    private ColorSchemasManager colorManager;
    void Awake()
    {
        LevelManager = GameObject.Find("LevelManager").GetComponent<LevelCreater>();
        colorManager = GameObject.Find("ColorSchemasManager").GetComponent<ColorSchemasManager>();
        wallWidth = LevelManager.WidthWall;
        wallHeight = LevelManager.HeightWall;
    }
    void Start()
    {
        wallLines = new GameObject[numberOfLines * 2];
        FindSegmentDistance();
        lineHeight = HeightOfLines();// (heightOfTurn + Mathf.Abs(heightOfBeginninig)) / numberOfLines;
        lineOfWall.transform.localScale = new Vector3(1, lineHeight, currentWallScale);
        FindRotateDurations();
        movingDelay= movingDuration / numberOfLines;
        DOTween.defaultEaseType = Ease.Linear;
        PutWalls();
    }
    void Update()
    {
        if (LevelManager.player.transform.position.z + 300 > currentWallScale / 2)
        {
            //Rescale walls
            currentWallScale = currentWallScale + 200;
            foreach (var line in wallLines)
            {
                line.transform.localScale = new Vector3(line.transform.localScale.x, line.transform.localScale.y, currentWallScale);
            }
        }
    }
    private void PutWalls()
    {
        //Put on left side
        for (int i = 0; i < numberOfLines; i++)
        {
            GameObject line = Instantiate(lineOfWall);
            wallLines[i] = line;
            if (i % 2 != 0)
            {
                line.GetComponent<MeshRenderer>().material = colorManager.waveMat1;
                line.gameObject.tag = "WaveBlock1";
            }
            else
            {
                line.GetComponent<MeshRenderer>().material = colorManager.waveMat2;
                line.gameObject.tag = "WaveBlock2";
            }
            float posX = -wallWidth / 2 - (line.transform.localScale.x / 2);
            float posY = heightOfBeginninig;
            float posZ = currentWallScale / 2;
            line.transform.position = new Vector3(posX, posY, posZ);
            Sequence exteranlSequence = DOTween.Sequence();
            exteranlSequence.Append(line.transform.DOMove(line.transform.position, movingDelay * i));//pause
            int numberOfSegments = 5;
            Vector3[] path = new Vector3[numberOfSegments + 2];
            path[0] = new Vector3(posX, heightOfTurn, posZ);
            float centerOfCirclePosX = posX - circelRadius;
            for (int segment = 1; segment <= numberOfSegments; segment++)
            {
                float angle = (90 / numberOfSegments) * segment;
                float endPosX = centerOfCirclePosX + (circelRadius * Mathf.Cos(angle * Mathf.PI / 180));
                float endPosY = heightOfTurn + (circelRadius * Mathf.Sin(angle * Mathf.PI / 180));
                path[segment] = new Vector3(endPosX, endPosY, posZ);
            }
            float posXOfEndPart = posX - circelRadius - endingDistance;
            float posYOfEndPart = heightOfTurn + circelRadius;
            path[numberOfSegments + 1] = new Vector3(posXOfEndPart, heightOfTurn + circelRadius, posZ);
            Sequence movingSequence = DOTween.Sequence();
            movingSequence.Append(line.transform.DOPath(path, movingDuration));
            movingSequence.Insert(timeOfRotateStart, line.transform.DORotate(new Vector3(0, 0, 90), turningTime));
            movingSequence.SetLoops(10000, LoopType.Restart);
            exteranlSequence.Append(movingSequence);
        }
        //Put on right side
        for (int i = 0; i < numberOfLines; i++)
        {
            GameObject line = Instantiate(lineOfWall);
            wallLines[numberOfLines + i] = line;
            if (i % 2 != 0)
            {
                line.GetComponent<MeshRenderer>().material = colorManager.waveMat1;
                line.gameObject.tag = "WaveBlock1";
            }
            else
            {
                line.GetComponent<MeshRenderer>().material = colorManager.waveMat2;
                line.gameObject.tag = "WaveBlock2";
            }
            float posX = wallWidth / 2 + (line.transform.localScale.x / 2);
            float posY = heightOfBeginninig;
            float posZ = currentWallScale / 2;
            line.transform.position = new Vector3(posX, posY, posZ);
            Sequence exteranlSequence = DOTween.Sequence();
            exteranlSequence.Append(line.transform.DOMove(line.transform.position, movingDelay * i)).SetEase(Ease.Linear); ;//pause
            int numberOfSegments = 5;
            Vector3[] path = new Vector3[numberOfSegments + 2];
            path[0] = new Vector3(posX, heightOfTurn, posZ);
            float centerOfCirclePosX = posX + circelRadius;
            for (int segment = 1; segment <= numberOfSegments; segment++)
            {
                float angle = (90 / numberOfSegments) * segment;
                float endPosX = centerOfCirclePosX - (circelRadius * Mathf.Cos(angle * Mathf.PI / 180));
                float endPosY = heightOfTurn + (circelRadius * Mathf.Sin(angle * Mathf.PI / 180));
                path[segment] = new Vector3(endPosX, endPosY, posZ);
            }
            float posXOfEndPart = posX + circelRadius + endingDistance;
            float posYOfEndPart = heightOfTurn + circelRadius;
            path[numberOfSegments + 1] = new Vector3(posXOfEndPart, heightOfTurn + circelRadius, posZ);
            Sequence movingSequence = DOTween.Sequence();
            movingSequence.Append(line.transform.DOPath(path, movingDuration)).SetEase(Ease.Linear); ;
            movingSequence.Insert(timeOfRotateStart, line.transform.DORotate(new Vector3(0, 0, -90), turningTime)).SetEase(Ease.Linear); ;
            movingSequence.SetLoops(10000, LoopType.Restart);
            exteranlSequence.Append(movingSequence).SetEase(Ease.Linear);
        }
        currentWallScale = currentWallScale + lengthOfLines;
    }
    private void FindRotateDurations()
    {
        /*float fullDuration = 0;
        float angle = (90 / numberOfSegments);
        float startX = circelRadius;
        float startY = 0;
        float endX = circelRadius * (Mathf.Cos(angle * Mathf.PI / 180));
        float endY = circelRadius * (Mathf.Sin(angle * Mathf.PI / 180));
        float segmentDistance = Vector2.Distance(new Vector2(startX, startY), new Vector2(endX, endY));
        float fullLengthOfTopPart = (segmentDistance * numberOfSegments) + endingDistance;
        float lengthOfFirstPart = heightOfTurn + Mathf.Abs(heightOfBeginninig);
        fullDuration = riseTime * (fullLengthOfTopPart / lengthOfFirstPart);
        turningTime = fullDuration * ((segmentDistance * numberOfSegments) / fullLengthOfTopPart);
        endTime = fullDuration * (endingDistance / fullLengthOfTopPart);*/
        float fullLengthOfTopPart = (segmentDistance * numberOfSegments) + endingDistance;
        float lengthOfBottomPart = heightOfTurn + Mathf.Abs(heightOfBeginninig);
        float fullLength = fullLengthOfTopPart + lengthOfBottomPart;
        timeOfRotateStart = movingDuration * (lengthOfBottomPart / fullLength);
        turningTime = movingDuration * ((segmentDistance * numberOfSegments) / fullLength);

    }
    float HeightOfLines()
    {
        float height= 0;
        float fullLengthOfTopPart = (segmentDistance * numberOfSegments) + endingDistance;
        float quarterCircleLength = 2 * Mathf.PI * circelRadius / 4;
        float lengthOfBottomPart = heightOfTurn + Mathf.Abs(heightOfBeginninig);
        float fullLength = fullLengthOfTopPart + lengthOfBottomPart;
        height = fullLength / numberOfLines;
        return height + additionalHeight;
    }
    void FindSegmentDistance()
    {
        float angle = (90 / numberOfSegments);
        float startX = circelRadius;
        float startY = 0;
        float endX = circelRadius * (Mathf.Cos(angle * Mathf.PI / 180));
        float endY = circelRadius * (Mathf.Sin(angle * Mathf.PI / 180));
        segmentDistance = Vector2.Distance(new Vector2(startX, startY), new Vector2(endX, endY));
    }
    
}
