using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaEndTargetFollow : MonoBehaviour
{
    public Vector3 targetPos;

    // Update is called once per frame
    void Update()
    {
        if (targetPos != null)
        {
            if (transform.position != targetPos) transform.position = targetPos;
        }
    }
}
