using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridEntity : GridManager
{
    Grid grid;
    public List<Vector3> tilesRange;
    public List<Vector3> tilesAoe;
    public List<Vector3> tilesFull;
    public List<Vector3> tilesIgnore;
    public List<Transform> foundEntities = new List<Transform>();
    public List<Transform> foundObjects = new List<Transform>();
    public List<Transform> foundTerrain = new List<Transform>();
    [SerializeField] Vector3 mousePosition;
    private Vector3 previousCasterPosition = new Vector3();
    private Transform arrow;
    private bool canUpdate = true;

    public bool onSearchMode { get; private set; }
    public bool auto = false; //For self targeting
    public List<string> _desiredTargets = new List<string>();
    private bool _isAuto;
    public int _range;
    public int _AOE;
    public Vector3 pointClicked;
    public Vector3 centerOfAOE;
    public Shapes _shapeType;
    public int _targetsNum;
    public int timesTargetWasSent;
    public bool _multiTargetsOnly;
    public bool _HasAOEEffect;
    public bool gridIni;
    private Transform _actionMaker;
    //For Direct Actions
    public GameObject targetUnit = null;
    //For Actions with multiple possible targets
    public List<GameObject> targetUnits = new List<GameObject>();
    public Camera mainCamera;
    public List<Transform> _selectable = new List<Transform>();
    public Transform _selectableTarget;
    public LayerMask ignorar;
    public GridGlobal _gridGlobal;
    public bool canAttack;
    public void Start()
    {
        _gridGlobal = FindObjectOfType<GridGlobal>();
        grid = _gridGlobal.grid;
        tilesRange = new List<Vector3>();
        tilesAoe = new List<Vector3>();
        tilesFull = new List<Vector3>();
        mainCamera = Camera.main;
    }
    private void Update()
    {
        TargetingLoop();
    }
    public void TargetingLoop()
    {
        if (onSearchMode)
        {
            Debug.Log("Entrou no search");
            CleanEntities();
            if (_isAuto) 
            { 
                AutoRange(_actionMaker, _range, tilesRange);
                //_gridGlobal.FillGrid(tilesRange, _gridGlobal.tileMapRange, tilesIgnore); //NOTE: AUTOATTACK SHOULDNT FILL GRID BECAUSE OF AI. LATER: IMPLEMENT NEVER TO FILL GRID IF AI.
                FindEntities(_desiredTargets);
                TargetFirstFoundEntity();
                CleanAllAreas();
                return;
            }
            CasterRange(_actionMaker, _range, tilesRange);
            _gridGlobal.FindWalls(tilesRange, tilesIgnore);
            _gridGlobal.FillGrid(tilesRange, _gridGlobal.tileMapRange, tilesIgnore);
            if (_AOE > 1)
            {
                MouseRange(_AOE, tilesAoe);
                _gridGlobal.FindWalls(tilesAoe, tilesIgnore);
                _gridGlobal.FillGrid(tilesAoe, _gridGlobal.tileMapAoe, tilesIgnore);
                _gridGlobal.PaintGridForAOE(tilesAoe);
            }
            if (_AOE == 1)
            {
                _gridGlobal.CleanArea(tilesAoe, _gridGlobal.tileMapAoe);
                tilesAoe = tilesRange.ToList();
                _gridGlobal.FillGrid(tilesAoe, _gridGlobal.tileMapAoe, tilesIgnore);
                _gridGlobal.PaintGridForAOE(tilesAoe);
            }
            tilesIgnore.Clear();
            FindEntities(_desiredTargets);
            //FindObjects();
            //FindTerrain();
            _gridGlobal.PaintGridForFoundEntities(foundEntities, tilesFull);
            RemoveAndDeselectTargetsOutOfRange();
            //Handle selection
            Collider2D[] selection = StartSelectionWithMouse();
            if (_AOE == 1)
            {
                selection = null;
                selection = StartSelectionForRange();
            }
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Clicking");
                if (_AOE > 0)
                {
                    targetAoe(selection);
                }
                if (selection != null)
                {
                    Debug.Log("Selection Not Null");
                    pointClicked = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, +10f));
                    
                    if (_AOE <= 0)
                    {
                        Debug.Log("Targetting Once");
                        targetOnce(selection);
                    }
                }
            }
            
        }
        else if (!_isAuto)
        {
            _gridGlobal.CleanAllTileMaps(tilesRange, tilesAoe);
            CleanAllAreas();
            CleanEntities();
            CleanSelection(Color.yellow);
        }
    }

    

    private bool UpdateGrid()
    {
        if (grid.LocalToCell(previousCasterPosition) != grid.LocalToCell(_actionMaker.position))
        {
            previousCasterPosition = _actionMaker.position;
            return true;
        }
        if (canUpdate == true)
        {
            canUpdate = false;
            return true;
        }
        //else if ()
        return false;
    }
    private void AutoRange(Transform caster, int range, List<Vector3> _tiles)
    {
        GetArea(range, caster.GetChild(0).position, _tiles, Shapes.Area);
    }

    void CasterRange(Transform caster, int range, List<Vector3> _tiles)
    {
        _gridGlobal.CleanArea(tilesFull, _gridGlobal.tileMapRange);
        if (UpdateGrid())
        {
            _gridGlobal.CleanArea(_tiles, _gridGlobal.tileMapRange);
            GetArea(range, caster.GetChild(0).position, _tiles, _shapeType);
        }
    }
    
    void MouseRange(int area, List<Vector3> _tiles)
    {
        Vector3 previousMousePosition = mousePosition;
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, +10f);
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

        if (grid.LocalToCell(mousePosition) != grid.LocalToCell(previousMousePosition))
        {
            if (tilesRange.Contains(grid.LocalToCell(mousePosition)))
            {
                _gridGlobal.CleanArea(_tiles, _gridGlobal.tileMapAoe);
                GetArea(area, mousePosition, _tiles, _shapeType);
            }
            else
            {
                _gridGlobal.CleanArea(_tiles, _gridGlobal.tileMapAoe);
            }
        }
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
        if (_isAuto)
        {
            Vector3 center = grid.LocalToCell(position);
            centerOfAOE = grid.GetCellCenterWorld(Vector3Int.FloorToInt(center));
            _tiles.Add(center);
            for (int i = 0; i < range; i++)
            {
                for (int iteration = 1; iteration < 5; iteration++)
                {
                    Debug.Log("Painting");
                    if (!tilesFull.Contains(Coordinates(iteration, i, center)))
                        _tiles.Add(Coordinates(iteration, i, center));
                }
                for (int f = 1; f <= i+1; f++)
                {
                    for (int iteration = 1; iteration < 5; iteration++)
                    {
                        if (iteration == 1) _tiles.Add(new Vector3(center.x - (i + 1), center.y + f, center.z));
                        if (iteration == 2) _tiles.Add(new Vector3(center.x - (i + 1), center.y - f, center.z));
                        if (iteration == 3) _tiles.Add(new Vector3(center.x + (i + 1), center.y + f, center.z));
                        if (iteration == 4) _tiles.Add(new Vector3(center.x + (i + 1), center.y - f, center.z));
                        if (!tilesFull.Contains(Coordinates(iteration, i, f, center)))
                            _tiles.Add(Coordinates(iteration, i, f, center));
                    }
                }
            }
            return;
        }
        if (shapeType == Shapes.Area)
        {
            Vector3 center = grid.LocalToCell(position);
            centerOfAOE = grid.GetCellCenterWorld(Vector3Int.FloorToInt(center));
            _tiles.Add(center);
            for (int i = 0; i < range; i++)
            {
                for (int iteration = 1; iteration < 5; iteration++)
                {
                    Debug.Log("Painting");
                    if (!tilesFull.Contains(Coordinates(iteration, i, center)))
                        _tiles.Add(Coordinates(iteration, i, center));
                }
                if (i > 0 && i < range)
                {
                    for (int f = 1; f <= i; f++)
                    {
                        Debug.Log("Painting");
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
            List<float> lastCoordinates = new List<float>();
            if (lastCoordinates != _actionMaker.GetComponent<Movement>().lastCoordinates)
            {
                lastCoordinates = _actionMaker.GetComponent<Movement>().lastCoordinates;
                canUpdate = true;
            }
            var center = grid.LocalToCell(_actionMaker.transform.GetChild(0).position);
            if (lastCoordinates[0] != 0)
            {
                for (int i = 0; i < range; i++)
                {
                    _tiles.Add(new Vector3(center.x + ((i + 1) * lastCoordinates[0]), center.y, center.z));
                }
            }
            else if (lastCoordinates[1] != 0)
            {
                for (int i = 0; i < range; i++)
                {
                    _tiles.Add(new Vector3(center.x, center.y + ((i + 1) * lastCoordinates[1]), center.z));
                }
            }
        }
    }
    public void FindEntities(List<string> entityTags)
    {
        tilesFull.AddRange(tilesRange);
        tilesFull.AddRange(tilesAoe);
        GameObject[] arr = null; //Temporary array 
        List<GameObject> localEntities = new List<GameObject>();
        CleanEntities();
        foreach (string tag in entityTags)
        {
            arr = GameObject.FindGameObjectsWithTag(tag);
            foreach (var element in arr) localEntities.Add(element);
        }
        Array.Clear(arr, 0, arr.Length);
        for (var i = 0; i < tilesFull.Count; i++)
        {
            for (var l = 0; l < localEntities.Count; l++)
            {
                BoxCollider2D childCollider = localEntities[l].transform.GetChild(0).GetComponent<BoxCollider2D>();
                var V2EntityPos = childCollider.ClosestPoint(tilesFull[i]);
                if (_gridGlobal.chao.LocalToCell(tilesFull[i]) == _gridGlobal.chao.LocalToCell(V2EntityPos) && foundEntities.Contains(localEntities[l].transform) == false)
                {
                    foundEntities.Add(localEntities[l].transform);
                }
            }
        }
    }

    public List<GameObject> FindEntities(List<string> entityTags, List<Vector3> area)
    {
        List<GameObject> _foundEntities = new List<GameObject>();
        GameObject[] arr = null; //Temporary array 
        List<GameObject> localEntities = new List<GameObject>();
        foreach (string tag in entityTags)
        {
            arr = GameObject.FindGameObjectsWithTag(tag);
            foreach (var element in arr) localEntities.Add(element);
        }
        Array.Clear(arr, 0, arr.Length);
        for (var i = 0; i < area.Count; i++)
        {
            for (var l = 0; l < localEntities.Count; l++)
            {
                BoxCollider2D childCollider = localEntities[l].transform.GetChild(0).GetComponent<BoxCollider2D>();
                var V2EntityPos = childCollider.ClosestPoint(area[i]);
                if (_gridGlobal.chao.LocalToCell(area[i]) == _gridGlobal.chao.LocalToCell(V2EntityPos) && foundEntities.Contains(localEntities[l].transform) == false)
                {
                    _foundEntities.Add(localEntities[l]);
                }
            }
        }
        return _foundEntities;
    }

    private void CleanEntities()
    {
        foundEntities.Clear();
    }
    private void RemoveAndDeselectTargetsOutOfRange()
    {
        foreach (GameObject unit in targetUnits)
        {
            if (foundEntities.Contains(unit.transform) != true)
            {
                targetUnits.Remove(unit);
                timesTargetWasSent--;
                ResetArrow(unit);
            }
        }
    }

    private Collider2D[] StartSelectionWithMouse()
    {
        if (_AOE == 1) return null;
        //Debug.LogWarning("Is on target");
        Collider2D[] hits = Physics2D.OverlapPointAll(mainCamera.ScreenToWorldPoint(Input.mousePosition), ~ignorar);
        CleanSelection(Color.yellow);
        if (_AOE > 1) return SelectInsideAoe();
        else SelectTarget(hits);
        return hits;
    }
    private Collider2D[] StartSelectionForRange()
    {
        List<Collider2D> entityColliders = new List<Collider2D>();
        foreach (var entity in foundEntities)
        {
            entityColliders.Add(entity.GetComponent<Collider2D>());
        }
        Collider2D[] hits = entityColliders.ToArray();
        return hits;
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
            SelectArrow(_selectable[i].gameObject);
            tempList.Add(_selectable[i].gameObject.GetComponent<Collider2D>());
            //Debug.LogWarning($"_SELECTABLE IS {0} AND ITS COLLIDER IS {1}" + i + _selectable[i].gameObject.GetComponent<Collider2D>());
        }
        hits = tempList.ToArray();
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
                    if (_gridGlobal.chao.LocalToCell(V2MousePos) == _gridGlobal.chao.LocalToCell(V2EntityPos))
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
                        _gridGlobal.SetTileColor(true, Color.red, _gridGlobal.chao.LocalToCell(V2MousePos), _gridGlobal.tileMapRange);
                    }
                }
            }
            if (_selectableTarget != null && _selectable.Contains(_selectableTarget) == false)
            {
                if (targetUnits.Contains(_selectableTarget.gameObject) == false)
                {
                    SelectArrow(_selectableTarget.gameObject);
                    _selectable.Add(_selectableTarget);
                }
            }
        }
    }

    private List<GameObject> TargetFirstFoundEntity()
    {
        float LowestDistance = 0f;
        Transform localEntity = null;
        foreach (var entity in foundEntities)
        {
            var Distance = 0f;
            if (LowestDistance == 0f)
            {
                LowestDistance = Vector3.Distance(_actionMaker.position, entity.position);
                localEntity = entity;
            }
            else Distance = Vector3.Distance(_actionMaker.position, entity.position);
            if (LowestDistance < Distance)
            {
                LowestDistance = Distance;
                localEntity = entity;
            }
        }
        if (localEntity != null)
        {
            timesTargetWasSent++;
            targetUnits.Add(localEntity.gameObject);
        }
        return targetUnits;
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
                    _gridGlobal.CleanAllTileMaps(tilesRange, tilesAoe);
                    CleanEntities();
                }
                else if (hit.tag == "Floor")
                {
                    Vector2 V2MousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    foreach (Transform entity in foundEntities)
                    {
                        BoxCollider2D childCollider = entity.transform.GetChild(0).GetComponent<BoxCollider2D>();
                        var V2EntityPos = childCollider.ClosestPoint(V2MousePos);

                        if (_gridGlobal.chao.LocalToCell(V2MousePos) == _gridGlobal.chao.LocalToCell(V2EntityPos))
                        {
                            targetUnit = entity.gameObject;
                        }
                    }
                }
            }
        }
        if (targetUnit != null)
        {
            if (_multiTargetsOnly) //If the same target cannot be selected many times
            {
                if (targetUnits.Contains(targetUnit) != true && targetUnit.GetComponent<EntityModel>() != null)
                {
                    targetUnits.Add(targetUnit);
                    TargetArrow(targetUnit);
                    timesTargetWasSent++;
                }
            }
            else
            {
                if (targetUnit.GetComponent<EntityModel>() != null)
                {
                    targetUnits.Add(targetUnit);
                    TargetArrow(targetUnit);
                    timesTargetWasSent++;
                }
            }
            canUpdate = true;
            Debug.Log(targetUnits.Count);
            Debug.Log(targetUnit);
            targetUnit = null;
            return targetUnits;
        }
        return null;
    }

    private List<GameObject> targetAoe(Collider2D[] hits)
    {
        if (!hits.Any()) canAttack = true; 
        foreach (Collider2D hit in hits)
        {
            Debug.Log(hit.gameObject);
            var hitObject = hit.gameObject;
            targetUnits.Add(hitObject);
            //targetUnits.Add(hit.GetComponentInParent<Transform>().gameObject);
        }
        return targetUnits;
    }
    void CleanAllAreas()
    {
        tilesRange.Clear();
        tilesAoe.Clear();
        tilesFull.Clear();
        tilesIgnore.Clear();
    }
    public void ResetParams()
    {
        onSearchMode = false;
        _range = 0;
        _actionMaker = null;
        _AOE = 0;
        _isAuto = false;
        _HasAOEEffect = false;
        targetUnit = null;
        canAttack = false;
        targetUnits.Clear();
        previousCasterPosition = new Vector3();
        timesTargetWasSent = 0;
        _shapeType = Shapes.Area;
    }
    private void CleanSelection(Color color)
    {
        if (_selectable != null)
        {
            foreach (Transform entity in _selectable)
            {

                arrow = entity.transform.GetChild(1);
                var arrowSprite = arrow.GetComponent<SpriteRenderer>();
                if (arrowSprite.color == color)
                {
                    arrowSprite.color = Color.white;
                    if (arrowSprite.enabled == true) arrowSprite.enabled = false;
                }

            }
            _selectable.Clear();
            _selectableTarget = null;
        }
    }

    internal List<Vector3> PassAOEArea()
    {
        return tilesAoe;
    }
    internal Vector3 PassPointClicked()
    {
        return pointClicked;
    }
    internal Vector3 PassCenterOfAOE()
    {
        return centerOfAOE;
    }
    public void SearchMode(bool boo)
    {
        onSearchMode = boo;
    }
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
    public void StartSearchMode(bool boo, int range, Transform actionMaker, int aoe, bool HasAOEEffect, Shapes shapeType, List<string> desiredTargets)
    {
        onSearchMode = boo;
        _range = range;
        _actionMaker = actionMaker;
        _desiredTargets = desiredTargets;
        _AOE = aoe;
        _shapeType = shapeType;
        _HasAOEEffect = HasAOEEffect;
    }

    internal void StartSearchMode(bool boo, int range, Transform actionMaker, List<string> desiredTargets, bool isAuto)
    {
        onSearchMode = boo;
        _range = range;
        _actionMaker = actionMaker;
        _desiredTargets = desiredTargets;
        _isAuto = isAuto;
    }
    public void TargetArrow(GameObject _obj)
    {
        if (_obj != null && _obj.GetComponent<SpriteRenderer>())
        {
            arrow = _obj.transform.GetChild(1);
            var arrowSprite = arrow.GetComponent<SpriteRenderer>();
            if (arrowSprite.enabled == false) arrowSprite.enabled = true;
            arrowSprite.color = Color.red;
        }
    }
    public void TargetArrow(List<GameObject> _objs)
    {
        foreach (GameObject _obj in _objs)
        {
            if (_obj != null && _obj.GetComponent<SpriteRenderer>())
            {
                arrow = _obj.transform.GetChild(1);
                var arrowSprite = arrow.GetComponent<SpriteRenderer>();
                if (arrowSprite.enabled == false) arrowSprite.enabled = true;
                arrowSprite.color = Color.red;
            }
        }
    }
    public void SelectArrow(GameObject _obj)
    {
        if (_obj != null && _obj.GetComponent<SpriteRenderer>())
        {
            arrow = _obj.transform.GetChild(1);
            var arrowSprite = arrow.GetComponent<SpriteRenderer>();
            if (arrowSprite.enabled == false) arrowSprite.enabled = true;
            arrowSprite.color = Color.yellow;
        }

    }
    public void SelectArrow(List<GameObject> _objs)
    {
        foreach (GameObject _obj in _objs)
        {
            if (_obj != null && _obj.GetComponent<SpriteRenderer>())
            {
                arrow = _obj.transform.GetChild(1);
                var arrowSprite = arrow.GetComponent<SpriteRenderer>();
                if (arrowSprite.enabled == false) arrowSprite.enabled = true;
                arrowSprite.color = Color.yellow;
            }
        }

    }
    public void ResetArrow(GameObject _obj)
    {
        if (_obj != null && _obj.GetComponent<SpriteRenderer>())
        {
            arrow = _obj.transform.GetChild(1);
            var arrowSprite = arrow.GetComponent<SpriteRenderer>();
            arrowSprite.color = Color.white;
            if (arrowSprite.enabled == true) arrowSprite.enabled = false;

        }
    }
    public void ResetArrow(List<GameObject> _objs)
    {
        foreach (GameObject _obj in _objs)
        {
            if (_obj != null && _obj.GetComponent<SpriteRenderer>())
            {
                arrow = _obj.transform.GetChild(1);
                var arrowSprite = arrow.GetComponent<SpriteRenderer>();
                arrowSprite.color = Color.white;
                if (arrowSprite.enabled == true) arrowSprite.enabled = false;
            }
        }
    }
}
