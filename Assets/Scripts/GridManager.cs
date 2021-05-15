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
    public List<Vector3> tiles = new List<Vector3>();
    public static List<Transform> foundEntities = new List<Transform>();
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
        GetArea(area, caster.position);
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

    void Foo()
    {
        Debug.Log("Foo");
        foreach(Vector3 tile in tiles)
        {
            foreach (Transform entity in foundEntities)
            {
                if (grid.LocalToCell(entity.position) == tile)
                {
                    Debug.Log(entity.position);
                }
            }
        }
    }
   
    private void PaintGrid()
    {
        GameObject localPlayer = GameObject.FindGameObjectWithTag("Player");

        foreach (Vector3 tile in tiles)
        {
            tileMap.SetTile(Vector3Int.FloorToInt(tile), tileBase);

            if (chao.LocalToCell(tile) == chao.LocalToCell(localPlayer.transform.position))
            {
                foundEntities.Add(localPlayer.transform);
            }
        }
        if (foundEntities != null)
            Foo();
    }

    //                --FOR DEBUGGING--
    //
    /*
    private void PrintArea()
    {
        foreach (Vector3 tile in tiles)
        {
            //Debug.Log(tile);
        }
    }
    */
}
