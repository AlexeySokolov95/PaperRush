using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Class;
public class ChessSpikeScript : LevelBlock
{
    public float distanceBetweenRow = 15;
    public int numberOfSpikeOnEvenRow = 5;
    public int numberOfSpikeOnOddRow = 5;
    public float newLength = 100;
    GameObject chessSpikeOnEvenRow;
    GameObject chessSpikeOnOddRow;
    float newSpikeScaleXOnOddRow;
    float newSpikeScaleXOnEvenRow;
    // Use this for initialization
    void Start()
    {
        Initialization(newLength);
        PutWall();
        chessSpikeOnEvenRow = Resources.Load("pref_ChessSpike", typeof(GameObject)) as GameObject;
        chessSpikeOnOddRow = Resources.Load("pref_ChessSpike", typeof(GameObject)) as GameObject;
        PutSpike();
    }
    private void PutScaleForSpike()
    {
        Vector3 oldScale = chessSpikeOnEvenRow.transform.localScale;
        newSpikeScaleXOnEvenRow = widthWall / (numberOfSpikeOnEvenRow * 2);
        chessSpikeOnEvenRow.transform.localScale = new Vector3(newSpikeScaleXOnEvenRow, heightWall, oldScale.z);
        newSpikeScaleXOnOddRow = widthWall / (numberOfSpikeOnOddRow * 2);
        chessSpikeOnEvenRow.transform.localScale = new Vector3(newSpikeScaleXOnOddRow, heightWall, oldScale.z);
    }
   void doSmth() { }
    private void PutSpike()
    {
        PutScaleForSpike();
        float positionY = heightWall / 2;
        bool evenRow = false;
        int numberSpikeAndCrackOnEvenRow = numberOfSpikeOnEvenRow * 2;
        int numberSpikeAndCrackOnOddRow = numberOfSpikeOnOddRow * 2;
        float positionZNewRow = 0;
        while (positionZNewRow < lengthOfMainWall)
        {
            if(evenRow)
            {
                for( int i = 0; i < numberSpikeAndCrackOnEvenRow; i+=2 )
                {
                    GameObject chessSpikeOnRow = Instantiate(chessSpikeOnEvenRow) ;
                    float positionX = -(widthWall / 2) + (i * newSpikeScaleXOnEvenRow + (newSpikeScaleXOnEvenRow / 2));
                    chessSpikeOnRow.transform.position = new Vector3(positionX, positionY, zCoordinateBeginningOfBlock + positionZNewRow);
                    if (positionZNewRow > 70)
                        doSmth();
                }
                evenRow = false;
            }
            else
            {
                for (int i = 1; i < numberSpikeAndCrackOnOddRow; i +=2)
                {
                    GameObject chessSpikeOnRow = Instantiate(chessSpikeOnOddRow);
                    float positionX = -(widthWall / 2) + (i * newSpikeScaleXOnOddRow + (newSpikeScaleXOnOddRow / 2));
                    chessSpikeOnRow.transform.position = new Vector3(positionX, positionY, zCoordinateBeginningOfBlock + positionZNewRow);
                }
                evenRow = true;
            }
            positionZNewRow += distanceBetweenRow;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
