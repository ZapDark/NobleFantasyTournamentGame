using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "Helmet Database", menuName = "CustomSO/HelmetDatabase")]
public class HelmetDatabaseSO : ScriptableObject
{
    [System.Serializable]
    public struct HelmetData
    {
        public string name;
        public Sprite headpiece;
        public int hpIncrease;
    }

    public List<HelmetData> allHelmets;
}