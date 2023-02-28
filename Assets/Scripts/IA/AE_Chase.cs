using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_Chase : State
{
    protected AttackEnemy_SM sm;
    public AE_Chase(AttackEnemy_SM _sm)
    {
        sm = _sm;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        sm.positionOfDestination = sm.target.transform.position;
        
        sm.SetDestination();

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        sm.ChaseTarget();
        sm.LookAtTarget();
        if (sm.target == null)
        {
            sm.anim.SetBool("Run", false);
            sm.ChangeState(new AE_Move(sm));
        }
        if (sm.GetDistanceToTarget() <= sm.attackArea)
        {
            sm.anim.SetBool("Attack", true);
            sm.anim.SetBool("Run", false);
            sm.ChangeState(new AE_Attack(sm));
        }
        
    }


}
