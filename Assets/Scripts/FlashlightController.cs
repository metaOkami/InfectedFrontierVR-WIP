using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public bool isOn = false;
    public GameObject spotLight;

    public MeshRenderer flashLightModel;
    public Material matOff;
    public Material matOn;

    // Start is called before the first frame update
    public void FlashLight()
    {
        isOn = !isOn;
    }

    // Update is called once per frame
    void Update()
    {
        spotLight.SetActive(isOn);
        if (!isOn)
        {
            flashLightModel.material = matOff;
        }
        else
        {
            flashLightModel.material = matOn;
        }
        
    }
}
