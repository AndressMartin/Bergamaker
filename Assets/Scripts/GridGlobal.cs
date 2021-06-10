//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEditor.Tilemaps;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//public class GridGlobal : MonoBehaviour
//{

//    //            -----GRID MANAGEMENT

//    public Grid grid;
//    public Tilemap tileMapRange;
//    public Tilemap tileMapAoe;
//    public TileBase tintTile;
//    public Tilemap chao;
//    public Tilemap paredes;
//    public Tilemap buracos;
//    public Tilemap EffectsMap;
//    public List<Vector3> tilesRange = new List<Vector3>();
//    public List<Vector3> tilesAoe = new List<Vector3>();
//    public List<Vector3> tilesFull = new List<Vector3>();
//    public List<Vector3> tilesIgnore = new List<Vector3>();
//    public List<Transform> foundEntities = new List<Transform>();
//    public List<Transform> foundObjects = new List<Transform>();
//    public List<Transform> foundTerrain = new List<Transform>();
//    [SerializeField] Vector3 mousePosition;
//    private Vector3 previousCasterPosition;
//    private Transform arrow;
//    private bool canUpdate;

//    //public Material targetMat;
//    //public Material selectMat;
//    //public Material defaultMat;

//    void Start()
//    {
//        Debug.Log(_shapeType);
//        grid = FindObjectOfType<Grid>();
//        tileMapRange = grid.transform.Find("SelectionRange").GetComponent<Tilemap>();
//        tileMapAoe = grid.transform.Find("SelectionAoe").GetComponent<Tilemap>();
//        chao = grid.transform.Find("Chao").GetComponent<Tilemap>();
//        paredes = grid.transform.Find("Paredes").GetComponent<Tilemap>();
//        buracos = grid.transform.Find("Buracos").GetComponent<Tilemap>();
//        EffectsMap = grid.transform.Find("TerrainEffects").GetComponent<Tilemap>();
//        tintTile = Resources.Load<TileBase>("Tiles/Grid/whiteblock");
//        mainCamera = Camera.main;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        TargetingLoop();
//    }

//    public void TargetingLoop()
//    {
//        if (onSearchMode)
//        {
//            Debug.Log("Entrou no search");
//            CleanEntities();
//            CasterRange(_actionMaker, _range, tilesRange);
//            FindWalls(tilesRange);
//            FillGrid(tilesRange, tileMapRange, tintTile);
//            if (_AOE > 1)
//            {
//                MouseRange(_AOE, tilesAoe);
//                FindWalls(tilesAoe);
//                FillGrid(tilesAoe, tileMapAoe, tintTile);
//                PaintGridForAOE(tilesAoe);
//            }
//            if (_AOE == 1)
//            {
//                CleanArea(tilesAoe, tileMapAoe);
//                tilesAoe = tilesRange.ToList();
//                FillGrid(tilesAoe, tileMapAoe, tintTile);
//                PaintGridForAOE(tilesAoe);
//            }
//            FindEntities(_desiredTargets);
//            if (_isAuto) TargetFirstFoundEntity();
//            //FindObjects();
//            //FindTerrain();
//            PaintGridForFoundEntities();
//            //RemoveAndDeselectTargetsOutOfRange();
//            //Handle selection
//            Collider2D[] selection = StartSelectionWithMouse();
//            if (_AOE == 1)
//            {
//                selection = null;
//                selection = StartSelectionForRange();
//            }
//            if (Input.GetMouseButtonDown(0))
//            {
//                Debug.Log("Clicking");
//                if (selection != null)
//                {
//                    Debug.Log("Selection Not Null");
//                    pointClicked = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, +10f));
//                    if (_AOE > 0)
//                    {
//                        targetAoe(selection);
//                    }
//                    else
//                    {
//                        Debug.Log("Targetting Once");
//                        targetOnce(selection);
//                    }
//                }
//            }
//        }
//        else
//        {
//            CleanAllAreas();
//            CleanEntities();
//            CleanSelection(Color.yellow);
//        }
//    }









