using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SceneManagementScript : MonoBehaviour
{
    
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
