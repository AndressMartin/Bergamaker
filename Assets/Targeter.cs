using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    internal bool onSearchMode = false;
    public string desiredTarget { get; set; }
    public float range { get; set; }

    public GameObject targetUnit = null;

    public Material targetMat;
    public Material selectMat;
    public Material defaultMat;

    private Transform _selectable;

    private void Update()
    {
        if (onSearchMode == true)
            Target();
        //----Não chamar Aqui
        //TargetedOutline(targetUnit);
    }
    private GameObject Target()
    {
        if (_selectable != null)
        {
            ResetMat(_selectable.gameObject);
            _selectable = null;
        }
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.tag == desiredTarget)
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