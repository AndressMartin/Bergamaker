using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    internal bool onSearchMode { get; private set; }
    internal bool usingMouse { get; private set; }
    private bool beganSettingMouseToFalse;
    private string _desiredTarget;
    private int _range;
    private int _aoe;
    internal GameObject autoSelected { get; private set; }
    private Transform _actionMaker;
    //private InputSys _inputSys;
    public GameObject targetUnit = null;
    public Camera mainCamera;

    public Material targetMat;
    public Material selectMat;
    public Material defaultMat;
    private Transform _selectable;
    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (onSearchMode == true)
        {
            if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) && usingMouse == false)
            {
                usingMouse = true;
            }
            //else if ((Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0) && usingMouse == true)
            //{
            //     if (beganSettingMouseToFalse)
            //        StartCoroutine(SetUsingMouseAsFalse());
            //}
            if (usingMouse)
            {
                autoSelected = RemovePreviousAutoSelection(autoSelected);
                TargetWithMouse();
            }
            //DESATIVADO ENQUANTO NÃO FUNCIONA
            //else
            //{
            //    if (autoSelected == null)
            //    {
            //        var closest = FindClosestEnemy();
            //        AutoSelect(closest);
            //    }
            //}
        }
        //----Não chamar Aqui
        //TargetedOutline(targetUnit);
    }


    public void SearchMode(bool boo)
    {
        onSearchMode = boo;
    }
    public void StartSearchMode(bool boo, int range, Transform actionMaker, string desiredTarget)
    {
        onSearchMode = boo;
        _range = range;
        _actionMaker = actionMaker;
        _desiredTarget = desiredTarget;
        //_inputSys = _actionMaker.GetComponent<InputSys>();
    }
    public void StartSearchMode(bool boo, int range, Transform actionMaker, int aoe, string desiredTarget)
    {
        onSearchMode = boo;
        _range = range;
        _actionMaker = actionMaker;
        _desiredTarget = desiredTarget;
        _aoe = aoe;
        //_inputSys = _actionMaker.GetComponent<InputSys>();
    }

    private GameObject TargetWithMouse()
    {
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        CameraMoveWithMouse();
        if (_desiredTarget == "Player " || _desiredTarget == "Enemy")
        {
            if (_selectable != null)
            {
                if (_selectable.gameObject.tag == _desiredTarget)
                    ResetMat(_selectable.gameObject);
                _selectable = null;
            }
            if (hit.collider != null && hit.transform.gameObject.tag == _desiredTarget)
            {
                float distance = Vector2.Distance(_actionMaker.transform.position, hit.collider.transform.position);
                if (hit.collider.tag == _desiredTarget && distance <= _range)
                    SelectableOutline(hit.collider.gameObject);
            }
            _selectable = hit.transform;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //-----------PLAY ANIM OF FINDING ENEMY-----------
            if (hit.collider != null)
            {
                if (_aoe <= 0)
                {
                    if (hit.collider.tag == _desiredTarget)
                    {
                        targetUnit = hit.transform.gameObject;
                    }
                }
                else
                {
                    if (hit.collider.tag == _desiredTarget || hit.collider.tag == "Floor")
                    {
                        targetUnit = FindClosestEnemyTestForAOE(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    }
                }
            }
        }
        return targetUnit;
    }

    private void CameraMoveWithMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        float mouseX = (Input.mousePosition.x / Screen.width);
        float mouseY = (Input.mousePosition.y / Screen.height);
        mainCamera.transform.localPosition = (new Vector3(mouseX, mouseY, -10f));
    }

    private IEnumerator SetUsingMouseAsFalse()
    {
        beganSettingMouseToFalse = true;
        yield return new WaitForSeconds(1f);
        usingMouse = false;
        beganSettingMouseToFalse = false;
        Debug.Log("not using mouse");
    }

    private GameObject FindClosestEnemy()
    {
        Debug.Log("Auto Selection started");
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(_desiredTarget);
        float distance = Mathf.Infinity;
        Vector3 position = _actionMaker.transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                GameObject closest = go;
                distance = curDistance;
                Debug.Log(closest + "is the closest");
                return closest;
            }
            else
                return null;
        }
        return null;
    }
    private GameObject FindClosestEnemyTestForAOE(Vector2 pointer)
    {
        Debug.Log("Auto Selection started");
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = _aoe;
        Vector3 position = pointer;
        foreach (GameObject go in gos)
        {
            Debug.Log(go.name);
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                GameObject closest = go;
                distance = curDistance;
                Debug.Log(closest + "is the closest");
                return closest;
            }
        }
        return null;
    }
    private GameObject FindAllForAOE(Vector2 pointer)
    {
        Debug.Log("Auto Selection started");
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = _aoe;
        Vector3 position = pointer;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                GameObject closest = go;
                distance = curDistance;
                Debug.Log(closest + "is the closest");
                return closest;
            }
            else
                return null;
        }
        return null;
    }

    private void FindAll()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(_desiredTarget);
        float distance = Mathf.Infinity;
        Vector3 position = _actionMaker.transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                GameObject closest = go;
                distance = curDistance;
                Debug.Log(closest + "is the closest");
                AutoSelect(closest);
            }
        }
    }
    private void AutoSelect(GameObject closestTarget)
    {
        SelectableOutline(closestTarget);
        autoSelected = closestTarget;
        ResetParams();
    }
    public void ResetParams()
    {
        onSearchMode = false;
        _range = 0;
        _actionMaker = null;
        _aoe = 0;
        autoSelected = null;
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