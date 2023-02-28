using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE_Attack : State
{
    protected BaseEnemy_SM sm;
    public BE_Attack(BaseEnemy_SM _sm)
    {
        sm = _sm;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        sm.agent.isStopped = true;
        sm.DetectObstacle();
        if (sm.obstacle == null)
        {
            sm.agent.isStopped = false;
            sm.anim.SetBool("Attack", false);
            sm.ChangeState(new BE_Move(sm));
        }
    }

}
