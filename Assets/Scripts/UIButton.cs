using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public Transform shopRef;
    private UIShop shop;
    public Button button; 

    private void Start()
    {
        shop = shopRef.GetComponent<UIShop>();
    }
    
    public void OnClick()
    {
        foreach (Transform card in shopRef.GetChild(0).GetChild(0))
        {
            UICard cardU = card.GetComponent<UICard>();
            shop.OnCardClick(cardU, cardU.bodyData, cardU.helmetData, cardU.chestplateData, cardU.weaponData);
        }
        button.interactable = false;
    }
}
