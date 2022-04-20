using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadoutPanel : MonoBehaviour
{
    [SerializeField] Transform loadoutParent;
    [SerializeField] EquipSlot[] equipSlots;

    public event Action<NFTS> OnItemLeftClickedEvent;

    private void Start()
    {
        for (int i = 0; i < equipSlots.Length; i++)
        {
            equipSlots[i].OnLeftClickEvent += OnItemLeftClickedEvent;
        }
    }

    private void OnValidate()
    {
        equipSlots = loadoutParent.GetComponentsInChildren<EquipSlot>();
    }

    public bool AddNFT(Equip nft, out Equip preNft)
    {
        for (int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].Equipment == nft.Equipment)
            {
                preNft = (Equip)equipSlots[i].nft;
                equipSlots[i].nft = nft;
                return true;
            }
        }
        preNft = null;
        return false;
    }
    public bool RemoveNFT(Equip nft)
    {
        for (int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].nft == nft)
            {
                equipSlots[i].nft =null;
                return true;
            }
        }
        return false;
    }
}
