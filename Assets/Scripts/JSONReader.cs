using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JSONReader : MonoBehaviour
{
    public TextAsset jsonFile;
    public List<string> values = new List<string>();

    void Start()
    {
        Attributes attributesInJson = JsonUtility.FromJson<Attributes>(jsonFile.text);

        foreach (Attribute attribute in attributesInJson.attributes)
        {
            //Debug.Log("Found attribute: " + attribute.Trait_type + " " + attribute.Value);
            values.Add(attribute.Value);
        }
    }
}