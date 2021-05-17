using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{

    public Grid grid;
    public Tilemap tileMap;
    public TileBase tileBase;
    public Tilemap chao;
    public Tilemap paredes;
    public List<Vector3> tiles = new List<Vector3>();
    public List<Transform> foundEntities = new List<Transform>();
    private Transform caster;
    [SerializeField] Vector3 mousePosition;
    
    void Start()
    {
        grid = GetComponent<Grid>();
        caster = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        ShowRangeWithMouse(2);
        //RangeAroundCaster(caster, 2);
    }

    void ShowRangeWithMouse(int area)
    {
        Vector3 previousMousePosition = mousePosition;
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, +10f);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        if (grid.LocalToCell(mousePosition) != grid.LocalToCell(previousMousePosition))
        {
            CleanArea();
            GetArea(area, mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log(grid.LocalToCell(mousePosition));
        }
    }

    void RangeAroundCaster(Transform caster, int area)
    {
        Debug.Log(this.caster.position);
        CleanArea();
        CleanEntities();
        GetArea(area, caster.position);
    }

    private void CleanArea()
    {
        tiles.Clear();
        tileMap.ClearAllTiles();
    }

    private void CleanEntities()
    {
        foundEntities.Clear();
    }

    void GetArea(int range, Vector3 position)
    {
        Vector3 center = grid.LocalToCell(position);
        tiles.Add(center);
        for (int i = 0; i < range; i++)
        {
            tiles.Add(new Vector3(center.x - (i + 1), center.y, center.z));
            tiles.Add(new Vector3(center.x + (i + 1), center.y, center.z));
            tiles.Add(new Vector3(center.x, center.y - (i + 1), center.z));
            tiles.Add(new Vector3(center.x, center.y + (i + 1), center.z));
            if (i > 0 && i < range)
            {
                for (int f = 1; f <= i; f++)
                {
                    tiles.Add(new Vector3(center.x + (i + 1) - (f), center.y - (f), center.z));
                    tiles.Add(new Vector3(center.x + (i + 1) - (f), center.y + (f), center.z));
                    tiles.Add(new Vector3(center.x - (i + 1) + (f), center.y - (f), center.z));
                    tiles.Add(new Vector3(center.x - (i + 1) + (f), center.y + (f), center.z));
                }
            }
        }
        PaintGrid();
        FindEntities("Player");
        FindWalls();
    }

    private void FindWalls()
    {
        foreach (Vector3 tile in tiles)
        {
            Tile.ColliderType tileType = paredes.GetColliderType(Vector3Int.FloorToInt(tile));
            if (tileType == Tile.ColliderType.None)
            {
                Debug.Log("Parede em " + paredes.LocalToCell(tile));
            }
        }
    }

    void FindEntities(string entityTag)
    {
        CleanEntities();
        GameObject localEntity = GameObject.FindGameObjectWithTag(entityTag);
        foreach(Vector3 tile in tiles)
        {
            if (chao.LocalToCell(tile) == chao.LocalToCell(localEntity.transform.position))
            {
                if (foundEntities.Contains(localEntity.transform) == false)
                    foundEntities.Add(localEntity.transform);
            }
            if (foundEntities != null)
            {
                foreach (Transform entity in foundEntities)
                {
                    if (grid.LocalToCell(entity.position) == tile)
                    {
                        Debug.Log(localEntity.name + " em " + entity.position);
                    }
                }
            }
        }
    }
   
    private void PaintGrid()
    {
        foreach (Vector3 tile in tiles)
        {
            tileMap.SetTile(Vector3Int.FloorToInt(tile), tileBase);
        }
    }
}
