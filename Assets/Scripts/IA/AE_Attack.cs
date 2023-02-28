using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_Attack : State
{
    protected AttackEnemy_SM sm;
    public AE_Attack(AttackEnemy_SM _sm)
    {
        sm = _sm;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        sm.agent.isStopped = true;
        sm.LookAtTarget();
        sm.DetectObstacle();
        if (sm.GetDistanceToTarget() > sm.attackArea * sm.attackArea)
        {
            sm.agent.isStopped = false;
            sm.anim.SetBool("Run", true);
            sm.anim.SetBool("Attack", false);
            sm.ChangeState(new AE_Chase(sm));

        }
        if (sm.obstacleDetected == true)
        {
            if (sm.obstacle == null)
            {
                sm.obstacleDetected = false;
                sm.agent.isStopped = false;
                sm.anim.SetBool("Attack", false);
                sm.anim.SetBool("Run", false);
                sm.ChangeState(new AE_Move(sm));
            }
        }
    }

}
