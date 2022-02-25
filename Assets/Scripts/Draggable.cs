using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public LayerMask releaseMask;
    public Vector3 dragOffset = new Vector3(0, -0.4f, 0);
    
    private Tile validTile;
    private Camera cam;
    private SpriteRenderer bodySpriteRenderer;
    private SpriteRenderer helmetSpriteRenderer;
    private SpriteRenderer chestplateSpriteRenderer;
    private SpriteRenderer weaponSpriteRenderer;
    List<SpriteRenderer> spriteRenderers;
    
    private Vector3 oldPosition;
    private int oldSortingOrderBody;
    private int oldSortingOrderGear;
    private Vector3 previousTile;
    private Vector3 nullVector = new Vector3(-1000, -1000, -1000);

    public bool IsDragging = false;

    private void Start()
    {
        cam = Camera.main;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
        
        bodySpriteRenderer = spriteRenderers[0];
        helmetSpriteRenderer = spriteRenderers[1];
        chestplateSpriteRenderer = spriteRenderers[2];
        weaponSpriteRenderer = spriteRenderers[3];

        validTile = Tile.Instance;
    }
    
    public void OnStartDrag()
    {
        //Debug.Log(this.name + " start drag");

        oldPosition = this.transform.position;
        oldSortingOrderBody = bodySpriteRenderer.sortingOrder;
        oldSortingOrderGear = weaponSpriteRenderer.sortingOrder;

        bodySpriteRenderer.sortingOrder = 20;
        helmetSpriteRenderer.sortingOrder = 23;
        chestplateSpriteRenderer.sortingOrder = 22;
        weaponSpriteRenderer.sortingOrder = 21;
        IsDragging = true;
    }

    public void OnDragging()
    {
        if (!IsDragging)
            return;
        
        //Debug.Log(this.name + " dragging");

        Vector3 newPosition = cam.ScreenToWorldPoint(Input.mousePosition) + dragOffset;
        newPosition.z = 0;
        this.transform.position = newPosition;

        Vector3 tileUnder = cam.ScreenToWorldPoint(Input.mousePosition);
        tileUnder.z = 0;

        if (tileUnder != null)
        {
            validTile.SetHighlight(true, (!GridManager.Instance.GetNodeForPosition(tileUnder).IsOccupied || GridManager.Instance.GetNodeForPosition(tileUnder).worldPosition == oldPosition), tileUnder);

            /*if (previousTile != nullVector && tileUnder != previousTile)
            {
                //We are over a different tile.
                validTile.SetHighlight(false, false, tileUnder);
            }*/

            previousTile = tileUnder;
        }
    }

    public void OnEndDrag()
    {
        if (!IsDragging)
            return;
        
        //Debug.Log(this.name + " end drag");

        if (!TryRelease())
        {
            //Nothing was found, return to original position.
            this.transform.position = oldPosition;
        }

        Vector3 tileUnder = cam.ScreenToWorldPoint(Input.mousePosition);
        tileUnder.z = 0;

        if (previousTile != nullVector)
        {

            validTile.SetHighlight(false, false, tileUnder);
            previousTile = nullVector;
        }

        bodySpriteRenderer.sortingOrder = oldSortingOrderBody;
        helmetSpriteRenderer.sortingOrder = oldSortingOrderGear + 2;
        chestplateSpriteRenderer.sortingOrder = oldSortingOrderGear + 1;
        weaponSpriteRenderer.sortingOrder = oldSortingOrderGear;

        IsDragging = false;
    }

    private bool TryRelease()
    {
        RaycastHit2D hit =
            Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100, releaseMask);
        
        if (hit != null && hit.collider != null)
        {
            //Released over something!
            //Debug.Log("Released over " + hit.collider.name);
            Vector3 t = cam.ScreenToWorldPoint(Input.mousePosition);
            t.z = 0;
            if (t != null)
            {
                //It's a tile
                BaseEntity thisEntity = GetComponent<BaseEntity>();
                Node candidateNode = GridManager.Instance.GetNodeForPosition(t);
                if (candidateNode != null && thisEntity != null)
                {
                    if (!candidateNode.IsOccupied)
                    {

                        thisEntity.CurrentNode.SetOccupied(false);
                        thisEntity.SetCurrentNode(candidateNode);
                        candidateNode.SetOccupied(true);
                        thisEntity.transform.position = candidateNode.worldPosition;

                        return true;
                    }
                }
            }
        }

        return false;
    }
}
