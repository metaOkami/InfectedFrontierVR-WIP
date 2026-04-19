using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
public class MenuRayController : MonoBehaviour
{
    public InputActionProperty menuAction;
    public GameObject ray;

    private void Update()
    {
        if (menuAction.action.WasPressedThisFrame())
        {
            ray.SetActive(!ray.gameObject.activeSelf);
        }
    }
}
