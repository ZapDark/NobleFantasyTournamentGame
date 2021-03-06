using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "Weapon Database", menuName = "CustomSO/WeaponDatabase")]
public class WeaponDatabaseSO : ScriptableObject
{
    [System.Serializable]
    public struct WeaponData
    {
        public string name;
        public Sprite weapon;
        public int damage;
        public int range;
        public float attackSpeed;
    }

    public List<WeaponData> allWeapons;
}
