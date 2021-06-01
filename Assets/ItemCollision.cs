using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollision : MonoBehaviour
{
    public Inventory inventory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tag == "PlayerCollisionChild")
        {
            ItemWorld itemWolrd = collision.gameObject.GetComponent<ItemWorld>();
            if (itemWolrd != null)
            {
                inventory.AddItem(itemWolrd.GetItem());
                itemWolrd.DestroySelf();
            }
        }
    }
}
