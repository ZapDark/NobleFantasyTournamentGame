using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Equipment
{
    Character1,
    Character2,
    Character3,
}
[CreateAssetMenu]
public class Equip : NFTS
{
    public Equipment Equipment;
}

