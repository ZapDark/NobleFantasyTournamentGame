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

    protected Team myTeam;
    protected BaseEntity currentTarget = null;
    protected Node currentNode;

    protected bool canAttack = true;
    protected bool dead = false;
    protected float waitBetweenAttack;
    protected Healthbar healthbar;

    public Team UnitTeam => myTeam;
    public Node CurrentNode => currentNode;
    
    protected bool HasEnemy => currentTarget != null;
    protected bool IsInRange => HasEnemy && Vector3.Distance(this.transform.position, currentTarget.transform.position) <= (range + 0.3f);
    protected bool moving;
    protected Node destination;

    public void Setup(Team team, Node currentNode)
    {
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
        }
    }

    protected void FindTarget()
    {
        var allEnemies = GameManager.Instance.GetEntitiesAgainst(myTeam);
        float minDistance = Mathf.Infinity;
        BaseEntity entity = null;
        foreach (BaseEntity e in allEnemies)
        {
            if(Vector3.Distance(e.transform.position, this.transform.position) <= minDistance)
            {
                minDistance = Vector3.Distance(e.transform.position, this.transform.position);
                entity = e;
            }
        }

        currentTarget = entity;
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

    protected void GetInRange()
    {
        if (currentTarget == null)
            return;

        if(!moving)
        {
            Node candidateDestination = null;
            List<Node> candidates = GridManager.Instance.GetNodesCloseTo(currentTarget.currentNode); //Get a neighbors of a node
            candidates = candidates.OrderBy(x => Vector3.Distance(x.worldPosition, this.transform.position)).ToList();
            for(int i = 0; i < candidates.Count; i++)
            {
                if(!candidates[i].IsOccupied)
                {
                    candidateDestination = candidates[i];
                    break;
                }
            }

            if(candidateDestination == null)
                return;

            //find path to destination

            var path = GridManager.Instance.GetPath(currentNode, candidateDestination);
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

    protected virtual void Attack()
    {
        if(!canAttack)
            return;

        waitBetweenAttack = 1 / attackSpeed;
        //Wait for next attack
        StartCoroutine(WaitCoroutine());
    }

    IEnumerator WaitCoroutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(waitBetweenAttack);
        canAttack = true;
    }
}
