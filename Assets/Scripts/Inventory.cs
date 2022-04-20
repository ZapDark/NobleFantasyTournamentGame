using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Inventory : MonoBehaviour
{
    [SerializeField] List<NFTS> nfts;
    [SerializeField] Transform nftsParent;
    [SerializeField] Slot[] nftSlots;

    public event Action<NFTS> OnItemLeftClickedEvent;

    private void Start()
    {
        for (int i = 0; i < nftSlots.Length; i++)
        {
            nftSlots[i].OnLeftClickEvent += OnItemLeftClickedEvent;
        }
    }
    private void OnValidate()
    {
        if (nftsParent != null)
            nftSlots = nftsParent.GetComponentsInChildren<Slot>();
        RefreshUI();
    }

    private void RefreshUI()
    {
        int i = 0;
        for (; i < nfts.Count && i < nftSlots.Length; i++)
        {
            nftSlots[i].nft = nfts[i];
        }

        for (; i < nftSlots.Length; i++)
        {
            nftSlots[i].nft = null;
        }
    }

    public bool AddNft(NFTS nft)
    {
        if (isFull())
            return false;

        nfts.Add(nft);
        RefreshUI();
        return true;
    }
    public bool RemoveNft(NFTS nft)
    {
        if (nfts.Remove(nft))
        {
            RefreshUI();
            return true;
        }
        return false;
    }

    /*public NFTS RemoveNft(string nftID)
    {
        for(int i = 0; i < nftSlots.Length; i++)
        {
            NFTS nft = nftSlots[i].nft;
            if (nft != null && nft.ID == nftID)
            {
                nftSlots[i].nft = null;
                return nft;
            }
        }
        return null;
    }*/
    public bool isFull()
    {
        return nfts.Count >= nftSlots.Length;
    }

   /* public int Count(string nftID)
    {
        int n = 0;
        for(int i = 0; i < nftSlots.Length; i++)
        {
            if(nftSlots[i].nft.ID == nftID)
            {
                n++;
            }
        }
        return n;
    }*/
}
