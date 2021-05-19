using System;
using System.Collections;
using System.Collections.Generic;
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
    private string _desiredTarget;
    private int _range;
    private int _aoe;

    private Transform _actionMaker;
    public GameObject targetUnit = null;
    public Camera mainCamera;
    private Transform _selectable;
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
        FindEntities(_desiredTarget);
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

    void FindEntities(string entityTag)
    {
        CleanEntities();
        GameObject[] localEntities = GameObject.FindGameObjectsWithTag(entityTag);
        for (var i = 0; i < tiles.Count; i++)
        {
            for (var l = 0; l < localEntities.Length; l++)
            {
                if (chao.LocalToCell(tiles[i]) == chao.LocalToCell(localEntities[l].transform.position))
                {
                    if (foundEntities.Contains(localEntities[l].transform) == false)
                        foundEntities.Add(localEntities[l].transform);
                }
                if (foundEntities != null)
                {
                    foreach (Transform entity in foundEntities)
                    {
                        if (grid.LocalToCell(entity.position) == tiles[i])
                        {
                            //Debug.Log(localEntity.name + " em " + entity.position);
                        }
                    }
                }
            }
        }
    }
   
    private void PaintGrid()
    {
        bool continuePaintingTile = true;
        for (var i = 0; i < tiles.Count; i++) 
        {
            foreach(Vector3 tile in tilesIgnore)
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
    //For quick unsearch
    public void SearchMode(bool boo)
    {
        onSearchMode = boo;
    }

    //Single Search
    public void StartSearchMode(bool boo, int range, Transform actionMaker, string desiredTarget)
    {
        onSearchMode = boo;
        _range = range;
        _actionMaker = actionMaker;
        _desiredTarget = desiredTarget;
    }
    //AOE Search
    public void StartSearchMode(bool boo, int range, Transform actionMaker, int aoe, string desiredTarget)
    {
        onSearchMode = boo;
        _range = range;
        _actionMaker = actionMaker;
        _desiredTarget = desiredTarget;
        _aoe = aoe;
    }
    //Multiple Targets??
    //TODO: Not implemented

    private GameObject targetOnce()
    {
        Debug.LogWarning("Is on target");
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, _range, ~ignorar);
        // -- TODO: IMPORTANT TO IMPLEMENT LATER
        //CameraMoveWithMouse();
        if (_selectable != null)
        {
            if (_selectable.gameObject.tag == _desiredTarget)
                ResetMat(_selectable.gameObject);
            _selectable = null;
        }
        if (hit.collider != null && hit.transform.gameObject.tag == _desiredTarget)
        {
            float distance = Vector2.Distance(_actionMaker.transform.position, hit.collider.transform.position);
            if (hit.collider.tag == _desiredTarget && foundEntities.Contains(hit.collider.transform))
                SelectableOutline(hit.collider.gameObject);
        }
        _selectable = hit.transform;

        if (Input.GetMouseButtonDown(0))
        {
            //-----------PLAY ANIM OF FINDING ENEMY-----------
            if (hit.collider != null)
            {
                Debug.Log("Pressed down on " + hit.transform.gameObject);
                if (_aoe <= 0)
                {
                    if (hit.collider.tag == _desiredTarget && foundEntities.Contains(hit.collider.transform))
                    {
                        //Debug.Log("FoundEntities contains " + hit.collider.gameObject);
                        targetUnit = hit.transform.gameObject;
                        CleanArea();
                        CleanEntities();
                    }
                    
                }
            }
            else
            {
                Debug.Log("Floor: " + hit.point);
                foreach (Transform entity in foundEntities)
                {
                    Debug.Log("Entity: " + chao.LocalToCell(entity.position));

                    if (chao.LocalToCell(hit.point) == chao.LocalToCell(entity.position))
                    {
                        Debug.Log("Floor has " + entity);
                        targetUnit = entity.gameObject;
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