//    private void CleanAllAreas()
//    {
//        tilesRange.Clear();
//        tilesAoe.Clear();
//        tilesFull.Clear();
//        tilesIgnore.Clear();
//        tileMapRange.ClearAllTiles();
//        tileMapAoe.ClearAllTiles();
//    }
//    private void CleanEntities()
//    {
//        foundEntities.Clear();
//    }

//    Vector3 Coordinates(int iteration, int i, Vector3 center)
//    {
//        if (iteration == 1) return new Vector3(center.x - (i + 1), center.y, center.z);
//        if (iteration == 2) return new Vector3(center.x + (i + 1), center.y, center.z);
//        if (iteration == 3) return new Vector3(center.x, center.y - (i + 1), center.z);
//        if (iteration == 4) return new Vector3(center.x, center.y + (i + 1), center.z);
//        else return new Vector3(0, 0, 0);
//    }
//    Vector3 Coordinates(int iteration, int i, int f, Vector3 center)
//    {
//        if (iteration == 1) return new Vector3(center.x + (i + 1) - (f), center.y - (f), center.z);
//        if (iteration == 2) return new Vector3(center.x + (i + 1) - (f), center.y + (f), center.z);
//        if (iteration == 3) return new Vector3(center.x - (i + 1) + (f), center.y - (f), center.z);
//        if (iteration == 4) return new Vector3(center.x - (i + 1) + (f), center.y + (f), center.z);
//        else return new Vector3(0, 0, 0);
//    }
//    void GetArea(int range, Vector3 position, List<Vector3> _tiles, Shapes shapeType)
//    {
//        if (shapeType == Shapes.Area)
//        {
//            Vector3 center = grid.LocalToCell(position);
//            centerOfAOE = grid.GetCellCenterWorld(Vector3Int.FloorToInt(center));
//            _tiles.Add(center);
//            for (int i = 0; i < range; i++)
//            {
//                for (int iteration = 1; iteration < 5; iteration++)
//                {
//                    Debug.Log("Painting");
//                    if (!tilesFull.Contains(Coordinates(iteration, i, center)))
//                        _tiles.Add(Coordinates(iteration, i, center));
//                }
//                if (i > 0 && i < range)
//                {
//                    for (int f = 1; f <= i; f++)
//                    {
//                        Debug.Log("Painting");
//                        for (int iteration = 1; iteration < 5; iteration++)
//                        {
//                            if (!tilesFull.Contains(Coordinates(iteration, i, f, center)))
//                                _tiles.Add(Coordinates(iteration, i, f, center));
//                        }
//                    }
//                }
//            }
//        }
//        if (shapeType == Shapes.Cone)
//        {
//            Debug.Log("Cry");
//        }
//        if (shapeType == Shapes.Line)
//        {
//            List<float> lastCoordinates = new List<float>();
//            if (lastCoordinates != _actionMaker.GetComponent<Movement>().lastCoordinates)
//            {
//                lastCoordinates = _actionMaker.GetComponent<Movement>().lastCoordinates;
//                canUpdate = true;
//            }
//            var center = grid.LocalToCell(_actionMaker.transform.GetChild(0).position);
//            if (lastCoordinates[0] != 0)
//            {
//                for (int i = 0; i < range; i++)
//                {
//                    _tiles.Add(new Vector3(center.x + ((i + 1) * lastCoordinates[0]), center.y, center.z));
//                }
//            }
//            else if (lastCoordinates[1] != 0)
//            {
//                for (int i = 0; i < range; i++)
//                {
//                    _tiles.Add(new Vector3(center.x, center.y + ((i + 1) * lastCoordinates[1]), center.z));
//                }
//            }
//        }
//    }

//    private void FindWalls(List<Vector3> _tiles)
//    {
//        foreach (Vector3 tile in _tiles)
//        {
//            Tile.ColliderType tileType = paredes.GetColliderType(Vector3Int.FloorToInt(tile));
//            if (tileType != Tile.ColliderType.None)
//            {
//                tilesIgnore.Add(tile);
//            }
//            Tile.ColliderType tileType2 = buracos.GetColliderType(Vector3Int.FloorToInt(tile));
//            if (tileType2 != Tile.ColliderType.None)
//            {
//                tilesIgnore.Add(tile);
//            }
//        }
//    }

