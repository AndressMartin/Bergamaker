using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MovementModel
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRend;
    private Dash _dash;
    private InputSys _input;
    private BoxCollider2D bColl2d;
    private Animator animator;
    public bool isClimbing;
    public List<float> lastCoordinates;

    //Enumerador das direcoes do personagem
    private enum Direcao : int
    {
        Baixo,
        Lado,
        Cima
    };

    //Enumerador das animacoes do personagem
    public enum AnimacaoEnum
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

    public AnimacaoEnum animacao = AnimacaoEnum.Idle; //A animacao atual do personagem

    void Start()
    {
        lastCoordinates = new List<float>() { 0, 0 };
        rb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        _dash = GetComponent<Dash>();
        _input = GetComponent<InputSys>();
        bColl2d = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (lastCoordinates[0] != _input.horizontal && _input.horizontal != 0) 
        {
            lastCoordinates[0] = _input.horizontal;
            lastCoordinates[1] = 0;
        }

        if (lastCoordinates[1] != _input.vertical && _input.vertical != 0)
        {
            lastCoordinates[1] = _input.vertical;
            lastCoordinates[0] = 0;
        }

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

            //Define a animacao
            AnimacaoMovimento(horizontal, vertical);
        }
        else
        {
            velocidadeM = 0;
        }

        //Faz o personagem se mover, utilizando sua velocidade base e o modificador da velocidade
        rb.velocity = new Vector2(horizontal, vertical).normalized * (velocidade * velocidadeM);

        //Roda a animacao
        Animar();
    }

    

    private void AnimacaoMovimento(float horizontal, float vertical)
    {
        if (isClimbing)
        {
            spriteRend.flipX = false;
            animator.SetFloat("Direcao", (float)Direcao.Cima);
            animacao = AnimacaoEnum.SubindoEscadas;
        }
        else
        {
            if (horizontal != 0 || vertical != 0)
            {
                animacao = AnimacaoEnum.Andando;

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
                animacao = AnimacaoEnum.Idle;
            }
        }
    }

    private void Animar()
    {
        switch (animacao)
        {
            case AnimacaoEnum.Idle:
                animator.Play("Idle");
                break;

            case AnimacaoEnum.Andando:
                animator.Play("Andando");
                break;

            case AnimacaoEnum.SubindoEscadas:
                animator.Play("Subindo Escadas");
                break;

            case AnimacaoEnum.CastandoMagia:
                animator.Play("Castando Magia");
                break;

            case AnimacaoEnum.LancandoMagiaInicio:
                animator.Play("Lancando Magia - Inicio");
                break;

            case AnimacaoEnum.LancandoMagiaLooping:
                animator.Play("Lancando Magia - Looping");
                break;

            case AnimacaoEnum.TomandoDano:
                animator.Play("Tomando Dano");
                break;

            case AnimacaoEnum.Morto:
                animator.Play("Morto");
                break;

            case AnimacaoEnum.AtaqueBasico:
                animator.Play("Ataque Basico");
                break;
        }
    }

    public void AnimacaoEventoAcertarAtaque()
    {
        acertandoAtaque = true;
    }

    public void AnimacaoEventoTerminarAtaque()
    {
        terminandoAtaque = true;
    }

    public void AnimacaoEventoIniciarLancandoMagiaLooping()
    {
        animacao = AnimacaoEnum.LancandoMagiaLooping;
    }

    public override void AnimacaoIniciarCasting()
    {
        animacao = AnimacaoEnum.CastandoMagia;
    }

    public override void DefinirDirecaoAtaque(int AOE, Vector3 pointClicked, List<GameObject> targets)
    {
        Debug.Log("Entrou na funcao");
        float DistanciaX,
              DistanciaY;

        if (AOE > 0)
        {
            DistanciaX = transform.GetChild(0).transform.position.x - pointClicked.x;
            DistanciaY = transform.GetChild(0).transform.position.y - pointClicked.y;

            if (Mathf.Abs(DistanciaX) > Mathf.Abs(DistanciaY))
            {
                if (DistanciaX > 0)
                {
                    spriteRend.flipX = true;
                    animator.SetFloat("Direcao", (float)Direcao.Lado);
                }
                else
                {
                    spriteRend.flipX = false;
                    animator.SetFloat("Direcao", (float)Direcao.Lado);
                }
            }
            else
            {
                if ((DistanciaY) < 0)
                {
                    spriteRend.flipX = false;
                    animator.SetFloat("Direcao", (float)Direcao.Cima);
                }
                else
                {
                    spriteRend.flipX = false;
                    animator.SetFloat("Direcao", (float)Direcao.Baixo);
                }
            }
        }
        else
        {
            DistanciaX = transform.GetChild(0).transform.position.x - targets[0].transform.GetChild(0).transform.position.x;
            DistanciaY = transform.GetChild(0).transform.position.y - targets[0].transform.GetChild(0).transform.position.y;

            if (DistanciaY < 0)
            {
                spriteRend.flipX = false;
                animator.SetFloat("Direcao", (float)Direcao.Cima);
            }
            else
            {
                spriteRend.flipX = false;
                animator.SetFloat("Direcao", (float)Direcao.Baixo);
            }

            if (DistanciaX > 0.3)
            {
                if (DistanciaY >= -0.5 && DistanciaY <= 0.5)
                {
                    spriteRend.flipX = true;
                    animator.SetFloat("Direcao", (float)Direcao.Lado);
                }
            }
            else if (DistanciaX < -0.3)
            {
                if (DistanciaY >= -0.5 && DistanciaY <= 0.5)
                {
                    spriteRend.flipX = false;
                    animator.SetFloat("Direcao", (float)Direcao.Lado);
                }
            }
        }
    }

    public void AnimacaoLancandoMagiaInicio()
    {
        acertandoAtaque = false;
        terminandoAtaque = false;
        animacao = AnimacaoEnum.LancandoMagiaInicio;
    }

    public override void AnimacaoAtaqueBasico()
    {
        acertandoAtaque = false;
        terminandoAtaque = false;
        animacao = AnimacaoEnum.AtaqueBasico;
    }
}