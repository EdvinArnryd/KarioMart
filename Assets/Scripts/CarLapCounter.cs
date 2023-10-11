using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLapCounter : MonoBehaviour
{
    private int passedCheckPointNumber = 0;
    private float timeAtLastPassedCheckPoint = 0;

    private int lapsCompleted = 0;
    private const int lapsToComplete = 2;

    private bool isRaceCompleted = false;

    private int numberOfPassedCheckPoints = 0;

    public event Action<CarLapCounter> OnPassCheckPoint;

    private void OnTriggerEnter(Collider colliderPoint)
    {

        if (isRaceCompleted)
        {
            Debug.Log("The game is over you won!");
            return;
        }
        
        if (colliderPoint.CompareTag("Checkpoint"))
        {
            var checkPoint = colliderPoint.GetComponent<CheckPoint>();

            if (passedCheckPointNumber + 1 == checkPoint.checkPointNumber)
            {
                passedCheckPointNumber = checkPoint.checkPointNumber;

                numberOfPassedCheckPoints++;

                timeAtLastPassedCheckPoint = Time.time;

                if (checkPoint.isFinishLine)
                {
                    passedCheckPointNumber = 0;
                    lapsCompleted++;

                    if (lapsCompleted >= lapsToComplete)
                    {
                        isRaceCompleted = true;
                    }
                }
                
                OnPassCheckPoint?.Invoke(this);
            }
        }
    }
}
