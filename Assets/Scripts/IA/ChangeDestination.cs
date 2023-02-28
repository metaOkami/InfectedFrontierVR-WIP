using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDestination : MonoBehaviour
{
    public GameObject newDestination;
    public int numberPossibility;
    

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==LayerMask.NameToLayer("Enemy"))
        {
            
            if (other.gameObject.tag == "Runner")
            {
                int _decision = Random.Range(1, numberPossibility+1);
                if (_decision == numberPossibility)
                {
                    other.GetComponent<BaseEnemy_SM>().positionOfDestination = newDestination.transform.position;
                    other.GetComponent<BaseEnemy_SM>().SetDestination();

                }
            }
            if (other.gameObject.tag == "Fighter")
            {
                other.GetComponent<AttackEnemy_SM>().positionOfDestination = newDestination.transform.position;
                other.GetComponent<BaseEnemy_SM>().SetDestination();

            }

        }
    }

}
