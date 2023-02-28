using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandPoseController : MonoBehaviour
{
    public GameObject RightHand;
    public Animator RightAnimator;
    public GameObject LeftHand;
    public Animator LeftAnimator;

    public bool isRight;
    public bool isDirectGrab;
    public bool Agarrao;

    public InputActionProperty rightTrigger;
    public InputActionProperty leftTrigger;

    // Start is called before the first frame update
    void Start()
    {
        RightHand.SetActive(false);
        LeftHand.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Agarrao)
        {
            if (RightHand.activeSelf)
            {
                float RightTriggerValue = rightTrigger.action.ReadValue<float>();
                RightAnimator.SetFloat("Trigger", RightTriggerValue);
            }

            if (LeftHand.activeSelf)
            {
                float LeftTriggerValue = leftTrigger.action.ReadValue<float>();
                LeftAnimator.SetFloat("Trigger", LeftTriggerValue);
            }
        }
    }

    public void ActivateHand()
    {
        isRight = GetComponent<XRGrabInteractableTwoHands>().isRightHand;
        isDirectGrab = GetComponent<XRGrabInteractableTwoHands>().isDirectGrab;

        if (isDirectGrab)
        {
            if (isRight)
            {
                RightHand.SetActive(true);
                LeftHand.SetActive(false);
            }
            else
            {
                RightHand.SetActive(false);
                LeftHand.SetActive(true);
            }
        }
    }

    public void DeactivateHand()
    {
        RightHand.SetActive(false);
        LeftHand.SetActive(false);
    }

    public void AgarreOn()
    {
        Agarrao = true;
    }

    public void AgarreOff()
    {
        Agarrao = false;
    }
}
