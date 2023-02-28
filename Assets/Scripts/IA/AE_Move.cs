using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_Move : State
{
    protected AttackEnemy_SM sm;
    public AE_Move(AttackEnemy_SM _sm)
    {
        sm = _sm;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        sm.positionOfDestination = sm.portalPos.transform.position;
        sm.SetDestination();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //sm.agent.SetDestination(sm.portalPos.position);
        sm.DetectTargets();
        sm.DetectObstacle();
        if (sm.target != null)
        {
            sm.anim.SetBool("Run", true);
            sm.ChangeState(new AE_Chase(sm));
        }
        if (sm.obstacle != null)
        {
            sm.anim.SetBool("Attack", true);
            sm.ChangeState(new AE_Attack(sm));
        }
    }

}
