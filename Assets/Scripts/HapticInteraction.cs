using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


[System.Serializable] //Aquí serializamos la clase Haptic, para poder declararla más adelante
public class Haptic //Esta clase nos va a servir para asignar el mando de VR y mandarle la vibración
{
    //Estos son los dos valores dela vibración, intensidad y duración 
    [Range(0,1)]
    public float intensity;
    public float duration;

    //En esta función estamos asignando el mando de VR para poder llamarlo en la siguiente función 
    public void TriggerHaptic(BaseInteractionEventArgs eventArgs)
    {
        if(eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor) //Con esta línea estamos comprobando qué mando ha
                                                                                          //interactuado con el objeto y llamando a la siguiente función
        {
            TriggerHaptic(controllerInteractor.xrController);
        }
    }

    public void StopHaptic(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor) 
        {
            StopHaptic(controllerInteractor.xrController);
        }
    }

    public void TriggerHaptic(XRBaseController controller)  //Esta es la función que activa la vibración del mando asignando una duración e intensidad
    {
        if (intensity > 0)
        {
            controller.SendHapticImpulse(intensity, duration);
        }
    }
    public void StopHaptic(XRBaseController controller)  
    {
        
            controller.SendHapticImpulse(0, 0);
        
    }
}

public class HapticInteraction : MonoBehaviour
{
    //Aquí declaramos todas las funciones hápticas que vamos a realizar, es decir
    //cuándo queremos el vibre el mando al realizar X acción
    public Haptic hapticOnActivated; //Este evento es cuando pulsamos el gatillo con el objeto cogido
    public Haptic hapticOnDeactivated; //Este evento es llamada al dejar de pulsar el gatillo
    public Haptic hapticOnHoverEnter; //Este es cuando ponemos la mano encima
    public Haptic hapticOnHoverExit; //Este es cuando quitamos la mano de encima
    public Haptic hapticOnSelectEnter; //Este es al agarrarlo
    public Haptic hapticOnSelectExit; //Este al soltarlo
    


    void Start()
    {
        //Aquí enlazamos nuestros eventos hápticos con los eventos de OpenXR (OnActivate, OnHover, Select...)
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.activated.AddListener(hapticOnActivated.TriggerHaptic);
        interactable.deactivated.AddListener(hapticOnDeactivated.StopHaptic);
        interactable.hoverEntered.AddListener(hapticOnHoverEnter.TriggerHaptic);
        interactable.hoverExited.AddListener(hapticOnHoverExit.TriggerHaptic);
        interactable.selectEntered.AddListener(hapticOnSelectEnter.TriggerHaptic);
        interactable.selectExited.AddListener(hapticOnSelectExit.TriggerHaptic);

    }

    public void StopHapticImpulse()
    {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.deactivated.AddListener(hapticOnDeactivated.StopHaptic);
    }


}
