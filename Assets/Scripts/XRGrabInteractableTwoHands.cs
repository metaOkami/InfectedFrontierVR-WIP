using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;

public class XRGrabInteractableTwoHands : XRGrabInteractable
{
    public Transform leftAttach;
    public Transform rightAttach;

    public InteractionLayerMask directGrabLayer;
    public bool isDirectGrab;
    public bool isRightHand;

    Rigidbody PistolRb;

    PhotonView photonView;
    private bool isTaked;
    //private GameObject localRHandModel, localLHandModel;
    private XRGrabInteractableTwoHands GrabInteractableComponent;

    

    
    private void Start()
    {
        
        isTaked = false;
        photonView = GetComponent<PhotonView>();
        GrabInteractableComponent = GetComponent<XRGrabInteractableTwoHands>();
        
    }

    

    private void Update()
    {
       
        
        if (!photonView.IsMine)
        {
            if (isTaked)
            {
                GrabInteractableComponent.OnDisable();
            }
            else
            {
                GrabInteractableComponent.OnEnable();
            }
        }
       
    }
    //[PunRPC]
    //void RPC_OnDisparentGrabbable(int objectID)
    //{
        
    //    PhotonView childView = PhotonView.Find(objectID);

    //    childView.gameObject.transform.SetParent(null);
    //}
    //[PunRPC]
    //void RPC_OnParentGrabbable(int parentId, int childId)
    //{
    //    PhotonView parentView = PhotonView.Find(parentId);
    //    PhotonView childView = PhotonView.Find(childId);

    //    childView.gameObject.transform.SetParent(parentView.gameObject.transform);
    //}

    [PunRPC]
    void RPC_isTaked()
    {
        
        isTaked = true;
    }

    [PunRPC]
    void RPC_isNotTaked()
    {
        
        isTaked = false;
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
        //PhotonView currentHandView;

        photonView.RPC(nameof(RPC_isTaked), RpcTarget.All);
        if (args.interactorObject.transform.CompareTag("LeftHand"))
        {
            //currentHandView = localLHandModel.GetPhotonView();
            //photonView.RPC(nameof(RPC_OnParentGrabbable), RpcTarget.All, currentHandView.ViewID, photonView.ViewID);
            //rbView.enabled = false;
            attachTransform = leftAttach;
            isRightHand = false;
        }

        if (args.interactorObject.transform.CompareTag("RightHand"))
        {
            //currentHandView = localRHandModel.GetPhotonView();
            //photonView.RPC(nameof(RPC_OnParentGrabbable), RpcTarget.All, currentHandView.ViewID, photonView.ViewID);
            //rbView.enabled = false;

            attachTransform = rightAttach;
            isRightHand = true;
        }

        if(args.interactorObject.interactionLayers== directGrabLayer)
        {
            isDirectGrab = true;
        }
        else
        {
            isDirectGrab = false;
        }
        
        

        base.OnSelectEntered(args);
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        //rbView.enabled = true;

        //photonView.RPC(nameof(RPC_OnDisparentGrabbable), RpcTarget.All,photonView.ViewID);
        photonView.RPC(nameof(RPC_isNotTaked), RpcTarget.All);
        base.OnSelectExiting(args);
    }
}
