using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridIndice : MonoBehaviour
{
    public int thisIndice;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Imovel")
        {
            gameObject.tag = "Impassavel";
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Imovel")
        {
            gameObject.tag = "GridTile";
        }
    }
}
