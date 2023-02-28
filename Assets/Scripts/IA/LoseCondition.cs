using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class LoseCondition : MonoBehaviourPun
{
    public float portalHealth = 8f;
    public float damagePerZombie = 1f;
    public GameObject CanvasPerder;
    public TMP_Text vidaPortal;


    private void Start()
    {
        vidaPortal.text = "Portal Health : " + portalHealth;
        CanvasPerder.SetActive(false);
    }

    private void Update()
    {
        if (vidaPortal == null)
        {
            vidaPortal = GameObject.FindGameObjectWithTag("HealthTag").GetComponent<TMP_Text>();
        }
        if(portalHealth <= 0)
        {
            CanvasPerder.SetActive(true);
        }
        vidaPortal.text = "Portal Health : " + portalHealth;

    }

    [PunRPC]
    public void RPC_PortalLoseHealth(float damageToLose)
    {
        portalHealth -= damageToLose;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) 
        {

            photonView.RPC(nameof(RPC_PortalLoseHealth), RpcTarget.All, damagePerZombie);
            Wave_Manager.Instance.EnemyKilled();
            
            if (PhotonNetwork.IsMasterClient)
            {

                PhotonNetwork.Destroy(other.gameObject);
            }
        }
        //if (portalHealth <= 0)
        //{
        //    CanvasPerder.SetActive(true);
            
        //}
    }

}
