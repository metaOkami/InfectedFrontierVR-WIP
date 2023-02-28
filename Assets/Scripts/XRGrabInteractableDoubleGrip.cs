using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class XRGrabInteractableDoubleGrip : XRGrabInteractable
{
    public GameObject rightHandModel;
    public GameObject leftHandModel;

    public GameObject rightPoseHandModel, leftPoseHandModel;

    public Transform leftAttach;
    public Transform rightAttach;

    public bool isRightHand;

    public InteractionLayerMask directGrabLayer;
    public bool DirectGrab;
    public BoxCollider colliderGrip;

    [SerializeField]
    private Transform _GripAttach;

    private PhotonView photonView;
    private bool isTaked;

    public Camera scopeCam;


    protected override void Awake()
    {
        isTaked = false;
        photonView = GetComponent<PhotonView>();
        scopeCam.enabled = false;
        rightHandModel = GameObject.Find("Right Hand Presence");
        leftHandModel = GameObject.Find("Left Hand Presence");
        rightPoseHandModel.SetActive(false);
        leftPoseHandModel.SetActive(false);
        base.Awake();
        selectMode = InteractableSelectMode.Multiple;
    }

   
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (interactorsSelecting.Count == 1)
        {
            
            scopeCam.enabled = true;
            if (interactorsSelecting[0].transform.tag=="RightHand")
            {
                rightHandModel.SetActive(false);
                leftHandModel.SetActive(true);
                rightPoseHandModel.SetActive(true);
                leftPoseHandModel.SetActive(false);
            }
            else if(interactorsSelecting[0].transform.tag == "LeftHand")
            {
                rightHandModel.SetActive(true);
                leftHandModel.SetActive(false);
                rightPoseHandModel.SetActive(false);
                leftPoseHandModel.SetActive(true);
            }
            base.ProcessInteractable(updatePhase);
            colliderGrip.enabled = true;
        }
        else if (interactorsSelecting.Count == 2 &&
            updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            scopeCam.enabled = true;
            rightHandModel.SetActive(false);
            leftHandModel.SetActive(false);
            rightPoseHandModel.SetActive(true);
            leftPoseHandModel.SetActive(true);
            ProcessDoubleGrip();
        }
        else if (interactorsSelecting.Count == 0)
        {
            scopeCam.enabled = false;
            leftPoseHandModel.SetActive(false);
            rightPoseHandModel.SetActive(false);
            rightHandModel.SetActive(true);
            leftHandModel.SetActive(true);
            colliderGrip.enabled = true;
        }

    }

    private void ProcessDoubleGrip()
    {
        Transform firstAttach = GetAttachTransform(null);
        Transform firstHand = interactorsSelecting[0].transform;
        Transform secondAttach = _GripAttach;
        Transform secondHand = interactorsSelecting[1].transform;

        Vector3 directionBetweenHands = secondHand.position - firstHand.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionBetweenHands, firstHand.transform.up);

        Vector3 worldDirectionFromHandletoBase = transform.position - firstAttach.position;
        Vector3 localDirectionFromHandletoBase = transform.InverseTransformDirection(worldDirectionFromHandletoBase);

        Vector3 targetPosition = firstHand.position + localDirectionFromHandletoBase;
        transform.SetPositionAndRotation(targetPosition, targetRotation);
    }

    [PunRPC]
    void RPC_isTakedDGrip()
    {

        isTaked = true;
    }

    [PunRPC]
    void RPC_isNotTakedDGrip()
    {

        isTaked = false;
    }

    protected override void Grab()
    {
        if (interactorsSelecting.Count == 1)
        {
            base.Grab();
        }
    }

    protected override void Drop()
    {
        if (!isSelected)
        {
            base.Drop();
        }
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        if (interactorsSelecting[0] == args.interactorObject)
        {
            base.OnActivated(args);
        }
    }

    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        if (interactorsSelecting[0] == args.interactorObject)
        {
            base.OnDeactivated(args);
        }
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (!isTaked)
        {
            photonView.RequestOwnership();
        }
        base.OnHoverEntered(args);
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        photonView.RPC(nameof(RPC_isTakedDGrip), RpcTarget.All);

        if (args.interactorObject.transform.CompareTag("LeftHand"))
        {
            rightHandModel.SetActive(true);
            leftHandModel.SetActive(false);
            rightPoseHandModel.SetActive(false);
            leftPoseHandModel.SetActive(true);
            attachTransform = leftAttach;
            isRightHand = false;


        }

        if (args.interactorObject.interactionLayers == directGrabLayer)
        {
            DirectGrab = true;
        }
        else
        {
            DirectGrab = false;
        }

        if (args.interactorObject.transform.CompareTag("RightHand"))
        {
            rightHandModel.SetActive(false);
            leftHandModel.SetActive(true);
            rightPoseHandModel.SetActive(true);
            leftPoseHandModel.SetActive(false);
            attachTransform = rightAttach;
            isRightHand = true;
        }
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        photonView.RPC(nameof(RPC_isNotTakedDGrip), RpcTarget.All);
        base.OnSelectExited(args);
    }
}