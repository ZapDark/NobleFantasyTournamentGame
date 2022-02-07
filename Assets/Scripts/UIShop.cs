using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShop : MonoBehaviour
{
    public List<UICard> allCards;

    private BodyDatabaseSO cBodyDb;
    private HelmetDatabaseSO cHelmetDb;
    private ChestplateDatabaseSO cChestplateDb;
    private WeaponDatabaseSO cWeaponDb;

    private void Start()
    {
        cBodyDb = GameManager.Instance.bodyDatabase;
        cHelmetDb = GameManager.Instance.helmetDatabase;
        cChestplateDb = GameManager.Instance.chestplateDatabase;
        cWeaponDb = GameManager.Instance.weaponDatabase;
        GenerateCard();
    }

    //Replace with GetJson in the future
    public void GenerateCard()
    {
        for(int i = 0; i < allCards.Count; i++)
        {
            allCards[i].Setup(cBodyDb.allBodies[Random.Range(0, cBodyDb.allBodies.Count)], cHelmetDb.allHelmets[Random.Range(0, cHelmetDb.allHelmets.Count)], cChestplateDb.allChestplate[Random.Range(0, cChestplateDb.allChestplate.Count)], cWeaponDb.allWeapons[Random.Range(0, cWeaponDb.allWeapons.Count)], this);
        }
    }

    public void OnCardClick(BodyDatabaseSO.BodyData bodyCData, HelmetDatabaseSO.HelmetData helmetCData, ChestplateDatabaseSO.ChestplateData chestplateCData, WeaponDatabaseSO.WeaponData weaponCData)
    {
        Debug.Log("Card clicked!");
    }
}
