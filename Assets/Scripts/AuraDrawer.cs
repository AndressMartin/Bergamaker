using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AuraDrawer : MonoBehaviour
{
    private int vertexCount = 20; // 4 vertices == square
    public float lineWidth = 0.2f;
    public float radius;
    [SerializeField] public bool radiusOnMouse;

    private LineRenderer lineRenderer;
    Vector3 pos;
    Vector3[] pontos;
    public bool update;

    //FOR MOUSE
    [SerializeField] Vector3 mousePosition;

    private void Awake()
    {
        pontos = new Vector3[vertexCount];
        lineRenderer = GetComponent<LineRenderer>();
        SetupCircle();
    }

    private void Update()
    {
        
        {
            if (update)
            {
                SetupCircle();
                update = false;
            }
            if (radiusOnMouse == false)
            {
                for (int i = 0; i < lineRenderer.positionCount; i++)
                {
                    lineRenderer.SetPosition(i, pontos[i] + transform.position);
                }
            }
            else
            {
                FindWithMouse();
            }
        }
    }

  void FindWithMouse()
    {
        Debug.Log("Finding with mouse");
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, +10f);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, pontos[i] + mousePosition);
        }
    }

    private void SetupCircle()
    {
        lineRenderer.widthMultiplier = lineWidth;

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        lineRenderer.positionCount = vertexCount;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            lineRenderer.SetPosition(i, pos);
            pontos[i] = pos;
            theta += deltaTheta;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        Vector3 oldPos = Vector3.zero;
        for (int i = 0; i < vertexCount + 1; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            Gizmos.DrawLine(oldPos, transform.position + pos);
            oldPos = transform.position + pos;

            theta += deltaTheta;
        }
    }
#endif
}
