using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class Climber : MonoBehaviour
{
    private CharacterController controller;
    public static ActionBasedController climbingHand;
    private ActionBasedContinuousMoveProvider contMove;

    public InputActionProperty velocityLeft;
    public InputActionProperty velocityRight;

    public bool movimientoContinuoOn;

    public static bool leftClimb;
    public static bool rightClimb;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        contMove = GetComponent<ActionBasedContinuousMoveProvider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (climbingHand)
        {
            Climb();
            if (movimientoContinuoOn)
            {
                contMove.enabled = false;
            }
        }
        else
        {
            if (movimientoContinuoOn)
            {
                contMove.enabled = true;
            }
        }
    }

    public void Climb()
    {
        if (leftClimb)
        {
            Vector3 velocity = velocityLeft.action.ReadValue<Vector3>();
            controller.Move(transform.rotation * -velocity * Time.fixedDeltaTime);
        }

        if (rightClimb)
        {
            Vector3 velocity = velocityRight.action.ReadValue<Vector3>();
            controller.Move(transform.rotation * -velocity * Time.fixedDeltaTime);
        }
    }
}
