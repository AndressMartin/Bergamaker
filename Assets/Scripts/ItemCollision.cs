using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollision : MonoBehaviour
{
    public Inventory inventory;
    public ItemSounds itemSounds;

    private void Awake()
    {
        itemSounds = FindObjectOfType<ItemSounds>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tag == "PlayerCollisionChild")
        {
            //Debug.Log("por cima do item aperte espaço");
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
                ItemWorld itemWolrd = collision.gameObject.GetComponent<ItemWorld>();
                if (itemWolrd != null)
                {
                    itemSounds.main(itemWolrd.item);
                    inventory.AddItem(itemWolrd.GetItem());
                    itemWolrd.DestroySelf();
                }
            //}

        }
    }

}
