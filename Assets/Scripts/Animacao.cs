using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animacao : MonoBehaviour
{
    private SpriteRenderer spriteRend;
    public Movement movementScript;
    private Animator animator;

    //Variaveis para as animacoes
    public bool acertandoAtaque = false,
                terminandoAtaque = false;

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

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        movementScript = GetComponent<Movement>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimacaoMovimento(float horizontal, float vertical)
    {
        if (movementScript.isClimbing)
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

    public void Animar()
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

    public void AnimacaoIniciarCasting()
    {
        animacao = AnimacaoEnum.CastandoMagia;
    }

    public void DefinirDirecaoAtaque(int AOE, Vector3 pointClicked, List<GameObject> targets)
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

    public void AnimacaoAtaqueBasico()
    {
        acertandoAtaque = false;
        terminandoAtaque = false;
        animacao = AnimacaoEnum.AtaqueBasico;
    }
}
