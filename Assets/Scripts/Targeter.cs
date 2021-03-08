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
    public string desiredTarget { get; set; }
    private int _range;
    private Transform _actionMaker;

    public GameObject targetUnit = null;

    public Material targetMat;
    public Material selectMat;
    public Material defaultMat;

    private Transform _selectable;

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
            else
            {
                if (autoSelected == null)
                {
                    var closest = FindClosestEnemy();
                    AutoSelect(closest);
                }
            }
        }
        //----Não chamar Aqui
        //TargetedOutline(targetUnit);
    }

    private IEnumerator SetUsingMouseAsFalse()
    {
        beganSettingMouseToFalse = true;
        yield return new WaitForSeconds(1f);
        usingMouse = false;
        beganSettingMouseToFalse = false;
        Debug.Log("not using mouse");
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
    }

    private GameObject TargetWithMouse()
    {
        if (_selectable != null)
        {
            ResetMat(_selectable.gameObject);
            _selectable = null;
        }
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            float distance = Vector2.Distance(_actionMaker.transform.position, hit.collider.transform.position);
            if (hit.collider.tag == desiredTarget && distance <= 2)
                SelectableOutline(hit.collider.gameObject);
        }
        _selectable = hit.transform;
        if (Input.GetMouseButtonDown(0))
        {
            //-----------PLAY ANIM OF FINDING ENEMY-----------
            if (hit.collider != null)
            {
                if (hit.collider.tag == desiredTarget)
                {
                    targetUnit = hit.transform.gameObject;
                    Debug.Log(targetUnit.name);
                }
            }
        }
        return targetUnit;
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
    GameObject RemovePreviousAutoSelection(GameObject previousSelection)
    {
        ResetMat(previousSelection);
        return null;
    }
    private void AutoSelect(GameObject closestTarget)
    {
        //closestTarget = FindClosestEnemy();
        //targetUnit = closestTarget;
        SelectableOutline(closestTarget);
        autoSelected = closestTarget;
    }
    public void TargetedOutline(GameObject _obj)
    {
        if (_obj != null)
        {
            _obj.GetComponent<SpriteRenderer>().material = targetMat;
        }
    }
    public void SelectableOutline(GameObject _obj)
    {
        if (_obj != null)
        {
            _obj.GetComponent<SpriteRenderer>().material = selectMat;
        }
            
    }
    public void ResetMat(GameObject _obj)
    {
        if (_obj != null)
        {
            _obj.GetComponent<SpriteRenderer>().material = defaultMat;
        }
    }
}