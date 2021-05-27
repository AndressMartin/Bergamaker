using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

public class ItemWorld : MonoBehaviour
{
    private void Start()
    {
        
    }
    

    public static ItemWorld SpawItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }
    public static ItemWorld DropItem(Vector3 dropPosition,Item item)
    {
        Vector3 randomDir = UtilsClass.GetRandomDir();
        ItemWorld itemWorld = SpawItemWorld(dropPosition + randomDir * 3f, item);
        itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 3f, ForceMode2D.Impulse);
        return itemWorld;
    }


    private Item item;
    private SpriteRenderer SpriteRenderer;
    private TextMeshPro TextMeshPro;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        TextMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    public void SetItem(Item item)
    {
        this.item = item;
        SpriteRenderer.sprite = item.GetSprite();

        if(item.amount > 1)
        {
            TextMeshPro.SetText(item.amount.ToString());

        }
        else
        {
            TextMeshPro.SetText("");
        }
    }

    public Item GetItem()
    {
        return item;
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
