using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour
{
    public Collider2D colisao;
    public SpriteRenderer sprite;
    public InputSys _input;
    public bool interacting;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        _input = GetComponent<InputSys>();

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
            Debug.Log("if da porta");

            if (interacting)
                Debug.Log("if interagindo");

            Open();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && interacting)
            Open();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Close();
            interacting = false;
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