//    void FindEntities(List<string> entityTags)
//    {
//        tilesFull.AddRange(tilesRange);
//        tilesFull.AddRange(tilesAoe);
//        GameObject[] arr = null; //Temporary array 
//        List<GameObject> localEntities = new List<GameObject>();
//        CleanEntities();
//        foreach (string tag in entityTags)
//        {
//            arr = GameObject.FindGameObjectsWithTag(tag);
//            foreach (var element in arr) localEntities.Add(element);
//        }
//        Array.Clear(arr, 0, arr.Length);
//        for (var i = 0; i < tilesFull.Count; i++)
//        {
//            for (var l = 0; l < localEntities.Count; l++)
//            {
//                BoxCollider2D childCollider = localEntities[l].transform.GetChild(0).GetComponent<BoxCollider2D>();
//                var V2EntityPos = childCollider.ClosestPoint(tilesFull[i]);
//                if (chao.LocalToCell(tilesFull[i]) == chao.LocalToCell(V2EntityPos) && foundEntities.Contains(localEntities[l].transform) == false)
//                {
//                    foundEntities.Add(localEntities[l].transform);
//                }
//            }
//        }
//    }

//    private void FindObjects()
//    {
//        throw new NotImplementedException();
//    }
//    private void FindTerrain()
//    {
//        throw new NotImplementedException();
//    }

//    private void FillGrid(List<Vector3> _tiles, Tilemap _tilemap, TileBase tileBase)
//    {
//        bool continuePaintingTile = true;
//        for (var i = 0; i < _tiles.Count; i++)
//        {
//            foreach (Vector3 tile in tilesIgnore)
//            {
//                if (_tiles[i] == tile && continuePaintingTile == true)
//                {
//                    continuePaintingTile = false;
//                }
//            }
//            if (continuePaintingTile)
//                _tilemap.SetTile(Vector3Int.FloorToInt(_tiles[i]), tileBase);
//            else
//                continuePaintingTile = true;
//        }
//    }
//    private void SetTileColor(bool boo, Color color, Vector3Int position, Tilemap tilemap)
//    {
//        // Flag the tile, inidicating that it can change colour.
//        // By default it's set to "Lock Color".
//        if (boo)
//            tilemap.SetTileFlags(position, TileFlags.None);
//        else
//            tilemap.SetTileFlags(position, TileFlags.LockColor);

//        // Set the color.
//        tilemap.SetColor(position, color);
//    }

//    private void PaintGridForFoundEntities()
//    {
//        if (foundEntities != null)
//        {
//            for (var i = 0; i < tilesFull.Count; i++)
//            {
//                foreach (Transform entity in foundEntities)
//                {
//                    BoxCollider2D childCollider = entity.transform.GetChild(0).GetComponent<BoxCollider2D>();
//                    var V2EntityPos = childCollider.ClosestPoint(tilesFull[i]);
//                    if (grid.LocalToCell(V2EntityPos) == tilesFull[i] && tileMapRange.GetTileFlags(Vector3Int.FloorToInt(tilesFull[i])) == TileFlags.LockColor)
//                    {
//                        SetTileColor(true, Color.yellow, chao.LocalToCell(V2EntityPos), tileMapRange);
//                    }
//                }
//            }
//        }
//    }

//    private void PaintGridForAOE(List<Vector3> _tiles)
//    {
//        foreach (Vector3 tile in _tiles)
//        {
//            SetTileColor(true, Color.red, Vector3Int.FloorToInt(tile), tileMapAoe);
//        }
//    }

//    //For quick unsearch
//    public void SearchMode(bool boo)
//    {
//        onSearchMode = boo;
//    }

//    //Single Search
//    public void StartSearchMode(bool boo, int range, Transform actionMaker, List<string> desiredTargets)
//    {
//        onSearchMode = boo;
//        _range = range;
//        _actionMaker = actionMaker;
//        _desiredTargets = desiredTargets;
//    }
//    public void StartSearchMode(bool boo, int range, Transform actionMaker, bool multiTargetsOnly, List<string> desiredTargets)
//    {
//        onSearchMode = boo;
//        _range = range;
//        _actionMaker = actionMaker;
//        _desiredTargets = desiredTargets;
//        _multiTargetsOnly = multiTargetsOnly;
//    }
//    //AOE Search
//    public void StartSearchMode(bool boo, int range, Transform actionMaker, int aoe, bool HasAOEEffect, Shapes shapeType, List<string> desiredTargets)
//    {
//        onSearchMode = boo;
//        _range = range;
//        _actionMaker = actionMaker;
//        _desiredTargets = desiredTargets;
//        _AOE = aoe;
//        _shapeType = shapeType;
//        _HasAOEEffect = HasAOEEffect;
//    }

