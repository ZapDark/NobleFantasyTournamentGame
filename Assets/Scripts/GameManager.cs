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
        // Create the 2 teams.
        entitiesByTeam.Add(Team.Team1, new List<BaseEntity>());
        entitiesByTeam.Add(Team.Team2, new List<BaseEntity>());
    }

    public void OnEntitySelected(BodyDatabaseSO.BodyData bodyData, HelmetDatabaseSO.HelmetData helmetData, ChestplateDatabaseSO.ChestplateData chestplateData, WeaponDatabaseSO.WeaponData weaponData)
    {
        BaseEntity newEntity = Instantiate(bodyData.prefab/*, entitiesByTeam[Team.Team1]*/);
        newEntity.gameObject.name = bodyData.name + " Ally";
        
        List<SpriteRenderer> sprites = new List<SpriteRenderer>();
        newEntity.gameObject.GetComponentsInChildren<SpriteRenderer>(false, sprites);

        sprites[0].sprite = bodyData.body;
        sprites[1].sprite = helmetData.headpiece;
        sprites[2].sprite = chestplateData.chestpiece;
        sprites[3].sprite = weaponData.weapon;

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
            int randomBodyI = UnityEngine.Random.Range(0, bodyDatabase.allBodies.Count - 1);
            int randomHelmetI = UnityEngine.Random.Range(0, helmetDatabase.allHelmets.Count - 1);
            int randomChestplateI = UnityEngine.Random.Range(0, chestplateDatabase.allChestplate.Count - 1);
            int randomWeaponI = UnityEngine.Random.Range(0, weaponDatabase.allWeapons.Count - 1);

            BaseEntity newEntity = Instantiate(bodyDatabase.allBodies[randomBodyI].prefab);

            newEntity.gameObject.name = bodyDatabase.allBodies[randomBodyI].name + " Enemy";
        
            List<SpriteRenderer> sprites = new List<SpriteRenderer>();
            newEntity.gameObject.GetComponentsInChildren<SpriteRenderer>(false, sprites);

            sprites[0].sprite = bodyDatabase.allBodies[randomBodyI].body;
            sprites[1].sprite = helmetDatabase.allHelmets[randomHelmetI].headpiece;
            sprites[2].sprite = chestplateDatabase.allChestplate[randomChestplateI].chestpiece;
            sprites[3].sprite = weaponDatabase.allWeapons[randomWeaponI].weapon;

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