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

    public int baseDamage = 5;
    public int baseHealth = 100;
    [Range(1,5)]
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

    protected bool MoveTowards(Node nextNode)
    {
        Vector3 direction = (nextNode.worldPosition - this.transform.position);
        if(direction.sqrMagnitude <= 0.005f)
        {
            transform.position = nextNode.worldPosition;
            return true;
        }

        this.transform.position += direction.normalized * movementSpeed * Time.deltaTime;
        return false;
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

        if(!moving)
        {
            destination = null;

            GetNodesInFront(xOff);

            /*List<Node> candidates = GridManager.Instance.GetNodesCloseTo(currentTarget.currentNode); //Get a neighbors of a node
            candidates = candidates.OrderBy(x => Vector3.Distance(x.worldPosition, this.transform.position)).ToList();
            for(int i = 0; i < candidates.Count; i++)
            {
                if(!candidates[i].IsOccupied)
                {
                    candidateDestination = candidates[i];
                    break;
                }
            }
            */

            if(destination == null)
                return;
            
            //find path to destination
            
            var path = GridManager.Instance.GetPath(currentNode, destination);

            if (path == null || path.Count <= 1)
                return;

            if(path[1].IsOccupied)
                return;
            
            //Take ownership of the node
            path[1].SetOccupied(true);
            destination = path[1];
        }

        moving = !MoveTowards(destination);
        if(!moving)
        {
            //Target reached
            //Free previous node
            currentNode.SetOccupied(false);
            //Update current node
            currentNode = destination;
        }
    }

    /*protected virtual void Attack()
    {
        if(!canAttack)
            return;

        waitBetweenAttack = attackSpeed / 2;
        //Wait for next attack
        //StartCoroutine(WaitCoroutine());
    }*/

    /*IEnumerator WaitCoroutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(waitBetweenAttack);
        canAttack = true;
    }*/

    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        if (!dead)
        {
            //Debug.Log("actionTick = " + actionTick + " Moving = " + moving + " | IsInRange = " + IsInRange);
            FindTarget();
            if (IsInRange && !moving)
            {
                actionTick += 1;
                if (actionTick >= attackSpeed)
                {
                    //Attack();
                    actionTick = 0;
                    currentTarget.TakeDamage(baseDamage);
                }
            }
            else
            {
                GetInRange();
            }
        }
    }
}
