using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    public Healthbar barPrefab;
    public SpriteRenderer body;
    public SpriteRenderer headpiece;
    public SpriteRenderer chestpiece;
    public SpriteRenderer weapon;

    public string weaponName;
    public Animator atkAnimator;

    public int baseDamage = 5;
    public int baseHealth = 100;

    public int range = 1;
    public float attackSpeed = 1f; //Attacks per second
    public float movementSpeed = 1f;

    public int actionTick = 0;

    protected Team myTeam;
    protected BaseEntity currentTarget = null;
    protected Node currentNode;

    protected bool canAttack = true;
    protected bool dead = false;
    protected float waitBetweenAttack;
    protected Healthbar healthbar;

    public Team UnitTeam => myTeam;
    public Node CurrentNode => currentNode;
    
    protected bool inRangeEnemy = false;
    protected bool HasEnemy => currentTarget != null;
    protected bool IsInRange => HasEnemy && inRangeEnemy;
    protected bool moving;
    protected Node destination;
    protected int xOff = 1;

    public void Setup(Team team, Node currentNode)
    {
        actionTick = 0;
        TimeTickSystem.OnTick += TimeTickSystem_OnTick;
        myTeam = team;
        if(myTeam == Team.Team2)
        {
            body.flipX = true;
            chestpiece.flipX = true;
            headpiece.flipX = true;
            weapon.flipX = true;
        }

        this.currentNode = currentNode;
        transform.position = currentNode.worldPosition;
        currentNode.SetOccupied(true);
        healthbar = Instantiate(barPrefab, this.transform);
        healthbar.Setup(this.transform, baseHealth);
        
        if(myTeam == Team.Team2)
            xOff = -1;

        atkAnimator.SetBool("Sword", (weaponName == "Sword"));
        atkAnimator.SetBool("Kunai", (weaponName == "Kunai"));
        atkAnimator.SetBool("Healing Staff", (weaponName == "Healing Staff"));
        atkAnimator.SetBool("Monk Fist", (weaponName == "Monk"));
        atkAnimator.SetBool("Necro Torch", (weaponName == "Torch"));
        atkAnimator.SetBool("Bow", (weaponName == "Bow"));
        atkAnimator.SetBool("Magic Staff", (weaponName == "Magic Staff"));
    }

    public void TakeDamage(int amount)
    {
        baseHealth -= amount;
        healthbar.UpdateBar(baseHealth);
        if(baseHealth <= 0)
        {
            dead = true;
            currentNode.SetOccupied(false);
            //Informs the Game Manager
            GameManager.Instance.UnitDead(this);
            destination.SetOccupied(false);
        }
    }

    public void SetCurrentNode(Node node)
    {
        currentNode = node;
    }

    protected void FindTarget()
    {
        var allEnemies = GameManager.Instance.GetEntitiesAgainst(myTeam);
        //Debug.Log(allEnemies[0].gameObject.name+", "+allEnemies[1].gameObject.name+", "+allEnemies[2].gameObject.name);
        float minDistance = Mathf.Infinity;
        BaseEntity entity = null;
        foreach (BaseEntity e in allEnemies)
        {
            if(Vector3.Distance(e.currentNode.mapPosition, this.currentNode.mapPosition) <= minDistance)
            {
                minDistance = Vector3.Distance(e.currentNode.mapPosition, this.currentNode.mapPosition);
                entity = e;
            }
        }

        currentTarget = entity;
        inRangeEnemy = (minDistance <= range + 0.9f);
    }

    protected void MoveTowards(Node nextNode)
    {
        Vector3 direction = (nextNode.worldPosition - this.transform.position);
        if(direction.sqrMagnitude <= 1.3f)
            this.transform.position = nextNode.worldPosition;
    }

    protected void GetNodesInFront(int xOffset)
    {
        Vector3 destinationPos = new Vector3(this.currentNode.mapPosition.x + xOffset, this.currentNode.mapPosition.y, this.currentNode.mapPosition.z);
        List<Node> candidates = GridManager.Instance.GetNodesCloseTo(this.currentNode);

        foreach (Node cand in candidates)
        {
            if (cand.mapPosition == destinationPos)
            {    
                destination = cand;
                break;
            }
        }
    }

    protected void GetInRange()
    {
        if (currentTarget == null)
            return;
        
        GetNodesInFront(xOff);

        if(destination == null)
            return;
        
        if (actionTick >= movementSpeed)
        {
            if(destination.IsOccupied)
                return;
            
            //Take ownership of the node
            destination.SetOccupied(true);
            
            MoveTowards(destination);

            currentNode.SetOccupied(false);
            //Update current node
            currentNode = destination;
            actionTick = 0;
        }
    }

    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        if (!dead)
        {
            FindTarget();
            actionTick += 1;
            if (actionTick >= attackSpeed)
            {
                
                if (IsInRange)
                {
                    atkAnimator.SetBool("IsAttacking", true);

                    actionTick = 0;
                    currentTarget.TakeDamage(baseDamage);
                }
                else
                {
                    atkAnimator.SetBool("IsAttacking", false);
                    GetInRange();
                }
            }
        }
    }
}
