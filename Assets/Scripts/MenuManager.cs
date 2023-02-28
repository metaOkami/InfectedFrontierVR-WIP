using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;
    public InputActionProperty showButton;
    public GameObject menuRay;

    private void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
       
        if (showButton.action.WasPressedThisFrame())
        {
            
            menu.SetActive(!menu.activeSelf);
            
            
        }
    }
}
