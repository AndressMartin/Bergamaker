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
    private Animator animator;
    public bool _permissaoAndar = true;
    public bool lento;
    private float lentidao = 2.5f;
    public bool isClimbing;

    private enum Direcao : int
    {
        Baixo,
        Lado,
        Cima
    };

    private enum Animacao
    {
        Idle,
        Andando
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
            
            Virar(horizontal, vertical);
        
    }
    private void Virar(float horizontal, float vertical)
    {
        if (horizontal != 0 || vertical != 0)
        {
            animator.SetBool("Andando", true);
            animator.SetInteger("Animacao", (int)Animacao.Andando);
            if (horizontal == -1f)
            {
                spriteRend.flipX = true;
                animator.SetBool("Esquerda", true);

                animator.SetBool("Direita", false);
                animator.SetBool("Cima", false);
                animator.SetBool("Baixo", false);

                animator.SetInteger("Direcao", (int)Direcao.Lado);

            }
            else if (horizontal == +1f)
            {
                spriteRend.flipX = false;
                animator.SetBool("Direita", true);


                animator.SetBool("Esquerda", false);
                animator.SetBool("Cima", false);
                animator.SetBool("Baixo", false);

                animator.SetInteger("Direcao", (int)Direcao.Lado);
            }

            if (vertical == -1f)
            {
                animator.SetBool("Baixo", true);

                animator.SetBool("Direita", false);
                animator.SetBool("Esquerda", false);
                animator.SetBool("Cima", false);

                animator.SetInteger("Direcao", (int)Direcao.Baixo);
            }

            else if (vertical == +1f)
            {
                animator.SetBool("Cima", true);

                animator.SetBool("Direita", false);
                animator.SetBool("Esquerda", false);
                animator.SetBool("Baixo", false);

                animator.SetInteger("Direcao", (int)Direcao.Cima);
            }
            Debug.Log(animator.GetBool("Andando"));
        }
        else if (horizontal == 0 && vertical == 0)
        {
            animator.SetBool("Andando", false);
            animator.SetInteger("Animacao", (int)Animacao.Idle);
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
