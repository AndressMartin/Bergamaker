using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    internal bool onSearchMode = false;
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
    private void FindSelectable()
    {
        if (_selectable != null)
        {
            var selectionRenderer = _selectable.GetComponent<SpriteRenderer>();
            selectionRenderer.material = defaultMat;
            _selectable = null;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectable = hit.transform;
            if (selectable.CompareTag("Selectable"))
            {
                var selectionRenderer = selectable.GetComponent<Renderer>();
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = targetMat;
                }

                _selectable = selectable;
            }
        }
    }
    private GameObject Target()
    {
        //Debug.Log($"LF target");

        //-----------PLAY ANIM OF FINDING ENEMY-----------
        if (_selectable != null)
        {
            ResetMat(_selectable.gameObject);
            _selectable = null;
        }
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            SelectableOutline(hit.collider.gameObject);
        }
        _selectable = hit.transform;
        if (Input.GetMouseButtonDown(0))
        {

            if (hit.collider != null)
            {
                //Debug.Log("Target object: " + hit.collider.gameObject.name);

                if (hit.collider.tag == "Enemy")
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