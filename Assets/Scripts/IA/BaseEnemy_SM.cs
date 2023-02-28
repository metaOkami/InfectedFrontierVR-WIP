using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public class BaseEnemy_SM : StateMachine
{
    public NavMeshAgent agent;
    public float health = 100;
    public GameObject destination;
    public float timeToAttack;
    public LayerMask obstacleLayer;
    public float detectArea;
    public Transform obstacle;
    public Animator anim;
    public bool isOnSpike = false;
    private Rigidbody[] rigidbodys;
    private PhotonView myphotonview;
    public float timeToDestroy = 5f;

    private Vector3 targetDestination;
    public Vector3 positionOfDestination;

    private Vector3 currentPosition;
    private Vector3 netPosition;
    public AudioSource GrowlingSound;
    public AudioSource GetHitSound;

    void Start()
    {
        Debug.Log("He entrado en el start del enemigo");
        if (PhotonNetwork.IsMasterClient)
        {

            //myphotonview.RPC(nameof(RPC_SyncPosition), RpcTarget.Others);
        }
        positionOfDestination = destination.transform.position;
        agent = GetComponent<NavMeshAgent>();
        currentState = new BE_Move(this);
        rigidbodys = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodys)
        {
            rigidbody.isKinematic = true;
        }
        myphotonview = GetComponent<PhotonView>();

        SetDestination();
        SyncPosition();
    }
    public void DetectObstacle()
    {
        Collider[] _targets = Physics.OverlapSphere(transform.position, detectArea, obstacleLayer);
        if (_targets.Length > 0)
        {
            obstacle = _targets[0].transform;
        }
        else
        {
            obstacle = null;

        }
    }

    public void MakeDamage()
    {
        if (obstacle != null)
        {
            obstacle.GetComponent<BarricateBehaviour>().GetHit(40f);

        }
    }

    [PunRPC]
    public void RPC_EnemyTakeDamage(float damageToDo)
    {
        health -= damageToDo;

    }
    public void TakeDamage(float _damage)
    {
        myphotonview.RPC(nameof(RPC_EnemyTakeDamage), RpcTarget.All, _damage);
        GetHitSound.Play();
        StartCoroutine(CTR_GrowlingSound());
        if (isOnSpike == false)
        {
            anim.SetTrigger("Hit");
        }
        if (health <= 0)
        {

            Die();
        }
    }

    IEnumerator CTR_GrowlingSound() 
    {
        GrowlingSound.Pause();
        yield return new WaitForSeconds(0.1f);
        GrowlingSound.Play();

    }

    public void Die()
    {

        GrowlingSound.Pause();
        myphotonview.RPC(nameof(RPC_SetEnabled), RpcTarget.All, true);
        StartCoroutine(CRT_Destroy());
    }

    public IEnumerator CRT_Destroy()
    {
        yield return new WaitForSeconds(timeToDestroy);
        myphotonview.RPC(nameof(RPC_BEDestroy), RpcTarget.All);
    }

    [PunRPC]
    public void RPC_BEDestroy()
    {
        Wave_Manager.Instance.EnemyKilled();
        Destroy(this.gameObject);
    }

    public void ChangeMovement()
    {
        agent.isStopped = !agent.isStopped;
    }

    [PunRPC]
    void RPC_SetEnabled(bool _enable)
    {
        SetEnabled(_enable);
        

    }

    public void SetEnabled(bool enable)
    {
        this.gameObject.GetComponent<CapsuleCollider>().enabled = !enable;
        bool iskinematic = !enable;
        
        foreach (Rigidbody rigidbody in rigidbodys)
        {
            rigidbody.isKinematic = iskinematic;
        }
        anim.enabled = !enable;
        agent.enabled = !enable;
    }

    

    public void SyncPosition()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            currentPosition = transform.position;
            myphotonview.RPC(nameof(RPC_SyncPosition), RpcTarget.All);
        }
        netPosition = currentPosition;

    }
    [PunRPC]
    public void RPC_SyncPosition()
    {
        transform.position = netPosition;
    }

    public void SetDestination()
    {
        Debug.Log("Llega a SetDestination");
        myphotonview.RPC(nameof(RPC_SetDestination), RpcTarget.All, positionOfDestination);
        agent.SetDestination(targetDestination);
    }

    [PunRPC]
    void RPC_SetDestination(Vector3 _destination)
    {
        targetDestination = _destination;
        
    }

    public override void Update()
    {
        base.Update();
    }
    
}
