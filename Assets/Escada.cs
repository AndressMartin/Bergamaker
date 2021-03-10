using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Escada : MonoBehaviour
{
    public BoxCollider2D _boxCollider;
    public Collider2D _tileMapCollider;
    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && 
            collision.GetComponent<Movement>().isClimbing == false)
        {
            Debug.Log("STAY");
            collision.GetComponent<Movement>().isClimbing = true;
            _boxCollider.enabled = false;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("EXIT");
            collision.GetComponent<Movement>().isClimbing = false;
            _boxCollider.enabled = true;
        }
    }
}