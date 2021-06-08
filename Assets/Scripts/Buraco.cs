using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buraco : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.parent != null)
        {
            if (collision.transform.parent.tag == "Player" /*|| collision.tag == "Enemy"*/)
            {
                if (!collision.transform.parent.GetComponent<Dash>().dashing)
                    Derrubar(collision.transform.parent);
            }
        }
    }

    private void Derrubar(Transform collider)
    {
        Debug.Log($"AAAAAAAAAAAAAAAAAAAAAAAAH {collider}");
        collider.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        collider.GetComponent<Movement>().PermitirMovimento(false);
    }
}
