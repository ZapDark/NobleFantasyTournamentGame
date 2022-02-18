using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : Manager<GameManager>
{
    public BodyDatabaseSO bodyDatabase;
    public HelmetDatabaseSO helmetDatabase;
    public ChestplateDatabaseSO chestplateDatabase;
    public WeaponDatabaseSO weaponDatabase;
    
    Dictionary<Team, List<BaseEntity>> entitiesByTeam = new Dictionary<Team, List<BaseEntity>>();
    
    int unitsPerTeam = 3;

    void Start()
    {
        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e) 
        {
            //Debug.Log("tick: " + e.tick);
        };
        // Create the 2 teams.
        entitiesByTeam.Add(Team.Team1, new List<BaseEntity>());
        entitiesByTeam.Add(Team.Team2, new List<BaseEntity>());
    }

    public void OnEntitySelected(BodyDatabaseSO.BodyData bodyData, HelmetDatabaseSO.HelmetData helmetData, ChestplateDatabaseSO.ChestplateData chestplateData, WeaponDatabaseSO.WeaponData weaponData)
    {
        BaseEntity newEntity = Instantiate(bodyData.prefab/*, entitiesByTeam[Team.Team1]*/);
        newEntity.gameObject.name = bodyData.name + " Ally";
        newEntity.baseDamage = weaponData.damage;
        newEntity.range = weaponData.range;
        newEntity.attackSpeed = weaponData.attackSpeed;
        newEntity.baseHealth = newEntity.baseHealth + helmetData.hpIncrease + chestplateData.hpIncrease;
        
        newEntity.body.sprite = bodyData.body;
        newEntity.headpiece.sprite = helmetData.headpiece;
        newEntity.chestpiece.sprite = chestplateData.chestpiece;
        newEntity.weapon.sprite = weaponData.weapon;

        entitiesByTeam[Team.Team1].Add(newEntity);

        newEntity.Setup(Team.Team1, GridManager.Instance.GetFreeNode(Team.Team1));
    }
    
    public void UnitDead(BaseEntity e)
    {
        entitiesByTeam[e.UnitTeam].Remove(e);

        Destroy(e.gameObject);
    }

    public List<BaseEntity> GetEntitiesAgainst(Team againstTeam)
    {
        if(againstTeam == Team.Team1)
            return entitiesByTeam[Team.Team2];
        else
            return entitiesByTeam[Team.Team1];
    }

    public void DebugFight()
    {
        for(int i = 0; i < unitsPerTeam; i++)
        {
            //New unit for team 1
            BodyDatabaseSO.BodyData bodyData = bodyDatabase.allBodies[UnityEngine.Random.Range(0, bodyDatabase.allBodies.Count - 1)];
            HelmetDatabaseSO.HelmetData helmetData = helmetDatabase.allHelmets[UnityEngine.Random.Range(0, helmetDatabase.allHelmets.Count - 1)];
            ChestplateDatabaseSO.ChestplateData chestplateData = chestplateDatabase.allChestplate[UnityEngine.Random.Range(0, chestplateDatabase.allChestplate.Count - 1)];
            WeaponDatabaseSO.WeaponData weaponData = weaponDatabase.allWeapons[UnityEngine.Random.Range(0, weaponDatabase.allWeapons.Count - 1)];

            BaseEntity newEntity = Instantiate(bodyData.prefab);

            newEntity.gameObject.name = bodyData.name + " Enemy";
            newEntity.baseDamage = weaponData.damage;
            newEntity.range = weaponData.range;
            newEntity.attackSpeed = weaponData.attackSpeed;
            newEntity.baseHealth = newEntity.baseHealth + helmetData.hpIncrease + chestplateData.hpIncrease;
        
            newEntity.body.sprite = bodyData.body;
            newEntity.headpiece.sprite = helmetData.headpiece;
            newEntity.chestpiece.sprite = chestplateData.chestpiece;
            newEntity.weapon.sprite = weaponData.weapon;

            entitiesByTeam[Team.Team2].Add(newEntity);

            newEntity.Setup(Team.Team2, GridManager.Instance.GetFreeNode(Team.Team2));
        }
    }
}

public enum Team
{
    Team1,
    Team2
}