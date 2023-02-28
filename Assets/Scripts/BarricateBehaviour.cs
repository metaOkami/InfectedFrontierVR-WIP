using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BarricateBehaviour : MonoBehaviourPun
{
    public float life = 100f;
    public ParticleSystem BarricatePS;
    public Trap myTrap;

    private void Start()
    {
        myTrap = GetComponent<Trap>();
        this.gameObject.layer = LayerMask.NameToLayer("Barricate");
    }
    

    public void GetHit(float damage)
    {
        Debug.Log("Me han dao soy la trampa");
        Debug.Log("Vida: "+life);
        photonView.RPC(nameof(RPC_TakeDamage), RpcTarget.All, damage);
        if (life <= 0)
        {
            myTrap.enabled = false;
            photonView.RPC(nameof(RPC_OnDestroyBarricate), RpcTarget.All);
            BarricatePS.gameObject.transform.SetParent(null);
            BarricatePS.Play();
            
        }
    }

    [PunRPC]
    public void RPC_OnDestroyBarricate()
    {
        
        DestroyBarricate();
    }

    [PunRPC]
    public void RPC_TakeDamage(float _damage)
    {
        
        life -= _damage;
    }

    public void DestroyBarricate()
    {
        Debug.Log("Debería destruirme, mi vida es: " + life);
        Destroy(this.gameObject);
    }

    
}
