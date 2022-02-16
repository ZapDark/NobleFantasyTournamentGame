using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MeleeEntity : BaseEntity
{
    private int ticks = 0;

    public void Update()
    {
        ticks++;
        if (ticks >= 3)
        {
            ticks = 0;
            FindTarget();
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
}
