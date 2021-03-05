using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour
{
    private Collider2D colisao;
    private SpriteRenderer sprite;
    
    public Player jogador;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        jogador = FindObjectOfType<Player>();

        //Achar o collider sem trigger
        Collider2D[] mbs = GetComponents<Collider2D>();
        foreach (Collider2D mb in mbs)
            if (mb.isTrigger == false)
                colisao = mb;

    }
   
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag ("Jogador"))
        {
            if (jogador.interagindo)
                Open();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject..CompareTag("Jogador") && jogador.interagindo)
            Open();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Jogador")
        {
            Close();
            jogador.AlterarInteracao(false);
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
