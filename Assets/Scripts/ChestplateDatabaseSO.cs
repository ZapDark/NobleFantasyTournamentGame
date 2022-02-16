using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "Chestplate Database", menuName = "CustomSO/ChestplateDatabase")]
public class ChestplateDatabaseSO : ScriptableObject
{
    [System.Serializable]
    public struct ChestplateData
    {
        public string name;
        public Sprite chestpiece;
        public int hpIncrease;
    }

    public List<ChestplateData> allChestplate;
}