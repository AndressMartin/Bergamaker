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
    [SerializeField] Vector3 mousePosition;
    public List<Vector3> tiles = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        FindWithMouse();
    }


    void FindWithMouse()
    {
        Vector3 previousMousePosition = mousePosition;
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, +10f);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //Debug.Log(grid.LocalToCell(mousePosition));
        if (grid.LocalToCell(mousePosition) != grid.LocalToCell(previousMousePosition))
        {
            CleanArea();
            GetArea(4, mousePosition);
        }
    }

    private void CleanArea()
    {
        tiles.Clear();
        tileMap.ClearAllTiles();
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
    }

    //private void PaintGrid(int range, Vector3 position)
    //{

    //}
    private void PaintGrid()
    {
        foreach (Vector3 tile in tiles)
        {
            tileMap.SetTile(Vector3Int.FloorToInt(tile), tileBase);
        }
    }
    private void PrintArea()
    {
        foreach (Vector3 tile in tiles)
        {
            //Debug.Log(tile);
        }
    }
}
