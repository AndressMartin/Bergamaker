using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour
{
    public Collider2D colisao;
    public SpriteRenderer sprite;
    public InputSys _input;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        _input = FindObjectOfType<Player>().GetComponent<InputSys>();

        //Achar o collider sem trigger
        Collider2D[] mbs = GetComponents<Collider2D>();
        foreach (Collider2D mb in mbs)
            if (mb.isTrigger == false)
                colisao = mb;


    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_input.interactPress)
                Open();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Close();
        }
    }

    void Open()
    {
        sprite.enabled = false;
        colisao.enabled = false;
    }
    void Close()
    {
        sprite.enabled = true;
        colisao.enabled = true;
    }
}
