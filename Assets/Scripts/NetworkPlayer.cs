using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using TMPro;
public class NetworkPlayer : MonoBehaviourPun
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Animator rightHandAnimator;
    public Animator leftHandAnimator;

    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;


    

    
    private void Start()
    {
        
        XROrigin rig = FindObjectOfType<XROrigin>();
        headRig=rig.transform.Find("Camera Offset/Main Camera");
        leftHandRig = rig.transform.Find("Camera Offset/LeftHand Controller");
        rightHandRig = rig.transform.Find("Camera Offset/RightHand Controller");

        if (photonView.IsMine)
        {
            
            this.gameObject.tag = "LocalPlayer";
            head.gameObject.tag = "LocalPlayer";
            leftHand.gameObject.tag = "LocalLHandPlayer";
            rightHand.gameObject.tag = "LocalRHandPlayer";
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }
        else
        {
            this.gameObject.tag = "NetPlayer";
            head.gameObject.tag = "NetPlayer";
            leftHand.gameObject.tag = "NetLHandPlayer";
            rightHand.gameObject.tag = "NetRHandPlayer";
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            

            MapPosition(head, headRig);
            MapPosition(leftHand, leftHandRig);
            MapPosition(rightHand, rightHandRig);

            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAnimator);
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), rightHandAnimator);

        }
    }

    void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }
    void MapPosition(Transform target,Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }

}