//    internal void StartSearchMode(bool boo, int range, Transform actionMaker, List<string> desiredTargets, bool isAuto)
//    {
//        onSearchMode = boo;
//        _range = range;
//        _actionMaker = actionMaker;
//        _desiredTargets = desiredTargets;
//        _isAuto = isAuto;
//    }
//    //Multiple Targets??
//    //TODO: Not implemented
//    private Collider2D[] StartSelectionWithMouse()
//    {
//        if (_AOE == 1) return null;
//        //Debug.LogWarning("Is on target");
//        Collider2D[] hits = Physics2D.OverlapPointAll(mainCamera.ScreenToWorldPoint(Input.mousePosition), ~ignorar);
//        CleanSelection(Color.yellow);
//        if (_AOE > 1) return SelectInsideAoe();
//        else SelectTarget(hits);
//        return hits;
//    }

//    private Collider2D[] StartSelectionForRange()
//    {
//        List<Collider2D> entityColliders = new List<Collider2D>();
//        foreach (var entity in foundEntities)
//        {
//            entityColliders.Add(entity.GetComponent<Collider2D>());
//        }
//        Collider2D[] hits = entityColliders.ToArray();
//        return hits;
//    }
//    private void SelectTarget(Collider2D[] hits)
//    {
//        foreach (Collider2D hit in hits)
//        {
//            if (hit != null && foundEntities.Contains(hit.transform))
//            {
//                _selectableTarget = hit.transform;

//            }
//            else if (hit != null && hit.transform.gameObject.tag == "Floor")
//            {
//                Vector2 V2MousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
//                foreach (Transform entity in foundEntities)
//                {
//                    BoxCollider2D childCollider = entity.transform.GetChild(0).GetComponent<BoxCollider2D>();
//                    var V2EntityPos = childCollider.ClosestPoint(V2MousePos);
//                    if (chao.LocalToCell(V2MousePos) == chao.LocalToCell(V2EntityPos))
//                    {
//                        if (_selectableTarget != null)
//                        {
//                            if (Vector2.Distance(V2MousePos, V2EntityPos) < Vector2.Distance(V2MousePos, _selectableTarget.position))
//                            {
//                                _selectableTarget = entity;
//                            }
//                        }
//                        else
//                        {
//                            _selectableTarget = entity;
//                        }
//                        SetTileColor(true, Color.red, chao.LocalToCell(V2MousePos), tileMapRange);
//                    }
//                }
//            }
//            if (_selectableTarget != null && _selectable.Contains(_selectableTarget) == false)
//            {
//                if (targetUnits.Contains(_selectableTarget.gameObject) == false)
//                {
//                    SelectArrow(_selectableTarget.gameObject);
//                    _selectable.Add(_selectableTarget);
//                }
//            }
//        }
//    }

//    private Collider2D[] SelectInsideAoe()
//    {
//        Collider2D[] hits = null;
//        for (var i = 0; i < tilesAoe.Count; i++)
//        {
//            foreach (Transform entity in foundEntities)
//            {
//                BoxCollider2D childCollider = entity.transform.GetChild(0).GetComponent<BoxCollider2D>();
//                var V2EntityPos = childCollider.ClosestPoint(tilesAoe[i]);
//                if (tilesAoe.Contains(grid.LocalToCell(V2EntityPos)) && _selectable.Contains(entity) != true)
//                {
//                    _selectable.Add(entity);
//                }
//            }
//        }
//        List<Collider2D> tempList = new List<Collider2D>();
//        for (int i = 0; i < _selectable.Count; i++)
//        {
//            SelectArrow(_selectable[i].gameObject);
//            tempList.Add(_selectable[i].gameObject.GetComponent<Collider2D>());
//            //Debug.LogWarning($"_SELECTABLE IS {0} AND ITS COLLIDER IS {1}" + i + _selectable[i].gameObject.GetComponent<Collider2D>());
//        }
//        hits = tempList.ToArray();
//        return hits;
//    }


