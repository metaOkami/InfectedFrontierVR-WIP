using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State currentState;

    public virtual void Update()
    {
        //Llamamos continuamente al OnUpdate
        currentState.OnUpdate();
    }

    public void ChangeState(State _newState)
    {
        //Salimos del estado en el que esta actualmente
        currentState.OnExit();
        //Asignamos el nuevo estado como estado actual
        currentState = _newState;
        //Llamamos al Enter del estado nuevo
        currentState.OnEnter();
    }
}
