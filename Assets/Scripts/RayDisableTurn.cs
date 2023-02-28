using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RayDisableTurn : MonoBehaviour
{
    public ActionBasedSnapTurnProvider turnComponent;

    public XRRayInteractor leftGrabRay;
    public XRRayInteractor rightGrabRay;

    public bool turnEnabled = true;

    // Update is called once per frame
    void Update()
    {
        turnComponent.enabled = turnEnabled;

        if(leftGrabRay.interactablesSelected.Count==0 && rightGrabRay.interactablesSelected.Count == 0)
        {
            turnEnabled = true;
        }
        else
        {
            turnEnabled = false;
        }
    }
}
