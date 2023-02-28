using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class AttackEnemy_SM : StateMachine
{
    public NavMeshAgent agent;
    public float health = 250;
    public float attackArea;
    public float detectArea;
    public Transform portalPos;
    public Transform target, obstacle;
    public Animator anim;
    public float timeToAttack = 3f;
    public LayerMask playerLayer, obstacleLayer;
    public bool obstacleDetected = false;
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
        if (PhotonNetwork.IsMasterClient)
        {

            //myphotonview.RPC(nameof(RPC_ASyncPosition), RpcTarget.Others);
        }
        positionOfDestination = portalPos.transform.position;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new AE_Move(this);
        rigidbodys = GetComponentsInChildren<Rigidbody>();
        myphotonview = GetComponent<PhotonView>();
        foreach (Rigidbody rigidbody in rigidbodys)
        {
            rigidbody.isKinematic = true;
        }
        SetDestination();
        
    }

    public void MakeDamage()
    {
        if (obstacle != null)
        {
            obstacle.GetComponent<BarricateBehaviour>().GetHit(40f);

        }
        if (target.transform.gameObject.layer == LayerMask.NameToLayer("NetPlayer"))
        {
            Debug.Log("Deberia de hacer daño al jugador");
            if (target.gameObject.GetComponentInParent<PlayerLifeManager>().PlayerLife <= 0 ||
                target.gameObject.GetComponentInParent<PlayerLifeManager>().PlayerLife-20<=0)
            {
                target.gameObject.GetComponentInParent<PlayerLifeManager>().HitPlayer(20);

                target = null;
            }
            target.gameObject.GetComponentInParent<PlayerLifeManager>().HitPlayer(20);
        }
    }
    [PunRPC]
    public void RPC_AEnemyTakeDamage(float damage)
    {
        health -= damage;

    }
    public void TakeDamage(float _damage)
    {
        myphotonview.RPC(nameof(RPC_AEnemyTakeDamage), RpcTarget.All, _damage);
        GetHitSound.Play();
        StartCoroutine(CTR_GrowlingSound());
        if (isOnSpike == false)
        {
            anim.SetTrigger("Hit");
        }
        if (health<=0)
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
        myphotonview.RPC(nameof(RPC_SetEnabled), RpcTarget.All, true);
        StartCoroutine(CRT_Destroy());
        GrowlingSound.Pause();
    }

    public void SetEnabled(bool enable)
    {
        bool iskinematic = !enable;
        foreach (Rigidbody rigidbody in rigidbodys)
        {
            rigidbody.isKinematic = iskinematic;
        }
        anim.enabled = !enable;
        agent.enabled = !enable;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = !enable;
    }


    [PunRPC]
    void RPC_SetEnabled(bool _enable)
    {
        SetEnabled(_enable);
    }

    public IEnumerator CRT_Destroy()
    {
        yield return new WaitForSeconds(timeToDestroy);
        myphotonview.RPC(nameof(RPC_AEDestroy), RpcTarget.All);
    }
    [PunRPC]
    void RPC_AEDestroy()
    {
        Wave_Manager.Instance.EnemyKilled();
        Debug.Log("Deberia destruirse");
        Destroy(this.gameObject);
    }

    public void SetDestination()
    {
        myphotonview.RPC(nameof(RPC_ASetDestination), RpcTarget.All, positionOfDestination);
        agent.SetDestination(targetDestination);
    }

    [PunRPC]
    void RPC_ASetDestination(Vector3 _destination)
    {
        targetDestination = _destination;

    }
    

    public void ChangeMovement()
    {
        agent.isStopped = !agent.isStopped;
    }

    public override void Update()
    {
        base.Update();
    }

    public void DetectObstacle()
    {
        Collider[] _obstacles = Physics.OverlapSphere(transform.position, attackArea, obstacleLayer);
        if (_obstacles.Length > 0)
        {
            Debug.Log("Detecto obstaculo");
            obstacle = _obstacles[0].transform;
            obstacleDetected = true;
        }
        else
        {
            Debug.Log("NO Detecto obstaculo");
            obstacle = null;
        }
    }


    public void DetectTargets()
    {
        //Guardamos todos los objetos encontrados con el overlap
        Collider[] _targets = Physics.OverlapSphere(transform.position, detectArea, playerLayer);
        //Si ha encontrado algún objeto, la longitud del array es mayor que 0
        if (_targets.Length > 0)
        {
            target = _targets[0].transform;
        }

        //Si el array está vacío, no ha encontrado nada
        else
        {
            //Dejamos el target a null para que deje de perseguirlo
            target = null;
        }
    }

    public void LookAtTarget()
    {
        if (target == null)
        {
            return;
        }
        //Calculamos la direccion con respecto al target
        Vector3 _direction = target.position - transform.position;
        //Hay que poner la Y en 0 para que solo haga el LookAt en el eje Y
        _direction.y = 0;
        //Orientamos al personaje para que mire hacia esa direccion
        Quaternion _rot = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, _rot, Time.deltaTime * 10f);
    }
    public float GetDistanceToTarget()
    {
        if (target == null)
        {
            return 0;
        }
        Vector3 _direction = target.position - transform.position;
        return _direction.sqrMagnitude;
    }

    public void ChaseTarget()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }



}
