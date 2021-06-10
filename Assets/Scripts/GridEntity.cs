//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//public class GridEntity : MonoBehaviour
//{
//    public bool onSearchMode { get; private set; }
//    public bool auto = false; //For self targeting
//    public List<string> _desiredTargets = new List<string>();
//    private bool _isAuto;
//    public int _range;
//    public int _AOE;
//    public Vector3 pointClicked;
//    public Vector3 centerOfAOE;
//    public Shapes _shapeType;
//    public int _targetsNum;
//    public int timesTargetWasSent;
//    public bool _multiTargetsOnly;
//    public bool _HasAOEEffect;

//    private Transform _actionMaker;
//    //For Direct Actions
//    public GameObject targetUnit = null;
//    //For Actions with multiple possible targets
//    public List<GameObject> targetUnits = new List<GameObject>();
//    public Camera mainCamera;
//    public List<Transform> _selectable = new List<Transform>();
//    public Transform _selectableTarget;
//    public LayerMask ignorar;

//    void CasterRange(Transform caster, int range, List<Vector3> _tiles)
//    {
//        CleanArea(tilesFull, tileMapRange);

//        if (UpdateGrid())
//        {
//            CleanArea(_tiles, tileMapRange);
//            GetArea(range, caster.GetChild(0).position, _tiles, _shapeType);
//        }
//    }
//    private bool UpdateGrid()
//    {
//        if (grid.LocalToCell(previousCasterPosition) != grid.LocalToCell(_actionMaker.position))
//        {
//            previousCasterPosition = _actionMaker.position;
//            return true;
//        }
//        if (canUpdate == true)
//        {
//            canUpdate = false;
//            return true;
//        }
//        //else if ()
//        return false;
//    }
//    void MouseRange(int area, List<Vector3> _tiles)
//    {
//        Vector3 previousMousePosition = mousePosition;
//        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, +10f);
//        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

//        if (grid.LocalToCell(mousePosition) != grid.LocalToCell(previousMousePosition))
//        {
//            if (tilesRange.Contains(grid.LocalToCell(mousePosition)))
//            {
//                CleanArea(_tiles, tileMapAoe);
//                GetArea(area, mousePosition, _tiles, _shapeType);
//            }
//        }
//    }

//    private void CleanArea(List<Vector3> _tiles, Tilemap _tilemap)
//    {
//        _tiles.Clear();
//        tilesIgnore.Clear();
//        _tilemap.ClearAllTiles(); //Send to GridManager
//    }





//    internal List<Vector3> SendAOEArea()
//    {
//        return tilesAoe;
//    }
//    internal Vector3 SendPointClicked()
//    {
//        return pointClicked;
//    }
//    internal Vector3 SendCenterOfAOE()
//    {
//        return centerOfAOE;
//    }
//}