//    private List<GameObject> targetOnce(Collider2D[] hits)
//    {
//        foreach (Collider2D hit in hits)
//        {
//            if (hit != null)
//            {
//                if (foundEntities.Contains(hit.transform))
//                {
//                    targetUnit = hit.gameObject;
//                    CleanAllAreas();
//                    CleanEntities();
//                }
//                else if (hit.tag == "Floor")
//                {
//                    Vector2 V2MousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
//                    foreach (Transform entity in foundEntities)
//                    {
//                        BoxCollider2D childCollider = entity.transform.GetChild(0).GetComponent<BoxCollider2D>();
//                        var V2EntityPos = childCollider.ClosestPoint(V2MousePos);

//                        if (chao.LocalToCell(V2MousePos) == chao.LocalToCell(V2EntityPos))
//                        {
//                            targetUnit = entity.gameObject;
//                        }
//                    }
//                }
//            }
//        }
//        if (targetUnit != null)
//        {
//            if (_multiTargetsOnly) //If the same target cannot be selected many times
//            {
//                if (targetUnits.Contains(targetUnit) != true && targetUnit.GetComponent<EntityModel>() != null)
//                {
//                    targetUnits.Add(targetUnit);
//                    TargetArrow(targetUnit);
//                    timesTargetWasSent++;
//                }
//            }
//            else
//            {
//                if (targetUnit.GetComponent<EntityModel>() != null)
//                {
//                    targetUnits.Add(targetUnit);
//                    TargetArrow(targetUnit);
//                    timesTargetWasSent++;
//                }
//            }
//            canUpdate = true;
//            Debug.Log(targetUnits.Count);
//            Debug.Log(targetUnit);
//            targetUnit = null;
//            return targetUnits;
//        }
//        return null;
//    }

//    private List<GameObject> targetAoe(Collider2D[] hits)
//    {
//        foreach (Collider2D hit in hits)
//        {
//            Debug.Log(hit.gameObject);
//            var hitObject = hit.gameObject;
//            targetUnits.Add(hitObject);
//            //targetUnits.Add(hit.GetComponentInParent<Transform>().gameObject);
//        }
//        return targetUnits;
//    }

//    private List<GameObject> TargetFirstFoundEntity()
//    {
//        float LowestDistance = 0f;
//        Transform localEntity = null;
//        foreach (var entity in foundEntities)
//        {
//            var Distance = 0f;
//            if (LowestDistance == 0f)
//            {
//                LowestDistance = Vector3.Distance(_actionMaker.position, entity.position);
//                localEntity = entity;
//            }
//            else Distance = Vector3.Distance(_actionMaker.position, entity.position);
//            if (LowestDistance < Distance)
//            {
//                LowestDistance = Distance;
//                localEntity = entity;
//            }
//        }
//        if (localEntity != null)
//        {
//            timesTargetWasSent++;
//            targetUnits.Add(localEntity.gameObject);
//        }
//        return targetUnits;
//    }


//    private void RemoveAndDeselectTargetsOutOfRange()
//    {
//        foreach (GameObject unit in targetUnits)
//        {
//            if (foundEntities.Contains(unit.transform) != true)
//            {
//                targetUnits.Remove(unit);
//                timesTargetWasSent--;
//                ResetArrow(unit);
//            }
//        }
//    }

//    public void ResetParams()
//    {
//        onSearchMode = false;
//        _range = 0;
//        _actionMaker = null;
//        _AOE = 0;
//        _isAuto = false;
//        _HasAOEEffect = false;
//        targetUnit = null;
//        targetUnits.Clear();
//        previousCasterPosition = new Vector3();
//        timesTargetWasSent = 0;
//    }

//    private void CleanSelection(Color color)
//    {
//        if (_selectable != null)
//        {
//            foreach (Transform entity in _selectable)
//            {

