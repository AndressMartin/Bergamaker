using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    internal bool onSearchMode = false;
    public GameObject targetUnit = null;
    private Outline outline;
    private void Update()
    {
        if (onSearchMode == true)
            Target();
        AddOrRemoveOutline();
    }
    private GameObject Target()
    {
        Debug.Log($"LF target");

        //-----------PLAY ANIM OF FINDING ENEMY-----------

        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);

                if (hit.collider.tag == "Enemy")
                {
                    targetUnit = hit.transform.gameObject;
                    Debug.Log(targetUnit.name);
                }
            }
        }
        return targetUnit;
    }

    void AddOrRemoveOutline()
    {
        if (targetUnit != null)
        {
            outline = targetUnit.AddComponent<Outline>();
        }
        //else
        //{
        //    Destroy(outline);
        //}
    }
}