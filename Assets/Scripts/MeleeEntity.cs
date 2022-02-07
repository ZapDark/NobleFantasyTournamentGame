using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEntity : BaseEntity
{
    public void Update()
    {
        //Find new target
        if(!HasEnemy)
        {
            FindTarget();
        }
        
        if(IsInRange && !moving)
        {
            //Attack!
            if(canAttack)
            {
                Attack();
            }
        }
        else
        {
            GetInRange();
        }
    }

    protected override void Attack()
    {
        base.Attack();
        currentTarget.TakeDamage(baseDamage);
    }
}
