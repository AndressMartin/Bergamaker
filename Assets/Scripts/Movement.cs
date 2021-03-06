using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float velocidade = 4f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRend;
    private MyDash _dash;
    private MyInput _input;
    public bool _permissaoAndar { get; set; } = true;
    public bool lento { get; private set; }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        _dash = FindObjectOfType<MyDash>();
        _input = FindObjectOfType<MyInput>();
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
                    rb.velocity = new Vector2(horizontal, vertical).normalized * velocidade/2;
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

    public void ChargingColor()
    {
        spriteRend.color = Color.yellow;
    }

    public void DefaultColor()
    {
        spriteRend.color = Color.green;
    }
    public void AttackColor()
    {
        spriteRend.color = Color.red;
    }

    public void DashingColor()
    {
        spriteRend.color = Color.blue;
    }
}
