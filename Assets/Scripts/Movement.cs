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
    public bool _permissaoAndar = true;
    public bool lento;
    private float lentidao = 2.5f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        _dash = FindObjectOfType<Dash>();
        _input = FindObjectOfType<InputSys>();
    }
    private void FixedUpdate()
    {
        Move(_input.horizontal, _input.vertical);
    }

    public void Move(float horizontal, float vertical)
    {
        if (_permissaoAndar)
            if (!_dash.dashing)
            {
                if (!lento)
                    rb.velocity = new Vector2(horizontal, vertical).normalized * velocidade;
                else
                    rb.velocity = new Vector2(horizontal, vertical).normalized * velocidade/lentidao;
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
