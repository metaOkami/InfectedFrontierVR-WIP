using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class SecondaryAction : MonoBehaviour
{
    //Esto son los inputs de los mandos
    public InputActionProperty secondaryLeftHand;
    public InputActionProperty secondaryRightHand;

    //Este es el evento que queremos ejecutar con la acción secundaria
    public UnityEvent secondaryActionEvent;

    //Estas son las booleanas para crear condiciones
    public bool isRightHand;
    public bool secondaryAction;
    public bool isGrab;

    //El animator para girar la linterna
    public Animator flashAnim;


    // Update is called once per frame
    void Update()
    {
        isRightHand = GetComponent<XRGrabInteractableTwoHands>().isRightHand;

        if(secondaryLeftHand.action.WasPressedThisFrame()&& isGrab && !isRightHand)
        {
            secondaryAction = !secondaryAction;
            secondaryActionEvent.Invoke();
        }

        if (secondaryRightHand.action.WasPressedThisFrame() && isGrab && isRightHand)
        {
            secondaryAction = !secondaryAction;
            secondaryActionEvent.Invoke();
        }
    }

    public void isGrabOn()
    {
        isGrab = true;
    }

    public void isGrabOff()
    {
        isGrab = false;
    }

    public void AnimatorAction()
    {
        flashAnim.SetBool("Secondary", secondaryAction);
    }

}
