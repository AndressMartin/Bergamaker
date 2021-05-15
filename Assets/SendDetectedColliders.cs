using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendDetectedColliders : MonoBehaviour
{
    

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("So...");
        if (collision.GetComponent<Player>() == true && GridManager.foundEntities.Contains(collision.transform) == false)
        {
            GridManager.foundEntities.Add(collision.transform);
            Debug.Log("Added" + collision.name);
        }
    }
}
