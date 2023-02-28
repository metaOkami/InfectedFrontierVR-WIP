using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
public class TrapPlacer : MonoBehaviour
{
    public GameObject trapPrefab;
    public LayerMask blockLayer;
    public InputActionProperty PlaceTrapButton;
    
    public InputActionProperty RotateTrap;
    public XRRayInteractor rightRay;

    public XRDirectInteractor rightDirectInteractor;

    //public GameObject wallTrapPrefab;

    public Camera cam;
    public float rayLength = 2f;
    public float degreesToRotate=45f;
    float yPos = 0;
    private Trap tempTrap;

    public void OnPutTrap()
    {
        TryPlaceTrap();
    }

    void Update()
    {
        
        //Cuando la trampa temporal no es null, significa que estamos intentando colocar una trampa
        if (tempTrap != null)
        {
            
            if(rightRay.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                yPos = hit.point.y;
            }
            
            //Movemos la trampa placeholder a la posicion del suelo en la que se colocaria
            tempTrap.transform.position = GetRoundedCenterGroundPos(yPos);

            //if (RotateTrap.action.WasPressedThisFrame())
            //{
            //    tempTrap.transform.Rotate(new Vector3(0, degreesToRotate, 0));
            //}

            if (CanPlaceGroundTrap() == true)   //Si podemos colocar la trampa porque no hay ningun osbtaculo u otra trampa colocada...
            {
                if (PlaceTrapButton.action.WasPressedThisFrame()) //Usando la key de colocar trampa, ponemos la trampa en el suelo
                {
                    rightRay.gameObject.SetActive(false);
                    PlaceTrap();
                    tempTrap = null;
                }
            }
        }
    }

    void TryPlaceTrap()
    {
        switch(tempTrap == null) //Comprueba si la trampa placeholder existe o no para entrar/salir del modo de colocar trampa
        {
            //Si NO existe la trampla placeholder, la instancia para entrar al modo de colocar trampa y poder ver donde se colocara
            case true:
                tempTrap = Instantiate(trapPrefab, trapPrefab.transform.position, Quaternion.identity).GetComponent<Trap>();
                break;
            //Si SI existe la trampla placeholder, la destruye para salir del demodo colocar trampa
            case false:
                Destroy(tempTrap.gameObject);
                break;
        }
    }

    void PlaceTrap()
    {
        //Marcamos la trampa como colocada si no lo estaba aun
        if(tempTrap.isPlaced == false)
        {
            rightDirectInteractor.allowSelect = true;
            Destroy(tempTrap.gameObject);
            tempTrap.Place();
        }
    }

    //Comprueba con un CheckBox del mismo tamaño que la trampa si hay algun obstaculo u otra trampa colocada en la posicion donde queremos crear una trampa
    bool CanPlaceGroundTrap()
    {
        //Si hay algun obstaculo o trampa, pone el material de la trampa placeholder en rojo y devuelve false
        if (Physics.CheckBox(GetRoundedCenterGroundPos(tempTrap.transform.position.y), tempTrap.transform.localScale / 3.01f, tempTrap.transform.rotation, blockLayer))
        {
            //tempTrap.SetHoloMatColor(Color.red);
            return false;
        }
        else    //Si la posicion esta libre, pone el material de la trampa placeholder en verde y devuelve true
        {
            tempTrap.SetHoloMatColor();
            return true;
        }
    }

    //Calcula y devuelve la posicion del suelo en la que hay que mover la trampa para mostrar donde se colocaria
    Vector3 GetRoundedCenterGroundPos(float _yPos)
    {
        Vector3 _roundedPos = Vector3.zero;
        Ray _ray = cam.ViewportPointToRay(new Vector3(.5f, .5f, cam.nearClipPlane));
        Debug.DrawRay(_ray.origin, _ray.direction * rayLength, Color.red);
        Debug.DrawRay(_ray.origin + _ray.direction * rayLength, Vector3.down * 5f, Color.red);
        if (rightRay.TryGetCurrent3DRaycastHit(out RaycastHit _hit))
        {
            Debug.Log(_hit.point);
            _roundedPos.x = Mathf.RoundToInt(_hit.point.x);
            _roundedPos.z = Mathf.RoundToInt(_hit.point.z);
            _roundedPos.y = _hit.point.y;
        }
        
        _roundedPos.y = _yPos;
        return _roundedPos;
    }

    private void OnDrawGizmos()
    {
        if(tempTrap != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(GetRoundedCenterGroundPos(tempTrap.transform.position.y), tempTrap.transform.localScale * 1.005f);
            Gizmos.DrawRay(transform.position, transform.forward * 1.9f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, transform.forward * 2.5f);
        }
    }
}
