using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
public class OnDisableStartButton : MonoBehaviourPun
{
    public GameObject Button;
    public Material disabledMat;
    public XRGrabInteractable GrabInteractable;
    public GameObject waveManager;

    
    public void DisableStartButton()
    {
        photonView.RPC(nameof(RPC_DisableStartButton), RpcTarget.All);
    }
    
    private void OnDisable()
    {
        Button.GetComponent<MeshRenderer>().material = disabledMat;
        GrabInteractable.enabled = false;
        waveManager.SetActive(true);
    }

    [PunRPC]
    public void RPC_DisableStartButton()
    {
        OnDisable();
    }
}
