using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class NFTS : ScriptableObject
{
   // [SerializeField] string id;
   // public string ID { get { return id; } }
    public string description;
    public Sprite icon;

  /*  private void OnDestroy()
    {
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }*/
}

