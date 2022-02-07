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
    
    //int unitsPerTeam = 3;

    void Start()
    {
        
        // Create the 2 teams.
    }

    /*private void InstantiateUnits()
    {
        entitiesByTeam.Add(Team.Team1, new List<BaseEntity>());
        entitiesByTeam.Add(Team.Team2, new List<BaseEntity>());
        for(int i = 0; i < unitsPerTeam; i++)
        {
            //New unit for team 1
            int randomIndex = UnityEngine.Random.Range(0, allEntitiesPrefab.Count - 1);
            BaseEntity newEntity = Instantiate(allEntitiesPrefab[randomIndex]);
            entitiesByTeam[Team.Team1].Add(newEntity);

            newEntity.Setup(Team.Team1, GridManager.Instance.GetFreeNode(Team.Team1));

            //New unit for team 2
            randomIndex = UnityEngine.Random.Range(0, allEntitiesPrefab.Count - 1);
            newEntity = Instantiate(allEntitiesPrefab[randomIndex]);
            entitiesByTeam[Team.Team2].Add(newEntity);

            newEntity.Setup(Team.Team2, GridManager.Instance.GetFreeNode(Team.Team2));
        }
    }*/

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
}

public enum Team
{
    Team1,
    Team2
}