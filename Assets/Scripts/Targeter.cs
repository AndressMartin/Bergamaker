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
    internal GameObject autoSelected { get; private set; }
    public string desiredTarget;
    private int _range;
    public int _aoe;
    private Transform _actionMaker;
    private InputSys _inputSys;
    public GameObject targetUnit = null;

    public Material targetMat;
    public Material selectMat;
    public Material defaultMat;
    private Transform _selectable;
    public Camera mainCamera;
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
    public void StartSearchMode(bool boo, int range, Transform actionMaker)
    {
        onSearchMode = boo;
        _range = range;
        _actionMaker = actionMaker;
        _inputSys = _actionMaker.GetComponent<InputSys>();
        Debug.LogAssertion(boo + "" + range + "" + actionMaker);
    }
    public void StartSearchMode(bool boo, int range, Transform actionMaker, int aoe)
    {
        onSearchMode = boo;
        _range = range;
        _actionMaker = actionMaker;
        _aoe = aoe;
        _inputSys = _actionMaker.GetComponent<InputSys>();
        Debug.LogAssertion(boo + "" + range + "" + actionMaker);
    }

    private GameObject TargetWithMouse()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (desiredTarget == "Player " || desiredTarget == "Enemy")
        {
            if (_selectable != null)
            {
                if (_selectable.gameObject.tag == desiredTarget)
                    ResetMat(_selectable.gameObject);
                _selectable = null;
            }
            if (hit.collider != null && hit.transform.gameObject.tag == desiredTarget)
            {
                float distance = Vector2.Distance(_actionMaker.transform.position, hit.collider.transform.position);
                if (hit.collider.tag == desiredTarget && distance <= _range)
                    SelectableOutline(hit.collider.gameObject);
            }
            _selectable = hit.transform;
        }
        //else if (desiredTarget == "Floor")
        //{
        //    if (_selectable != null)
        //    {
        //        _selectable = null;
        //    }
        //    if (hit.collider != null && hit.transform.gameObject.tag == desiredTarget)
        //    {
        //        float distance = Vector2.Distance(_actionMaker.transform.position, hit.collider.transform.position);
        //        //if (hit.collider.tag == desiredTarget && distance <= _range)
        //    }
        //    _selectable = hit.transform;
        //}

        if (Input.GetMouseButtonDown(0))
        {
            //-----------PLAY ANIM OF FINDING ENEMY-----------
            if (hit.collider != null)
            {
                if (_aoe <= 0)
                {
                    if (hit.collider.tag == desiredTarget)
                    {
                        targetUnit = hit.transform.gameObject;
                    }
                }
                else
                {
                    if (hit.collider.tag == desiredTarget || hit.collider.tag == "Enemy") //NEEDS TO BE FIXED FOR FRIENDLY SKILLS
                    {
                        targetUnit = FindClosestEnemyTestForAOE(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    }
                }
            }
        }
        return targetUnit;
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
        gos = GameObject.FindGameObjectsWithTag(desiredTarget);
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
        gos = GameObject.FindGameObjectsWithTag(desiredTarget);
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
        ResetParams(onSearchMode, _range, _actionMaker, autoSelected);
        Debug.Log("AUTOSELECTED" + autoSelected);
    }

    public void ResetParams(bool boo, int range, Transform actionMaker, GameObject target)
    {
        boo = false;
        range = 0;
        actionMaker = null;
        target = null;
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