using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class ShotOnActivate : MonoBehaviourPun
{
    public GameObject bullet;
    public Transform spawnPoint;
    public InputActionProperty leftAction;
    public InputActionProperty rightAction;
    public int maxBullets;
    private int _currentBullets;
    public bool canShot;
    XRGrabInteractable grabbable;
    public Animator anim;

    public TMP_Text textoBalaActual;
    public TMP_Text Magazine;
    public GameObject canvasBalas;
    public AudioSource ShootSound;
    public ParticleSystem ShootParticles;

    

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            canvasBalas.SetActive(true);
            _currentBullets = maxBullets;
            anim = PhotonView.Find(photonView.ViewID).gameObject.GetComponentInChildren<Animator>();
            grabbable = GetComponent<XRGrabInteractable>();
            grabbable.activated.AddListener(FireBullet);
            canShot = true;
            Magazine.text = maxBullets.ToString();
            textoBalaActual.text = _currentBullets.ToString();


        }
    }

    private void Update()
    {
        if (rightAction.action.WasPressedThisFrame() || leftAction.action.WasPressedThisFrame())
        {
            if (grabbable.isSelected && photonView.IsMine)
            {
                anim.SetBool("isEmpty", false);
                anim.SetTrigger("Reloading");
                _currentBullets = maxBullets;
                canShot = true;
                textoBalaActual.text = _currentBullets.ToString();
            }
            
        }
    }

    [PunRPC]
    public void RPC_PlayParticles()
    {
        PlayParticles();
    }

    public void PlayParticles()
    {
        ShootSound.Play();
        ShootParticles.Play();
    }
    public void FireBullet(ActivateEventArgs arg)
    {
        if (_currentBullets ==0)
        {
            _currentBullets = maxBullets;
            canShot = false;
            if (anim.GetBool("isEmpty") == false)
            {
                _currentBullets = maxBullets;
                Debug.Log("Te has quedado sin balas");
                anim.SetBool("isEmpty", true);
                textoBalaActual.text = _currentBullets.ToString();
            }
            return;

        }

        if (canShot)
        {

            _currentBullets--;
            anim.SetTrigger("Shot");
            photonView.RPC(nameof(RPC_PlayParticles), RpcTarget.All);
            textoBalaActual.text = _currentBullets.ToString();
            PhotonNetwork.Instantiate(bullet.name, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
