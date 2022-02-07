using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "Body Database", menuName = "CustomSO/BodyDatabase")]
public class BodyDatabaseSO : ScriptableObject
{
    [System.Serializable]
    public struct BodyData
    {
        public BaseEntity prefab;
        public string name;
        public Sprite body;
    }

    public List<BodyData> allBodies;
}
