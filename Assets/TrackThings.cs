using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrackThings : MonoBehaviour
{
    public TMP_Text textoRonda;
    
    public Wave_Manager waveManager;
    
    
    void Awake()
    {
        waveManager = GameObject.FindWithTag("WaveManager").GetComponent<Wave_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        textoRonda.text = waveManager.CurrentWave.ToString();
    }
}
