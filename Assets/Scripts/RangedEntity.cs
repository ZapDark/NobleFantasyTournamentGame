using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEntity : BaseEntity
{
    public void Update()
    {
        if (!HasEnemy)
        {
            FindTarget();
        }

        if (IsInRange && !moving)
        {
            //In range for attack!
            if (canAttack)
            {
                Attack();
                currentTarget.TakeDamage(baseDamage);
            }
        }
        else
        {
            GetInRange();
        }
    }
}