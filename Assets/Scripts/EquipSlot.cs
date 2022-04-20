using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSlot : Slot
{
    public Equipment Equipment;

    protected override void OnValidate()
    {
        base.OnValidate();
        gameObject.name = Equipment.ToString() + " Slot";
    }
}
