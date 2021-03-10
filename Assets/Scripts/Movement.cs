using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float velocidade = 4f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRend;
    private Dash _dash;
    private InputSys _input;
    private BoxCollider2D bColl2d;
    public bool _permissaoAndar = true;
    public bool lento;
    private float lentidao = 2.5f;
    public bool isClimbing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        _dash = GetComponent<Dash>();
        _input = GetComponent<InputSys>();
        bColl2d = GetComponent<BoxCollider2D>();
    }
    private void FixedUpdate()
    {
        Move(_input.horizontal, _input.vertical);

    }

    public void Move(float horizontal, float vertical)
    {
        if (_permissaoAndar)
        {
            if (!_dash.dashing && !isClimbing)
            {
                if (!lento)
                    rb.velocity = new Vector2(horizontal, vertical).normalized * velocidade;
                else
                    rb.velocity = new Vector2(horizontal, vertical).normalized * velocidade / lentidao;
            }
            if (isClimbing)
            {
                rb.velocity = new Vector2(0f, vertical).normalized * velocidade / lentidao;
            }
        }
            
            Virar(horizontal, vertical);
        
    }
    private void Virar(float horizontal, float vertical)
    {
        if (horizontal == -1f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (horizontal == +1f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    public void PermitirMovimento(bool permissao)
    {
        _permissaoAndar = permissao;
        rb.velocity = new Vector2(0f, 0f);
    }

    public void Lento(bool condicao)
    {
        lento = condicao;
    }

    
}
