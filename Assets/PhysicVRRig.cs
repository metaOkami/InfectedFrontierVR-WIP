using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicVRRig : MonoBehaviour
{
    public Transform playerHead;
    public Transform leftController;
    public Transform rightController;

    public ConfigurableJoint headJoint;
    public ConfigurableJoint leftHandJoint;
    public ConfigurableJoint rightHandJoint;

    public CapsuleCollider bodyCollider;

    public float bodyHeighMin = 0.5f;
    public float bodyHeighMax = 2f;

    
    private void FixedUpdate()
    {
        
        bodyCollider.height = Mathf.Clamp(playerHead.localPosition.y, bodyHeighMin, bodyHeighMax);
        bodyCollider.center = new Vector3(playerHead.localPosition.x, bodyCollider.center.y, playerHead.localPosition.z);

        leftHandJoint.targetPosition = leftController.localPosition;
        leftHandJoint.targetRotation = leftController.localRotation;

        rightHandJoint.targetPosition = rightController.localPosition;
        rightHandJoint.targetRotation = rightController.localRotation;

        headJoint.targetPosition = playerHead.localPosition;
    }
    
}
