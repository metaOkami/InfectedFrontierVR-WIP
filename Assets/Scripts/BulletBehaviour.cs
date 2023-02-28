using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletBehaviour : MonoBehaviourPun
{
    public float bulletSpeed;
    public float timeToDestroy;
    private Rigidbody _bulletRb;
    private float _timer;
    private EconomyManager economyMan;
   

    private void Start()
    {
        
        
        economyMan = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<EconomyManager>();
        _bulletRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _bulletRb.AddForce(transform.forward * bulletSpeed,ForceMode.Force);
        _timer += Time.fixedDeltaTime;
        
        if (_timer >= timeToDestroy)
        {
            if (photonView.IsMine)
            {

                PhotonNetwork.Destroy(this.gameObject);
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Runner")
        {
            if (photonView.IsMine)
            {
                economyMan.EarnMoney();
                PhotonNetwork.Destroy(this.gameObject);
            }
            if(this.name=="SniperBullet(Clone)" || this.name == "SniperClone")
            {
                collision.transform.GetComponent<BaseEnemy_SM>().TakeDamage(100);
            }else if(this.name== "45ACP Bullet" || this.name== "45ACP Bullet(Clone)")
            {
                collision.transform.GetComponent<BaseEnemy_SM>().TakeDamage(40);

            }
        }
        if (collision.transform.tag == "Fighter")
        {
            if (photonView.IsMine)
            {
                economyMan.EarnMoney();
                PhotonNetwork.Destroy(this.gameObject);
            }
            if (this.name == "SniperBullet(Clone)" || this.name == "SniperClone")
            {
                collision.transform.GetComponent<AttackEnemy_SM>().TakeDamage(100);
            }
            else if (this.name == "45ACP Bullet" || this.name == "45ACP Bullet(Clone)")
            {
                collision.transform.GetComponent<AttackEnemy_SM>().TakeDamage(40);

            }
        }
    }

}
