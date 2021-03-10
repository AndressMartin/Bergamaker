using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EscadaOld : MonoBehaviour
{
    public BoxCollider2D _boxCollider;
    public Collider2D _tileMapCollider;
    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<TilemapCollider2D>())
        {
            _tileMapCollider = collision;
        }
        if (collision.transform.parent != null)
        {
            
            if (collision.transform.parent.tag == "Player")
            {
                if (!collision.transform.parent.GetComponent<Dash>().dashing &&
                    collision.transform.parent.GetComponent<InputSys>().vertical == 1)
                { 
                    if (_tileMapCollider != null)
                        LiberarAcesso(_boxCollider);
                    else
                        LiberarAcesso(_boxCollider, _tileMapCollider);
                    Debug.Log("Liberando");
                }
                else
                {
                    if (_tileMapCollider != null)
                        BloquearAcesso(_boxCollider);
                    else
                        BloquearAcesso(_boxCollider, _tileMapCollider);
                    Debug.Log("Bloqueando");

                }
            }
            
        }
    }

    private void LiberarAcesso(BoxCollider2D boxCollider, Collider2D tilemapCollider)
    {
        boxCollider.enabled = false;
        tilemapCollider.enabled = false;
    }
    private void LiberarAcesso(BoxCollider2D boxCollider)
    {
        boxCollider.enabled = false;
    }
    private void BloquearAcesso(BoxCollider2D boxCollider)
    {
        boxCollider.enabled = true;
    }
    private void BloquearAcesso(BoxCollider2D boxCollider, Collider2D tilemapCollider)
    {
        boxCollider.enabled = true;
        tilemapCollider.enabled = true;

    }
}
