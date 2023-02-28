using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    
    // Start is called before the first frame update
    void Start()
    {
        ConnectToServer();
        
    }

    

    void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try to connect the server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the server");
        base.OnConnectedToMaster();
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        options.IsVisible = true;
        options.IsOpen = true;
        
        PhotonNetwork.JoinOrCreateRoom("sala",options,TypedLobby.Default);

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New player entered the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
