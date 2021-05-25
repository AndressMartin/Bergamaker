using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{

    //            -----GRID MANAGEMENT

    public Grid grid;
    public Tilemap tileMap;
    public TileBase tintTile;
    public Tilemap chao;
    public Tilemap paredes;
    public List<Vector3> tiles = new List<Vector3>();
    public List<Vector3> tilesIgnore = new List<Vector3>();
    public List<Transform> foundEntities = new List<Transform>();
    private Transform caster;
    [SerializeField] Vector3 mousePosition;

    //            -----TARGETING
    public bool onSearchMode { get; private set; }
    public bool auto = false; //For self targeting
    public List<string> _desiredTargets = new List<string>();
    private int _range;
    private int _aoe;

    private Transform _actionMaker;
    //For Direct Actions
    public GameObject targetUnit = null;
    //For Actions with multiple possible targets
    public GameObject[] targetUnits = null;
    public Camera mainCamera;
    public List<Transform> _selectable = new List<Transform>();
    public Transform _selectableTarget;
    public LayerMask ignorar;

    public Material targetMat;
    public Material selectMat;
    public Material defaultMat;

    void Start()
    {
        grid = GetComponent<Grid>();
        caster = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (onSearchMode)
        {
            Debug.Log("Entrou no search");
            RangeAroundCaster(_actionMaker, _range);
            PaintGridForFoundEntities();
        }
        else
        {
            CleanArea();
            CleanEntities();
        }
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

        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    Debug.Log(grid.LocalToCell(mousePosition));
        //}
    }

    void RangeAroundCaster(Transform caster, int range)
    {
        //Debug.Log(this.caster.position);
        CleanArea();
        CleanEntities();
        GetArea(range, caster.GetChild(0).position); //Child for GridPoint
    }

    private void CleanArea()
    {
        tiles.Clear();
        tilesIgnore.Clear();
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
        FindWalls();
        FindEntities(_desiredTargets);
        PaintGrid();
        targetOnce();
    }

    private void FindWalls()
    {
        foreach (Vector3 tile in tiles)
        {
            Tile.ColliderType tileType = paredes.GetColliderType(Vector3Int.FloorToInt(tile));
            if (tileType != Tile.ColliderType.None)
            {
                tilesIgnore.Add(tile);
            }
        }
    }

    void FindEntities(List<string> entityTags)
    {
        GameObject[] arr = null; //Temporary array 
        List<GameObject> localEntities = new List<GameObject>();
        CleanEntities();
        foreach (string tag in entityTags)
        {
            arr = GameObject.FindGameObjectsWithTag(tag);
            foreach(var element in arr)
                localEntities.Add(element);
        }
        Array.Clear(arr, 0, arr.Length);
        for (var i = 0; i < tiles.Count; i++)
        {
            for (var l = 0; l < localEntities.Count; l++)
            {
                BoxCollider2D childCollider = localEntities[l].transform.GetChild(0).GetComponent<BoxCollider2D>();
                var V2EntityPos = childCollider.ClosestPoint(tiles[i]);
                if (chao.LocalToCell(tiles[i]) == chao.LocalToCell(V2EntityPos) && foundEntities.Contains(localEntities[l].transform) == false)
                {
                    //if (foundEntities.Contains(localEntities[l].transform.GetChild(0).transform) == false)
                    foundEntities.Add(localEntities[l].transform);
                }
            }
        }
    }

    private void PaintGrid()
    {
        bool continuePaintingTile = true;
        for (var i = 0; i < tiles.Count; i++)
        {
            foreach (Vector3 tile in tilesIgnore)
            {
                if (tiles[i] == tile && continuePaintingTile == true)
                {
                    continuePaintingTile = false;
                }
            }
            if (continuePaintingTile)
                tileMap.SetTile(Vector3Int.FloorToInt(tiles[i]), tintTile);
            else
                continuePaintingTile = true;
        }
    }
    private void SetTileColor(bool boo, Color color, Vector3Int position, Tilemap tilemap)
    {
        // Flag the tile, inidicating that it can change colour.
        // By default it's set to "Lock Color".
        if (boo)
            tilemap.SetTileFlags(position, TileFlags.None);
        else
            tilemap.SetTileFlags(position, TileFlags.LockColor);

        // Set the color.
        tilemap.SetColor(position, color);
    }

    private void PaintGridForFoundEntities()
    {
        if (foundEntities != null)
        {
            for (var i = 0; i < tiles.Count; i++)
            {
                foreach (Transform entity in foundEntities)
                {
                    BoxCollider2D childCollider = entity.transform.GetChild(0).GetComponent<BoxCollider2D>();
                    var V2EntityPos = childCollider.ClosestPoint(tiles[i]);
                    if (grid.LocalToCell(V2EntityPos) == tiles[i] && tileMap.GetTileFlags(Vector3Int.FloorToInt(tiles[i])) == TileFlags.LockColor)
                    {
                        SetTileColor(true, Color.yellow, chao.LocalToCell(V2EntityPos), tileMap);
                        Debug.Log(entity.name + " em " + chao.LocalToCell(V2EntityPos));
                    }
                }
            }
        }
    }

    //For quick unsearch
    public void SearchMode(bool boo)
    {
        onSearchMode = boo;
    }

    //Single Search
    public void StartSearchMode(bool boo, int range, Transform actionMaker, List<string> desiredTargets)
    {
        onSearchMode = boo;
        _range = range;
        _actionMaker = actionMaker;
        _desiredTargets = desiredTargets;
    }
    //AOE Search
    public void StartSearchMode(bool boo, int range, Transform actionMaker, int aoe, List<string> desiredTargets)
    {
        onSearchMode = boo;
        _range = range;
        _actionMaker = actionMaker;
        _desiredTargets = desiredTargets;
        _aoe = aoe;
    }
    //Multiple Targets??
    //TODO: Not implemented

    private GameObject targetOnce()
    {
        //Debug.LogWarning("Is on target");
        Collider2D[] hits = Physics2D.OverlapPointAll(mainCamera.ScreenToWorldPoint(Input.mousePosition), ~ignorar);
        // -- TODO: IMPORTANT TO IMPLEMENT LATER
        //CameraMoveWithMouse();
        if (_selectable != null) //Check if list has stuff (was previously != null)
        {
            foreach (Transform entity in foundEntities)
            {
                Debug.Log(entity);
                if (foundEntities.Contains(entity))
                    ResetMat(entity.gameObject);
                if (entity.gameObject.tag == "Floor")
                    ResetMat(entity.gameObject);
            }
            _selectable.Clear();
        }
        foreach (Collider2D hit in hits)
        {
            if (hit != null && foundEntities.Contains(hit.transform))
            {
                SelectableOutline(hit.gameObject);
                _selectableTarget = hit.transform;

            }
            else if (hit != null && hit.transform.gameObject.tag == "Floor")
            {
                Vector2 V2MousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                foreach (Transform entity in foundEntities)
                {
                    BoxCollider2D childCollider = entity.transform.GetChild(0).GetComponent<BoxCollider2D>();
                    var V2EntityPos = childCollider.ClosestPoint(V2MousePos);
                    if (chao.LocalToCell(V2MousePos) == chao.LocalToCell(V2EntityPos))
                    {
                        if (_selectableTarget != null)
                        {
                            if (Vector2.Distance(V2MousePos, V2EntityPos) < Vector2.Distance(V2MousePos, _selectableTarget.position))
                            {
                                _selectableTarget = entity;
                            }
                            SelectableOutline(_selectableTarget.gameObject);
                            SetTileColor(true, Color.red, chao.LocalToCell(V2MousePos), tileMap);
                        }
                        //Debug.LogWarning("Should be highlighting " + entity.name);
                    }
                }
            }
            if (_selectableTarget != null && _selectable.Contains(_selectableTarget) == false) 
                _selectable.Add(_selectableTarget);
        }

        if (Input.GetMouseButtonDown(0))
        {
            foreach (Collider2D hit in hits)
            {
                if (hit != null)
                {
                    Debug.Log("Pressed down on " + hit.transform.gameObject);
                    if (_aoe <= 0)
                    {
                        if (foundEntities.Contains(hit.transform))
                        {
                            //Debug.Log("FoundEntities contains " + hit.collider.gameObject);
                            targetUnit = hit.gameObject;
                            CleanArea();
                            CleanEntities();
                        }
                        else if (hit.tag == "Floor")
                        {
                            Vector2 V2MousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                            //Debug.Log("Floor: " + chao.LocalToCell(V2MousePos));
                            foreach (Transform entity in foundEntities)
                            {
                                BoxCollider2D childCollider = entity.transform.GetChild(0).GetComponent<BoxCollider2D>();
                                var V2EntityPos = childCollider.ClosestPoint(V2MousePos);
                                //Debug.Log("Entity: " + chao.LocalToCell(entity.position));

                                if (chao.LocalToCell(V2MousePos) == chao.LocalToCell(V2EntityPos))
                                {
                                    //Debug.Log("Floor has " + entity);
                                    targetUnit = entity.gameObject;
                                }
                            }
                        }

                    }
                }

            }
        }

        return targetUnit;
    }
    public void ResetParams()
    {
        onSearchMode = false;
        _range = 0;
        _actionMaker = null;
        _aoe = 0;
        targetUnit = null;
    }
    GameObject RemovePreviousAutoSelection(GameObject previousSelection)
    {
        ResetMat(previousSelection);
        return null;
    }
    public void TargetedOutline(GameObject _obj)
    {
        if (_obj != null && _obj.GetComponent<SpriteRenderer>())
        {
            _obj.GetComponent<SpriteRenderer>().material = targetMat;
        }
    }
    public void SelectableOutline(GameObject _obj)
    {
        if (_obj != null && _obj.GetComponent<SpriteRenderer>())
        {
            _obj.GetComponent<SpriteRenderer>().material = selectMat;
        }

    }
    public void ResetMat(GameObject _obj)
    {
        if (_obj != null && _obj.GetComponent<SpriteRenderer>())
        {
            _obj.GetComponent<SpriteRenderer>().material = defaultMat;
        }
    }


}


