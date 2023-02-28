using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClimbInteractable : XRBaseInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if(args.interactorObject is XRDirectInteractor)
        {
            Climber.climbingHand = args.interactorObject.transform.GetComponent<ActionBasedController>();
            if (args.interactorObject.transform.CompareTag("LeftHand"))
            {
                Climber.leftClimb = true;
                Climber.rightClimb = false;
            }
            if (args.interactorObject.transform.CompareTag("RightHand"))
            {
                Climber.leftClimb = false;
                Climber.rightClimb = true;
            }

        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (args.interactorObject is XRDirectInteractor)
        {
            if(Climber.climbingHand && Climber.climbingHand.name == args.interactorObject.transform.name)
            {
                Climber.climbingHand = null;
                Climber.leftClimb = false;
                Climber.rightClimb = false;
            }
        }
    }
}
