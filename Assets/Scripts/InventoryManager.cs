using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] LoadoutPanel loadout;

    private void Awake()
    {
        inventory.OnItemLeftClickedEvent += EquipFromInventory;
        loadout.OnItemLeftClickedEvent += UnequipFromLoadout;
    }

    private void EquipFromInventory(NFTS nft)
    {
        if (nft is Equip)
        {
            Equip((Equip)nft);
        }
    }

    private void UnequipFromLoadout(NFTS nft)
    {
        if(nft is Equip)
        {
            Unequip((Equip)nft);
        }
    }
    public void Equip(Equip nft)
    {
        if (inventory.RemoveNft(nft))
        {
            Equip preNft;
            if (loadout.AddNFT(nft, out preNft))
            {
                if (preNft != null)
                {
                    inventory.AddNft(preNft);
                }
            }
            else
            {
                inventory.AddNft(nft);
            }
        }
    }

    public void Unequip(Equip nft)
    {
        if(!inventory.isFull() && loadout.RemoveNFT(nft))
        {
            inventory.AddNft(nft);
        }
    }
}
