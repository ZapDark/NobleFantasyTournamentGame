using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IPointerClickHandler//, IPointerEnterHandler, IPointerExitHandler

{
    [SerializeField] Image image;
    [SerializeField] NFTDisplay display;


    public event Action<NFTS> OnLeftClickEvent;
    private NFTS _nft;
    private UICard uiCard;
    public NFTS nft
    {
        get { return _nft; }
        set
        {
            _nft = value;
            if(_nft == null)
            {
                image.enabled = false;
            }
            else
            {
                image.sprite = _nft.icon;
                image.enabled = true;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData != null && eventData.button == PointerEventData.InputButton.Left)
        {
            if (nft != null && OnLeftClickEvent != null)
                OnLeftClickEvent(nft);
        }
    }

    protected virtual void OnValidate()
    {
        if (image == null)
        { 
            image = GetComponent<Image>();
        }

        if (display == null)
        {
            display = FindObjectOfType<NFTDisplay>();
        }
    }

   /* public void OnPointerEnter(PointerEventData eventData)
    {
        if (nft is Equip)
        {
            display.DisplayName((Equip)nft);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        display.HideDisplay();
    }*/
}
