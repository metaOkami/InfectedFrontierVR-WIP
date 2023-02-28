using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GrababbleNetSychronizer : MonoBehaviourPun, IPunObservable
{
    public float positionSmoothness = 5f;
    public float rotationSmoothness = 10f;


    private Rigidbody rb;
    
    private Vector3 netPos;
    private Quaternion netRot;


    private void Update()
    {
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, netPos, 5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, netRot, 10f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            netPos = (Vector3)stream.ReceiveNext();
            netRot = (Quaternion)stream.ReceiveNext();

        }
    }
}
