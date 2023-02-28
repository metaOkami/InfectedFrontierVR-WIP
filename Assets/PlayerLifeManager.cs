using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
public class PlayerLifeManager : MonoBehaviourPun
{
    public int PlayerLife = 100;
    public float timeToRespawn;
    public Slider yourLife, bigLife;
    public GameObject myPhysicsRig;
    public GameObject myXROrigin;
    public Transform respawnPoint;
    public GameObject respawnCanvas;
    ContinuousMovementPhysics movement;
    public TMP_Text counterTxt;

    float timer;
    bool isDead;
    private void Start()
    {
        myXROrigin = GameObject.Find("XR Origin");
        myPhysicsRig = GameObject.Find("PhysicRig");
        respawnPoint = GameObject.Find("Respawn").transform;
    }

    public void HitPlayer(int damage)
    {
        Debug.Log("Aplico daño");
        if (photonView.IsMine)
        {
            PlayerLife -= damage;
            photonView.RPC(nameof(RPC_HitPlayer), RpcTarget.Others, damage, photonView.ViewID);
        }
    }

    private void Update()
    {
        
        PlayerLife = Mathf.Clamp(PlayerLife, 0, 100);
        if (yourLife.value != 0)
        {
            yourLife.value = (float)PlayerLife / 100;
            bigLife.value = (float)PlayerLife / 100;
        }
        if (isDead)
        {
            counterTxt.text= (timeToRespawn - Time.deltaTime).ToString();
        }
        if (PlayerLife <= 0)
        {
            Respawn();
            isDead = true;
        }
        
    }

    private void Respawn()
    {
        if (photonView.IsMine)
        {
            photonView.RPC(nameof(RPC_RestorePlayerLife), RpcTarget.All, photonView.ViewID);
            PhysicVRRig myVRRig = myPhysicsRig.GetComponent<PhysicVRRig>();
            Rigidbody rb = myPhysicsRig.GetComponentInChildren<Rigidbody>();
            rb.useGravity = false;
            myVRRig.enabled = false;
            PlayerLife = 100;
            yourLife.value = 1;
            bigLife.value = 1;
            //myPhysicsRig.transform.position = new Vector3(respawnPoint.transform.position.x,30f,respawnPoint.transform.position.z);
            myXROrigin.transform.position = new Vector3(respawnPoint.transform.position.x, 30f, respawnPoint.transform.position.z);

            movement = myPhysicsRig.GetComponent<ContinuousMovementPhysics>();
            movement.enabled = false;
            respawnCanvas.SetActive(true);
            StartCoroutine(WaitForRespawn());
        }
        
    }

    IEnumerator WaitForRespawn()
    {
        
        yield return new WaitForSeconds(timeToRespawn);
        PhysicVRRig myVRRig = myPhysicsRig.GetComponent<PhysicVRRig>();
        Rigidbody rb = myPhysicsRig.GetComponentInChildren<Rigidbody>();
        rb.useGravity = true;
        myVRRig.enabled = true;
        respawnCanvas.SetActive(false);
        movement.enabled = true;
        isDead = false;
    }
    [PunRPC]
    public void RPC_RestorePlayerLife(int viewID)
    {
        PlayerLifeManager otherLifeManager = PhotonView.Find(viewID).GetComponent<PlayerLifeManager>();
        otherLifeManager.PlayerLife = 100;
    }

    [PunRPC]
    public void RPC_HitPlayer(int _damage, int viewID)
    {
        PlayerLifeManager otherLifeManager = PhotonView.Find(viewID).GetComponent<PlayerLifeManager>();
        otherLifeManager.PlayerLife -= _damage;
    }
}
