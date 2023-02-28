using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Respawn : MonoBehaviour
{
    public float timeRemaining = 10;
    private float time2;
    public bool timerIsRunning = false;
    public TMP_Text timeText;
    public GameObject canvasMuerte;
    //public GameObject canvasPrincipal;
    private Respawn respawnScript;
    
    void Start()
    {
        timerIsRunning = true;
        canvasMuerte.SetActive(true);
        //canvasPrincipal.SetActive(false);
        respawnScript = GetComponent<Respawn>();
        time2 = timeRemaining;

    }

    
    void Update()
    {
        canvasMuerte.SetActive(true);
        //canvasPrincipal.SetActive(false);
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                RespawnPlayer();
                
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void RespawnPlayer()
    {
        canvasMuerte.SetActive(false);
        //canvasPrincipal.SetActive(true);
        timeRemaining = time2;
        timerIsRunning = true;
        respawnScript.enabled = false;
    }
}
