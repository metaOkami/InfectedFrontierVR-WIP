using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Trap : MonoBehaviourPun
{
    public bool isPlaced = false; //Para comprobar que no haga su efecto antes de que isPlaced este en true
    public Material holoMat; //El material que debe tener la trampa cuando esta en modo placeholder
    public Renderer rend;
    public Material normalMat;
    public bool isStatic = false;

    private Material initialMat;
    public GameObject trapPrefab;

    private void Awake()
    {
        if (isStatic)
        {
            Place();
        }
        

        if (!photonView.IsMine)
        {
            rend.material = normalMat;
        }
    }

    private void Update()
    {
        if (isPlaced)
        {
            rend.material = normalMat;
        }
    }
    public void Place() //Se marca como colocada y actualizamos el material y su layer
    {
        
        PhotonNetwork.Instantiate(trapPrefab.name, transform.position, transform.rotation);
        
        isPlaced = true;
        rend.material = normalMat;
        //Se cambia la layer a Trap para que cuente como obstaculo a la hora de intentar colocar otra trampa
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
        
    }

    public void SetHoloMatColor() //Para cambiar el color del material holografico para indicar si se puede colocar o no
    {
        rend.material = holoMat;
    }

    

    

}
