﻿using System;
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
    private Animator animator;
    public bool _permissaoAndar = true;
    public bool lento;
    private float lentidao = 2.5f;
    public bool isClimbing;

    private int animacao = 0;

    private enum Direcao : int
    {
        Baixo,
        Lado,
        Cima
    };

    private enum AnimacaoEnum
    {
        Idle,
        Andando,
        SubindoEscadas,
        CastandoMagia,
        LancandoMagiaInicio,
        LancandoMagiaLooping,
        TomandoDano,
        Morto,
        AtaqueBasico
    };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        _dash = GetComponent<Dash>();
        _input = GetComponent<InputSys>();
        bColl2d = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }
    private void Update()
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

        DefinirAnimacao(horizontal, vertical);
        Animar();
    }

    private void DefinirAnimacao(float horizontal, float vertical)
    {
        if (horizontal != 0 || vertical != 0)
        {
            animacao = (int)AnimacaoEnum.Andando;

            if (horizontal == -1f)
            {
                spriteRend.flipX = true;
                animator.SetFloat("Direcao", (float)Direcao.Lado);
            }
            else if (horizontal == +1f)
            {
                spriteRend.flipX = false;
                animator.SetFloat("Direcao", (float)Direcao.Lado);
            }

            if (vertical == -1f)
            {
                spriteRend.flipX = false;
                animator.SetFloat("Direcao", (float)Direcao.Baixo);
            }
            else if (vertical == +1f)
            {
                spriteRend.flipX = false;
                animator.SetFloat("Direcao", (float)Direcao.Cima);
            }
        }
        else if (horizontal == 0 && vertical == 0)
        {
            animacao = (int)AnimacaoEnum.Idle;
        }
    }

    private void Animar()
    {
        switch(animacao)
        {
            case (int)AnimacaoEnum.Idle:
                animator.Play("Idle");
                break;

            case (int)AnimacaoEnum.Andando:
                animator.Play("Andando");
                break;
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