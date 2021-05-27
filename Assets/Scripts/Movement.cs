using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float velocidade = 4f, //Velocidade do personagem
                 velocidadeM = 1f; //Modificador da velocidade do personagem, para quando ele ficar lento ou subir escadas

    private Rigidbody2D rb;
    private SpriteRenderer spriteRend;
    private Dash _dash;
    private InputSys _input;
    private BoxCollider2D bColl2d;
    private Animator animator;
    public bool _permissaoAndar = true;
    public bool lento;
    public bool isClimbing;

    private int animacao = 0; //A animacao atual do personagem

    //Enumerador das direcoes do personagem
    private enum Direcao : int
    {
        Baixo,
        Lado,
        Cima
    };

    //Enumerador das animacoes do personagem
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
            //Altera o valor do modificador da velocidade do personagem
            if (lento || isClimbing)
            {
                if (lento)
                {
                    velocidadeM = 0.4f;
                }

                if (isClimbing)
                {
                    velocidadeM = 0.6f;
                }
            }
            else
            {
                velocidadeM = 1;
            }

            //Faz o personagem se mover, utilizando sua velocidade base e o modificador da velocidade
            rb.velocity = new Vector2(horizontal, vertical).normalized * (velocidade * velocidadeM);
        }

        //Define a animacao
        DefinirAnimacao(horizontal, vertical);

        //Roda a animacao
        Animar();
    }

    private void DefinirAnimacao(float horizontal, float vertical)
    {
        if (isClimbing)
        {
            spriteRend.flipX = false;
            animator.SetFloat("Direcao", (float)Direcao.Cima);
            animacao = (int)AnimacaoEnum.SubindoEscadas;
        }
        else
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
    }

    private void Animar()
    {
        switch (animacao)
        {
            case (int)AnimacaoEnum.Idle:
                animator.Play("Idle");
                break;

            case (int)AnimacaoEnum.Andando:
                animator.Play("Andando");
                break;

            case (int)AnimacaoEnum.SubindoEscadas:
                animator.Play("Subindo Escadas");
                break;
        }
    }

    public void PermitirMovimento(bool permissao)
    {
        _permissaoAndar = permissao;
        if (permissao == false) rb.velocity = new Vector2(0f, 0f);
    }

    public void Lento(bool condicao)
    {
        lento = condicao;
    }
}