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
    public Tilemap tileMapRange;
    public Tilemap tileMapAoe;
    public TileBase tintTile;
    public Tilemap chao;
    public Tilemap paredes;
    public List<Vector3> tilesRange = new List<Vector3>();
    public List<Vector3> tilesAoe = new List<Vector3>();
    public List<Vector3> tilesFull = new List<Vector3>();
    public List<Vector3> tilesIgnore = new List<Vector3>();
    public List<Transform> foundEntities = new List<Transform>();
    public List<Transform> foundObjects = new List<Transform>();
    public List<Transform> foundTerrain = new List<Transform>();
    [SerializeField] Vector3 mousePosition;

    //            -----TARGETING
    public bool onSearchMode { get; private set; }
    public bool auto = false; //For self targeting
    public List<string> _desiredTargets = new List<string>();
    public int _range;
    public int _aoe;
    public Shapes _shapeType;
    public int _targetsNum;
    public int timesTargetWasSent;
    public bool _multiTargetsOnly;

    private Transform _actionMaker;
    //For Direct Actions
    public GameObject targetUnit = null;
    //For Actions with multiple possible targets
    public List<GameObject> targetUnits = new List<GameObject>();
    public Camera mainCamera;
    public List<Transform> _selectable = new List<Transform>();
    public Transform _selectableTarget;
    public LayerMask ignorar;

    public Material targetMat;
    public Material selectMat;
    public Material defaultMat;

    void Start()
    {
        Debug.Log(_shapeType);
        grid = GetComponent<Grid>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (onSearchMode)
        {
            Debug.Log("Entrou no search");
            CleanEntities();
            CasterRange(_actionMaker, _range, tilesRange);
            FindWalls(tilesRange);
            FillGrid(tilesRange, tileMapRange);
            if (_aoe > 0)
            {
                MouseRange(_aoe, tilesAoe);
                FindWalls(tilesAoe);
                FillGrid(tilesAoe, tileMapAoe);
                PaintGridForAOE();
            }
            FindEntities(_desiredTargets);
            //FindObjects();
            //FindTerrain();
            PaintGridForFoundEntities();
            RemoveAndDeselectTargetsOutOfRange();
            //Handle selection
            Collider2D[] selection = StartSelection();

            if (Input.GetMouseButtonDown(0))
            {
                if (selection != null)
                {
                    if (_aoe > 0)
                    {
                        targetAoe(selection);
                    }
                    else
                    {
                        targetOnce(selection);
                    }
                }
            }
        }
        else
        {
            CleanAllAreas();
            CleanEntities();
            CleanSelection(selectMat);
        }
    }



    void CasterRange(Transform caster, int range, List<Vector3> _tiles)
    {
        CleanArea(tilesFull, tileMapRange);
        CleanArea(_tiles, tileMapRange);
        GetArea(range, caster.GetChild(0).position, _tiles, _shapeType);
    }
    void MouseRange(int area, List<Vector3> _tiles)
    {
        Vector3 previousMousePosition = mousePosition;
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, +10f);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (grid.LocalToCell(mousePosition) != grid.LocalToCell(previousMousePosition))
        {
            if (tilesRange.Contains(grid.LocalToCell(mousePosition)))
            {
                CleanArea(_tiles, tileMapAoe);
                GetArea(area, mousePosition, _tiles, _shapeType);
            }
        }
    }

    internal bool Any()
    {
        throw new NotImplementedException();
    }

    private void CleanArea(List<Vector3> _tiles, Tilemap _tilemap)
    {
        _tiles.Clear();
        tilesIgnore.Clear();
        _tilemap.ClearAllTiles();
    }
    private void CleanAllAreas()
    {
        tilesRange.Clear();
        tilesAoe.Clear();
        tilesFull.Clear();
        tilesIgnore.Clear();
        tileMapRange.ClearAllTiles();
        tileMapAoe.ClearAllTiles();
    }
    private void CleanEntities()
    {
        foundEntities.Clear();
    }

    Vector3 Coordinates(int iteration, int i, Vector3 center)
    {
        if (iteration == 1) return new Vector3(center.x - (i + 1), center.y, center.z);
        if (iteration == 2) return new Vector3(center.x + (i + 1), center.y, center.z);
        if (iteration == 3) return new Vector3(center.x, center.y - (i + 1), center.z);
        if (iteration == 4) return new Vector3(center.x, center.y + (i + 1), center.z);
        else return new Vector3(0, 0, 0);
    }
    Vector3 Coordinates(int iteration, int i, int f, Vector3 center)
    {
        if (iteration == 1) return new Vector3(center.x + (i + 1) - (f), center.y - (f), center.z);
        if (iteration == 2) return new Vector3(center.x + (i + 1) - (f), center.y + (f), center.z);
        if (iteration == 3) return new Vector3(center.x - (i + 1) + (f), center.y - (f), center.z);
        if (iteration == 4) return new Vector3(center.x - (i + 1) + (f), center.y + (f), center.z);
        else return new Vector3(0, 0, 0);
    }
    void GetArea(int range, Vector3 position, List<Vector3> _tiles, Shapes shapeType)
    {
        Vector3 center = grid.LocalToCell(position);
        _tiles.Add(center);
        if (shapeType == Shapes.Area)
        {
            for (int i = 0; i < range; i++)
            {
                for (int iteration = 1; iteration < 5; iteration++)
                {
                    if (!tilesFull.Contains(Coordinates(iteration, i, center)))
                        _tiles.Add(Coordinates(iteration, i, center));
                }
                if (i > 0 && i < range)
                {
                    for (int f = 1; f <= i; f++)
                    {
                        for (int iteration = 1; iteration < 5; iteration++)
                        {
                            if (!tilesFull.Contains(Coordinates(iteration, i, f, center)))
                                _tiles.Add(Coordinates(iteration, i, f, center));
                        }
                    }
                }
            }
        }
        if (shapeType == Shapes.Cone)
        {
            Debug.Log("Cry");
        }
        if (shapeType == Shapes.Line)
        {
            Debug.Log("Cry less");
        }
    }

    private void FindWalls(List<Vector3> _tiles)
    {
        foreach (Vector3 tile in _tiles)
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
        tilesFull.AddRange(tilesRange);
        tilesFull.AddRange(tilesAoe);
        GameObject[] arr = null; //Temporary array 
        List<GameObject> localEntities = new List<GameObject>();
        CleanEntities();
        foreach (string tag in entityTags)
        {
            arr = GameObject.FindGameObjectsWithTag(tag);
            foreach(var element in arr) localEntities.Add(element);
        }
        Array.Clear(arr, 0, arr.Length);
        for (var i = 0; i < tilesFull.Count; i++)
        {
            for (var l = 0; l < localEntities.Count; l++)
            {
                BoxCollider2D childCollider = localEntities[l].transform.GetChild(0).GetComponent<BoxCollider2D>();
                var V2EntityPos = childCollider.ClosestPoint(tilesFull[i]);
                if (chao.LocalToCell(tilesFull[i]) == chao.LocalToCell(V2EntityPos) && foundEntities.Contains(localEntities[l].transform) == false)
                {
                    foundEntities.Add(localEntities[l].transform);
                }
            }
        }
    }

    private void FindObjects()
    {
        throw new NotImplementedException();
    }
    private void FindTerrain()
    {
        throw new NotImplementedException();
    }

    private void FillGrid(List<Vector3> _tiles, Tilemap _tilemap)
    {
        bool continuePaintingTile = true;
        for (var i = 0; i < _tiles.Count; i++)
        {
            foreach (Vector3 tile in tilesIgnore)
            {
                if (_tiles[i] == tile && continuePaintingTile == true)
                {
                    continuePaintingTile = false;
                }
            }
            if (continuePaintingTile)
                _tilemap.SetTile(Vector3Int.FloorToInt(_tiles[i]), tintTile);
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
            for (var i = 0; i < tilesFull.Count; i++)
            {
                foreach (Transform entity in foundEntities)
                {
                    BoxCollider2D childCollider = entity.transform.GetChild(0).GetComponent<BoxCollider2D>();
                    var V2EntityPos = childCollider.ClosestPoint(tilesFull[i]);
                    if (grid.LocalToCell(V2EntityPos) == tilesFull[i] && tileMapRange.GetTileFlags(Vector3Int.FloorToInt(tilesFull[i])) == TileFlags.LockColor)
                    {
                        SetTileColor(true, Color.yellow, chao.LocalToCell(V2EntityPos), tileMapRange);
                    }
                }
            }
        }
    }

    private void PaintGridForAOE()
    {
        foreach(Vector3 tile in tilesAoe)
        {
            SetTileColor(true, Color.red, Vector3Int.FloorToInt(tile), tileMapAoe);
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
    public void StartSearchMode(bool boo, int range, Transform actionMaker, bool multiTargetsOnly, List<string> desiredTargets)
    {
        onSearchMode = boo;
        _range = range;
        _actionMaker = actionMaker;
        _desiredTargets = desiredTargets;
        _multiTargetsOnly = multiTargetsOnly;
    }
    //AOE Search
    public void StartSearchMode(bool boo, int range, Transform actionMaker, int aoe, Shapes shapeType, List<string> desiredTargets)
    {
        onSearchMode = boo;
        _range = range;
        _actionMaker = actionMaker;
        _desiredTargets = desiredTargets;
        _aoe = aoe;
        _shapeType = shapeType;
    }
    //Multiple Targets??
    //TODO: Not implemented
    private Collider2D[] StartSelection()
    {
        //Debug.LogWarning("Is on target");
        Collider2D[] hits = Physics2D.OverlapPointAll(mainCamera.ScreenToWorldPoint(Input.mousePosition), ~ignorar);
        CleanSelection(selectMat);
        if (_aoe > 0) return SelectInsideAoe();
        else SelectTarget(hits);
        return hits;
    }

    private void SelectTarget(Collider2D[] hits)
    {
        foreach (Collider2D hit in hits)
        {
            if (hit != null && foundEntities.Contains(hit.transform))
            {
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
                        }
                        else
                        {
                            _selectableTarget = entity;
                        }
                        SetTileColor(true, Color.red, chao.LocalToCell(V2MousePos), tileMapRange);
                    }
                }
            }
            if (_selectableTarget != null && _selectable.Contains(_selectableTarget) == false)
            {
                if (_multiTargetsOnly) //If the same target cannot be selected many times
                {
                    if (targetUnits.Contains(_selectableTarget.gameObject) == false)
                    {
                        SelectableOutline(_selectableTarget.gameObject);
                        _selectable.Add(_selectableTarget);
                    }
                }
                else
                {
                    SelectableOutline(_selectableTarget.gameObject);
                    _selectable.Add(_selectableTarget);
                }
            }
        }
    }

    private Collider2D[] SelectInsideAoe()
    {
        Collider2D[] hits = null;
        for (var i = 0; i < tilesAoe.Count; i++)
        {
            foreach (Transform entity in foundEntities)
            {
                BoxCollider2D childCollider = entity.transform.GetChild(0).GetComponent<BoxCollider2D>();
                var V2EntityPos = childCollider.ClosestPoint(tilesAoe[i]);
                if (tilesAoe.Contains(grid.LocalToCell(V2EntityPos)) && _selectable.Contains(entity) != true)
                {
                    _selectable.Add(entity);
                }
            }
        }
        List<Collider2D> tempList = new List<Collider2D>();
        for (int i = 0; i < _selectable.Count; i++)
        {
            SelectableOutline(_selectable[i].gameObject);
            tempList.Add(_selectable[i].gameObject.GetComponent<Collider2D>());
            //Debug.LogWarning($"_SELECTABLE IS {0} AND ITS COLLIDER IS {1}" + i + _selectable[i].gameObject.GetComponent<Collider2D>());
        }
        hits = tempList.ToArray();
        return hits;
    }


    private List<GameObject> targetOnce(Collider2D[] hits)
    {
        foreach (Collider2D hit in hits)
        {
            if (hit != null)
            {
                if (foundEntities.Contains(hit.transform))
                {
                    targetUnit = hit.gameObject;
                    CleanAllAreas();
                    CleanEntities();
                }
                else if (hit.tag == "Floor")
                {
                    Vector2 V2MousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    foreach (Transform entity in foundEntities)
                    { 
                        BoxCollider2D childCollider = entity.transform.GetChild(0).GetComponent<BoxCollider2D>();
                        var V2EntityPos = childCollider.ClosestPoint(V2MousePos);

                        if (chao.LocalToCell(V2MousePos) == chao.LocalToCell(V2EntityPos))
                        {
                            targetUnit = entity.gameObject;
                        }
                    }
                }
            }
        }
        if (_multiTargetsOnly) //If the same target cannot be selected many times
        {
            if (targetUnits.Contains(targetUnit) != true)
            {
                targetUnits.Add(targetUnit);
                TargetedOutline(targetUnit);
                timesTargetWasSent++;
            }
        }
        else
        {
            targetUnits.Add(targetUnit);
            TargetedOutline(targetUnit);
            timesTargetWasSent++;
        }

        return targetUnits;
    }

    private List<GameObject> targetAoe(Collider2D[] hits)
    {
        foreach (Collider2D hit in hits)
        {
            Debug.Log(hit.gameObject);
            var hitObject = hit.gameObject;
            targetUnits.Add(hitObject);
            //targetUnits.Add(hit.GetComponentInParent<Transform>().gameObject);
        }
        return targetUnits;
    }

    private void RemoveAndDeselectTargetsOutOfRange()
    {
        foreach(GameObject unit in targetUnits)
        {
            if (foundEntities.Contains(unit.transform) != true)
            {
                targetUnits.Remove(unit);
                timesTargetWasSent--;
                ResetMat(unit);
            }
        }
    }

    public void ResetParams()
    {
        onSearchMode = false;
        _range = 0;
        _actionMaker = null;
        _aoe = 0;
        targetUnit = null;
        targetUnits.Clear();
        timesTargetWasSent = 0;
    }

    private void CleanSelection(Material materialToRemove)
    {
        if (_selectable != null)
        {
            foreach (Transform entity in _selectable)
            {
                if (entity.gameObject.GetComponent<SpriteRenderer>().sharedMaterial == materialToRemove)
                    ResetMat(entity.gameObject);
            }
            _selectable.Clear();
            _selectableTarget = null;
        }
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
    public void TargetedOutline(List<GameObject> _objs)
    {
        foreach(GameObject _obj in _objs)
        {
            if (_obj != null && _obj.GetComponent<SpriteRenderer>())
            {
                _obj.GetComponent<SpriteRenderer>().material = targetMat;
            }
        }
    }
    public void SelectableOutline(GameObject _obj)
    {
        if (_obj != null && _obj.GetComponent<SpriteRenderer>())
        {
            _obj.GetComponent<SpriteRenderer>().material = selectMat;
        }

    }
    public void SelectableOutline(List<GameObject> _objs)
    {
        foreach (GameObject _obj in _objs)
        {
            if (_obj != null && _obj.GetComponent<SpriteRenderer>())
            {
                _obj.GetComponent<SpriteRenderer>().material = selectMat;
            }
        }

    }
    public void ResetMat(GameObject _obj)
    {
        if (_obj != null && _obj.GetComponent<SpriteRenderer>())
        {
            _obj.GetComponent<SpriteRenderer>().material = defaultMat;
        }
    }
    public void ResetMat(List<GameObject> _objs)
    {
        foreach (GameObject _obj in _objs)
        {
            if (_obj != null && _obj.GetComponent<SpriteRenderer>())
            {
                _obj.GetComponent<SpriteRenderer>().material = defaultMat;
            }
        }
    }
}


