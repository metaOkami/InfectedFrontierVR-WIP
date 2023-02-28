using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ExplosionTrapBehaviour : MonoBehaviourPun
{
    public ParticleSystem explosionPS;
    public float timeToDestroy = 1f;
    public float damage = 60;
    public AudioSource ExplosionSound;


    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Runner")
        {
            other.GetComponent<BaseEnemy_SM>().TakeDamage(damage);
            photonView.RPC(nameof(RPC_Explode), RpcTarget.All, photonView.ViewID);

            PhotonNetwork.Destroy(this.gameObject);
            ExplosionSound.Play();
        }
        if (other.gameObject.tag == "Fighter")
        {
            other.GetComponent<AttackEnemy_SM>().TakeDamage(damage);
            photonView.RPC(nameof(RPC_Explode), RpcTarget.All, photonView.ViewID);
            PhotonNetwork.Destroy(this.gameObject);
            ExplosionSound.Play();
        }
        
    }


    [PunRPC]
    void RPC_Explode(int viewID)
    {
        explosionPS = PhotonView.Find(viewID).GetComponentInChildren<ParticleSystem>();
        explosionPS.gameObject.transform.SetParent(null);
        explosionPS.Play();
        

    }
}
