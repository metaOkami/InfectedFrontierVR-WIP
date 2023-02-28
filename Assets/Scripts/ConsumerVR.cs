using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumerVR : MonoBehaviour
{
    public AudioSource auSource;

    private void Start()
    {
        auSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Consumable consumable=other.GetComponent<Consumable>();
        if (consumable != null && !consumable.isFinished)
        {
            consumable.Consume();
            auSource.Play();
        }
    }
}
