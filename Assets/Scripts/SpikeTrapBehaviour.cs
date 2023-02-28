using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpikeTrapBehaviour : MonoBehaviourPun
{
    public float trapHealth = 5;
    public float autoDamage = 1;
    private float timer = 1.5f;
    public float newMovSpeed = 0.3f;
    public float damage = 10;
    public ParticleSystem trapPS;


    private void Update()
    {
        if (trapHealth <= 0)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Runner")
        {
            other.GetComponent<BaseEnemy_SM>().agent.speed = newMovSpeed;
            other.GetComponent<BaseEnemy_SM>().isOnSpike = true;
        }
        if (other.gameObject.tag == "Fighter")
        {
            other.GetComponent<AttackEnemy_SM>().agent.speed = newMovSpeed;
            other.GetComponent<AttackEnemy_SM>().isOnSpike = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (timer <= 0)
        {
            if (other.gameObject.tag == "Runner")
            {
                other.GetComponent<BaseEnemy_SM>().TakeDamage(damage);
            }
            if (other.gameObject.tag == "Fighter")
            {
                other.GetComponent<AttackEnemy_SM>().TakeDamage(damage);
            }
            timer = 1.5f;
        }
        timer -= Time.deltaTime;
        trapHealth -= autoDamage * Time.deltaTime;
        if (trapHealth <= 0 )
        {
            if (other.gameObject.tag == "Runner")
            {
                other.GetComponent<BaseEnemy_SM>().agent.speed = 1f;
                other.GetComponent<BaseEnemy_SM>().isOnSpike = false;
            }
            if (other.gameObject.tag == "Fighter")
            {
                other.GetComponent<AttackEnemy_SM>().agent.speed = 1f;
                other.GetComponent<AttackEnemy_SM>().isOnSpike = false;
            }
            StartCoroutine(CRT_Destroy());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Runner")
        {
            other.GetComponent<BaseEnemy_SM>().agent.speed = 1f;
            other.GetComponent<BaseEnemy_SM>().isOnSpike = false;
        }
        if (other.gameObject.tag == "Fighter")
        {
            other.GetComponent<AttackEnemy_SM>().agent.speed = 1f;
            other.GetComponent<AttackEnemy_SM>().isOnSpike = false;
        }
    }
    public IEnumerator CRT_Destroy()
    {
        yield return new WaitForSeconds(.1f);
        photonView.RPC(nameof(RPC_Destroy), RpcTarget.All, photonView.ViewID);
        PhotonNetwork.Destroy(this.gameObject);

    }
    [PunRPC]
    void RPC_Destroy(int viewID)
    {
        ParticleSystem trapPS = PhotonView.Find(viewID).GetComponentInChildren<ParticleSystem>();
        trapPS.gameObject.transform.SetParent(null);
        trapPS.Play();
        
    }

}
