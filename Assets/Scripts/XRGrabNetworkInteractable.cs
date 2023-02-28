using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;
public class XRGrabNetworkInteractable : XRGrabInteractable
{
    PhotonView photonView;
    private bool isTaked;

    private XRGrabNetworkInteractable GrabInteractableComponent;

    


    private void Start()
    {
        isTaked = false;
        photonView = GetComponent<PhotonView>();
        GrabInteractableComponent = GetComponent<XRGrabNetworkInteractable>();
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
        
        photonView.RPC(nameof(RPC_isTaked), RpcTarget.All);
        base.OnSelectEntered(args);
        
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        photonView.RPC(nameof(RPC_isNotTaked), RpcTarget.All);
        base.OnSelectExiting(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        photonView.RPC(nameof(RPC_isNotTaked), RpcTarget.All);
        base.OnSelectExiting(args);
    }

   
}
