using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface NFTTracker
{
    int Count(string nftID);
    NFTS RemoveNft(string nftID);
    bool RemoveNft(NFTS nft);
    bool AddNft(NFTS nft);
    bool IsFull();
}
