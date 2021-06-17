using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animacao : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRend;
    public MovementModel movementModelScript;
    private Animator animator;
    public TextAsset tabelaDeAnimacoes;

    //Variaveis para as animacoes
    public bool acertandoAtaque = false,
                terminandoAtaque = false;

    //Enumerador das direcoes do personagem
    public enum Direcao : int
    {
        Baixo,
        Lado,
        Cima
    };

    public string animacao = "Idle"; //A animacao atual do personagem

    public Dictionary<string, string> animacaoLista = new Dictionary<string, string>(); 

    //Guarda as posicoes para calcular a velocidade dos inimigos
    private Vector3 posicaoAnterior,
                    posicaoAtual;

    //Guarda as velocidades dos inimigos
    float velocidadeX,
          velocidadeY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        movementModelScript = GetComponent<MovementModel>();
        animator = GetComponent<Animator>();

        ReadCsv(tabelaDeAnimacoes, 2);
    }

    // Fixed Update is called
    void FixedUpdate()
    {
        posicaoAtual = transform.position;

        velocidadeX = posicaoAtual.x - posicaoAnterior.x;
        velocidadeY = posicaoAtual.y - posicaoAnterior.y;

        posicaoAnterior = transform.position;

        //Debug.Log("Velocidade X: " + velocidadeX + "\nVelocidadeY" + velocidadeY);
    }

    // Update is called once per frame
    void Update()
    {
        //Muda a animacao caso o personagem possa se mover
        if(movementModelScript._permissaoAndar == true)
        {
            if(transform.CompareTag("Player"))
            {
                AnimacaoMovimento();
            }
            else if(transform.CompareTag("Enemy"))
            {
                Debug.Log("Entrou na Funcao do Inimigo");
                AnimacaoMovimentoInimigo();
            }
        }
    }

    public void AnimacaoMovimento()
    {
        Movement movementScript = (Movement)movementModelScript;

        if (movementScript.isClimbing)
        {
            spriteRend.flipX = false;
            animator.SetFloat("Direcao", (float)Direcao.Cima);
            TrocarAnimacao("Subindo Escadas");
        }
        else
        {
            if (rb.velocity.x != 0 || rb.velocity.y != 0)
            {
                TrocarAnimacao("Andando");

                if (rb.velocity.y < 0)
                {
                    spriteRend.flipX = false;
                    animator.SetFloat("Direcao", (float)Direcao.Baixo);
                }
                else if (rb.velocity.y > 0)
                {
                    spriteRend.flipX = false;
                    animator.SetFloat("Direcao", (float)Direcao.Cima);
                }
                else if (rb.velocity.x < 0)
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
            else if (rb.velocity.x == 0 && rb.velocity.y == 0)
            {
                TrocarAnimacao("Idle");
            }
        }
    }

    public void AnimacaoMovimentoInimigo()
    {
        if (velocidadeX != 0 || velocidadeY != 0)
        {
            TrocarAnimacao("Andando");

            if (velocidadeY < 0)
            {
                spriteRend.flipX = false;
                animator.SetFloat("Direcao", (float)Direcao.Baixo);
            }
            else if (velocidadeY > 0)
            {
                spriteRend.flipX = false;
                animator.SetFloat("Direcao", (float)Direcao.Cima);
            }

            if (velocidadeX < 0)
            {
                if (velocidadeY > -0.01 && velocidadeY < 0.01)
                {
                    spriteRend.flipX = true;
                    animator.SetFloat("Direcao", (float)Direcao.Lado);
                }
            }
            else if (velocidadeY > 0)
            {
                if (velocidadeY > -0.01 && velocidadeY < 0.01)
                {
                    spriteRend.flipX = false;
                    animator.SetFloat("Direcao", (float)Direcao.Lado);
                }
            }
        }
        else if (velocidadeX == 0 && velocidadeY == 0)
        {
            TrocarAnimacao("Idle");
        }
    }

    public void TrocarAnimacao(string novaAnimacao)
    {
        animacao = novaAnimacao;
        animator.Play(animacaoLista[animacao]);

        /*
        Debug.Log("Animacao: " + animacao + " Tipo: " + animacao.GetType());
        Debug.Log("Animacao do Dicionario: " + animacaoLista[animacao] + " Tipo: " + animacaoLista[animacao].GetType());
        Debug.Log("Sao iguais? R: " + (animacao == animacaoLista[animacao]));
        */
    }

    public void AnimacaoEventoAcertarAtaque()
    {
        acertandoAtaque = true;
    }

    public void AnimacaoEventoTerminarAtaque()
    {
        terminandoAtaque = true;
    }

    public void DefinirDirecaoAtaque(int AOE, Vector3 pointClicked, List<GameObject> targets)
    {
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

    public void ResetarParametrosDasAnimacoes()
    {
        acertandoAtaque = false;
        terminandoAtaque = false;
    }

    void ReadCsv(TextAsset textAsset, int colunas)
    {
        string[] data = textAsset.text.Split(new string[] { ";", "\r\n" }, StringSplitOptions.None);

        int tablesize = data.Length / colunas - 1;

        //Debug.Log(data[0] + "," + data[1]);
        for (int i = 0; i < tablesize; i++)
        {
            //Debug.Log(data[colunas * (i + 1)] + ", " + data[colunas * (i + 1) + 1]);
            animacaoLista.Add(data[colunas * (i + 1)], data[colunas * (i + 1) + 1]);
        }
        //Debug.Log("Fim da Tabela");
    }
}
