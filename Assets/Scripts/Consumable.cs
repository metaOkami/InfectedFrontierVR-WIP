using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    [SerializeField] GameObject[] portions;
    [SerializeField] int index = 0;

    public bool readyToConsume = false;
    public bool isFinished => index == portions.Length;
    public AudioSource auSource;

    private void Start()
    {
        auSource = GetComponent<AudioSource>();
        ConsumeGeo();
        Ready();
    }

    public void Consume()
    {
        if(!isFinished && readyToConsume)
        {
            index++;
            ConsumeGeo();
            auSource.Play();
        }
    }
    void ConsumeGeo()
    {
        for (int i = 0; i < portions.Length; i++)
        {
            portions[i].SetActive(i == index);
        }
    }

    public void Ready()
    {
        readyToConsume = !readyToConsume;
    }
}
