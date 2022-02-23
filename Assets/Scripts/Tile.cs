using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    public SpriteRenderer highlightSprite;
    public Sprite validColor;
    public Sprite wrongColor;
    public Tilemap grid;

    public static Tile Instance;

    protected void Awake()
    {
        Instance = (Tile)this;
    }
    
    public void SetHighlight(bool active, bool valid, Vector3 position)
    {
        highlightSprite.gameObject.SetActive(active);
        transform.position = grid.CellToWorld(grid.WorldToCell(position));

        highlightSprite.sprite = valid ? validColor : wrongColor;
    }
}