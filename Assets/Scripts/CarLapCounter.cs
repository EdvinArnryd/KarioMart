using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
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

    public String name;
    
    public GameObject winnerPanel;
    private void OnTriggerEnter(Collider colliderPoint)
    {

        if (isRaceCompleted)
        {
            winnerPanel.SetActive(true);
            Transform childTransform = winnerPanel.transform.GetChild(0);

            TextMeshProUGUI textMeshPro = childTransform.GetComponent<TextMeshProUGUI>();

            textMeshPro.text = "You win " + name + "!";
            
            Time.timeScale = 0;
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

    private void OnWin(TextMeshProUGUI panel)
    {
        panel.text = "Player " + name + " has won!";
    }
}
