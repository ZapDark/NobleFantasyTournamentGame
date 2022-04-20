using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NFTDisplay : MonoBehaviour
{
    [SerializeField] Text NftName;

    public void DisplayName(Equip nft)
    {
        NftName.text = nft.description;
        gameObject.SetActive(true);
    }

    public void HideDisplay()
    {
        gameObject.SetActive(false);
    }
}
