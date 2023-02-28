using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
public class EconomyManager : MonoBehaviourPun
{
    public int earnByShot = 10;
    public TMP_Text p1Text, p2Text;
    public TMP_Text moneySelf;
    public Button Barricate, Wire, Grenade;

    public GameObject barricatePrefab, wirePrefab, grenadePrefab;

    public TrapPlacer BTrapRayPlacer;

    public int money;
    private int otherMoney;

    private void Awake()
    {
        //BTrapRayPlacer = GameObject.FindGameObjectWithTag("BTrapPlacer").GetComponent<TrapPlacer>();
        //GTrapRayPlacer = GameObject.FindGameObjectWithTag("GTrapPlacer").GetComponent<TrapPlacer>();
        //WTrapRayPlacer = GameObject.FindGameObjectWithTag("WTrapPlacer").GetComponent<TrapPlacer>();



    }
    private void Start()
    {

        
        if (photonView.IsMine)
        {
            money = 0;
            moneySelf.text = money.ToString();
        }
        else
        {
            moneySelf.text = otherMoney.ToString();
            otherMoney = 0;
        }
        p2Text.enabled = false;
        
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (BTrapRayPlacer == null)
            {
                BTrapRayPlacer = GameObject.FindGameObjectWithTag("BTrapPlacer").GetComponent<TrapPlacer>();
                if (BTrapRayPlacer != null)
                {
                    BTrapRayPlacer.gameObject.SetActive(false);
                }
            }
            checkToBuyTraps();
            money = Mathf.Clamp(money, 0, 999);
            otherMoney = Mathf.Clamp(otherMoney, 0, 999);
        }
        
        //if (GTrapRayPlacer == null)
        //{
        //    GTrapRayPlacer = GameObject.FindGameObjectWithTag("GTrapPlacer").GetComponent<TrapPlacer>();
        //    if (GTrapRayPlacer != null)
        //    {
        //        GTrapRayPlacer.gameObject.SetActive(false);
        //    }
        //}
        //if (WTrapRayPlacer == null)
        //{
        //    WTrapRayPlacer = GameObject.FindGameObjectWithTag("WTrapPlacer").GetComponent<TrapPlacer>();
        //    if (WTrapRayPlacer != null)
        //    {
        //        WTrapRayPlacer.gameObject.SetActive(false);
        //    }
        //}
        CheckPlayersInRoom();
        SetMoneyText();
       
        
    }

    public void CheckPlayersInRoom()
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            p2Text.enabled = true;
        }
        else
        {
            p2Text.enabled = false;

        }
    }

    public void SetMoneyText()
    {
        if(photonView.IsMine && PhotonNetwork.IsMasterClient)
        {
            p1Text.text = money.ToString();
            

            moneySelf.text = money.ToString();
        }
        else if(photonView.IsMine && !PhotonNetwork.IsMasterClient)
        {
            p1Text.text = otherMoney.ToString();

            p2Text.text=  money.ToString();
            moneySelf.text = money.ToString();

        }
        if (!photonView.IsMine && PhotonNetwork.IsMasterClient)
        {
            p1Text.text = money.ToString();

            p2Text.text = otherMoney.ToString();
            moneySelf.text = otherMoney.ToString();


        }
        else if (!photonView.IsMine && !PhotonNetwork.IsMasterClient)
        {
            p1Text.text = otherMoney.ToString();
            p2Text.text = money.ToString();
            moneySelf.text = otherMoney.ToString();

        }
    }

    public void checkToBuyTraps()
    {
        if (money <= 100)
        {
            Barricate.interactable = false;
            Wire.interactable = false;
            Grenade.interactable = false;
        }
        if(money>=100 && money <= 200)
        {
            Barricate.interactable = true;
            Wire.interactable = false;
            Grenade.interactable = false;
        }
        if(money>=200 && money <= 300)
        {
            Barricate.interactable = true;
            Wire.interactable = true;
            Grenade.interactable = false;
        }
        else if(money>300)
        {
            Barricate.interactable = true;
            Wire.interactable = true;
            Grenade.interactable = true;
        }
    }

    public void SelectTrapBarricate()
    {
        if (photonView.IsMine)
        {
            if (money >= 100)
            {
                LoseMoney(100);
                
                BTrapRayPlacer.trapPrefab = barricatePrefab;
                BTrapRayPlacer.gameObject.SetActive(true);
                BTrapRayPlacer.OnPutTrap();
            }
        }
        
        
    }
    public void SelectTrapWire()
    {
        if (photonView.IsMine)
        {
            if (money >= 200)
            {
                LoseMoney(200);
                BTrapRayPlacer.trapPrefab = wirePrefab;
                BTrapRayPlacer.gameObject.SetActive(true);
                BTrapRayPlacer.OnPutTrap();
            }
        }
        
        
    }
    public void SelectTrapGrenade()
    {
        if (photonView.IsMine)
        {
            if (money >= 300)
            {
                LoseMoney(300);
                BTrapRayPlacer.trapPrefab = grenadePrefab;
                BTrapRayPlacer.gameObject.SetActive(true);
                BTrapRayPlacer.OnPutTrap();
            }
        }
        
        
    }


    [PunRPC]
    public void RPC_SendMoneyInfo(int _money)
    {
        Debug.Log("He mandado/recibido la pasta");
        otherMoney = _money;
    }

    public void EarnMoney()
    {
        Debug.Log("He ganado dinero");
        money += earnByShot;
        photonView.RPC(nameof(RPC_SendMoneyInfo), RpcTarget.Others, money);
    }

    public void LoseMoney(int _moneyToLose)
    {
        Debug.Log("He perdido dinero");
        money -= _moneyToLose;
        photonView.RPC(nameof(RPC_SendMoneyInfo), RpcTarget.All, money);

    }

}
