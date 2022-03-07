using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    public Image body;
    public Image headpiece;
    public Image chestpiece;
    public Image weapon;
    public Text name;

    private UIShop shopRef;
    public BodyDatabaseSO.BodyData bodyData;
    public WeaponDatabaseSO.WeaponData weaponData;
    public HelmetDatabaseSO.HelmetData helmetData;
    public ChestplateDatabaseSO.ChestplateData chestplateData;

    public void Setup(BodyDatabaseSO.BodyData bodyData, HelmetDatabaseSO.HelmetData helmetData, ChestplateDatabaseSO.ChestplateData chestplateData, WeaponDatabaseSO.WeaponData weaponData, UIShop shopRef)
    {
        body.sprite = bodyData.body;
        headpiece.sprite = helmetData.headpiece;
        chestpiece.sprite = chestplateData.chestpiece;
        weapon.sprite = weaponData.weapon;
        
        name.text = bodyData.name+" "+helmetData.name+" "+chestplateData.name+" "+weaponData.name;

        this.bodyData = bodyData;
        this.helmetData = helmetData;
        this.chestplateData = chestplateData;
        this.weaponData = weaponData;
        this.shopRef = shopRef;
    }

    public void OnClick()
    {
        //Tell the "shop"
        shopRef.OnCardClick(this, bodyData, helmetData, chestplateData, weaponData);
    }
}