//                arrow = entity.transform.GetChild(1);
//                var arrowSprite = arrow.GetComponent<SpriteRenderer>();
//                if (arrowSprite.color == color)
//                {
//                    arrowSprite.color = Color.white;
//                    if (arrowSprite.enabled == true) arrowSprite.enabled = false;
//                }

//            }
//            _selectable.Clear();
//            _selectableTarget = null;
//        }
//    }

//    public void ApplyEffectsOnTiles(List<Vector3> area, TerrainEffects effectType)
//    {
//        FillGrid(area, EffectsMap, FindEffectsTileBaseOfName(effectType));
//        ApplyCollider(area, EffectsMap);
//        EffectsMap.GetComponent<TerrainEffectsManagement>().GetEffectParams(area, effectType);
//    }

//    private void ApplyCollider(List<Vector3> area, Tilemap effectsMap)
//    {
//        foreach (Vector3 tile in area)
//        {
//            effectsMap.SetColliderType(Vector3Int.FloorToInt(tile), Tile.ColliderType.Grid);
//        }
//    }

//    private TileBase FindEffectsTileBaseOfName(TerrainEffects effectType)
//    {
//        TileBase tileBaseName = Resources.Load<TileBase>("Tiles/Animated Tiles/" + effectType + "AnimTile");
//        return tileBaseName;
//    }

//    GameObject RemovePreviousAutoSelection(GameObject previousSelection)
//    {
//        ResetArrow(previousSelection);
//        return null;
//    }
//    public void TargetArrow(GameObject _obj)
//    {
//        if (_obj != null && _obj.GetComponent<SpriteRenderer>())
//        {
//            arrow = _obj.transform.GetChild(1);
//            var arrowSprite = arrow.GetComponent<SpriteRenderer>();
//            if (arrowSprite.enabled == false) arrowSprite.enabled = true;
//            arrowSprite.color = Color.red;
//        }
//    }
//    public void TargetArrow(List<GameObject> _objs)
//    {
//        foreach (GameObject _obj in _objs)
//        {
//            if (_obj != null && _obj.GetComponent<SpriteRenderer>())
//            {
//                arrow = _obj.transform.GetChild(1);
//                var arrowSprite = arrow.GetComponent<SpriteRenderer>();
//                if (arrowSprite.enabled == false) arrowSprite.enabled = true;
//                arrowSprite.color = Color.red;
//            }
//        }
//    }
//    public void SelectArrow(GameObject _obj)
//    {
//        if (_obj != null && _obj.GetComponent<SpriteRenderer>())
//        {
//            arrow = _obj.transform.GetChild(1);
//            var arrowSprite = arrow.GetComponent<SpriteRenderer>();
//            if (arrowSprite.enabled == false) arrowSprite.enabled = true;
//            arrowSprite.color = Color.yellow;
//        }

//    }
//    public void SelectArrow(List<GameObject> _objs)
//    {
//        foreach (GameObject _obj in _objs)
//        {
//            if (_obj != null && _obj.GetComponent<SpriteRenderer>())
//            {
//                arrow = _obj.transform.GetChild(1);
//                var arrowSprite = arrow.GetComponent<SpriteRenderer>();
//                if (arrowSprite.enabled == false) arrowSprite.enabled = true;
//                arrowSprite.color = Color.yellow;
//            }
//        }

//    }
//    public void ResetArrow(GameObject _obj)
//    {
//        if (_obj != null && _obj.GetComponent<SpriteRenderer>())
//        {
//            arrow = _obj.transform.GetChild(1);
//            var arrowSprite = arrow.GetComponent<SpriteRenderer>();
//            arrowSprite.color = Color.white;
//            if (arrowSprite.enabled == true) arrowSprite.enabled = false;

//        }
//    }
//    public void ResetArrow(List<GameObject> _objs)
//    {
//        foreach (GameObject _obj in _objs)
//        {
//            if (_obj != null && _obj.GetComponent<SpriteRenderer>())
//            {
//                arrow = _obj.transform.GetChild(1);
//                var arrowSprite = arrow.GetComponent<SpriteRenderer>();
//                arrowSprite.color = Color.white;
//                if (arrowSprite.enabled == true) arrowSprite.enabled = false;
//            }
//        }
//    }
//}


