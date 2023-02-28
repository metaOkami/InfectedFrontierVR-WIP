using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE_Move : State
{
    protected BaseEnemy_SM sm;
    public BE_Move(BaseEnemy_SM _sm)
    {
        sm = _sm;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        sm.SetDestination();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        sm.DetectObstacle();
        if (sm.obstacle != null)
        {
            sm.anim.SetBool("Attack", true);
            sm.ChangeState(new BE_Attack(sm));
        }
    }
}
