using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenenoDoSapoLanceiro_Script : MonoBehaviour
{
    //Variaveis
    private bool ativo = false,
                 seMovendo = false;

    private Vector3 posicao, //Guarda a posicao da magia no mapa do jogo
                    vel; //Guarda a velocidade da magia em direcao ao seu alvo

    private Animator animator; //Guarda o Animator da magia

    private float tempo, //Guarda o tempo para ser usado na animacao da magia
                  tempoMax; //Guarda o tempo te duracao da animacao da magia

    public EntityModel actionMaker;//Guarda o objeto que esta lancando a magia
    private SpriteRenderer actionMakerSprite, //Guarda o sprite do objeto que esta lancando a magia
                           sprite; //Guarda o sprite da magia
    private Animator actionMakerAnimator; //Guarda o Animator do objeto que esta lancando a magia
    public Animacao ActionMakerAnimation { get; private set; } //Guarda o script de animacao do objeto que esta lancando a magia

    private Vector3 alvo; //Guarda a posicao que a magia deve acertar

    public Animation animacaoAtual;

    // Update is called once per frame
    void Update()
    {
        if(ativo == true)
        {
            SeMover();
            ativo = false;
        }

        if (seMovendo == true)
        {
            posicao += vel / tempoMax * Time.deltaTime;

            tempo += Time.deltaTime;

            transform.position = posicao;

            if (tempo >= tempoMax)
            {
                sprite.flipX = false;
                seMovendo = false;
                AnimacaoExplodindo();
            }
        }
    }

    public void Iniciar(EntityModel actionCaller, List<GameObject> targets)
    {
        posicao = transform.position;
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        tempo = 0;
        tempoMax = 1f;

        actionMaker = actionCaller;
        actionMakerSprite = actionMaker.GetComponent<SpriteRenderer>();
        ActionMakerAnimation = actionMaker.GetComponent<Animacao>();
        actionMakerAnimator = actionMaker.GetComponent<Animator>();
        alvo = targets[0].transform.position;

        switch(actionMakerAnimator.GetFloat("Direcao"))
        {
            case (float)Animacao.Direcao.Baixo:
                posicao.x = actionMakerSprite.bounds.center.x;
                posicao.y = (float)(actionMakerSprite.bounds.center.y - 0.5);
                animator.SetFloat("Direcao", (float)Animacao.Direcao.Baixo);
                break;

            case (float)Animacao.Direcao.Cima:
                posicao.x = actionMakerSprite.bounds.center.x;
                posicao.y = (float)(actionMakerSprite.bounds.center.y + 0.5);
                animator.SetFloat("Direcao", (float)Animacao.Direcao.Cima);
                break;

            case (float)Animacao.Direcao.Lado:
                posicao.y = actionMakerSprite.bounds.center.y;

                if(actionMakerSprite.flipX == true)
                {
                    sprite.flipX = true;
                    posicao.x = (float)(actionMakerSprite.bounds.center.x - 0.5);
                }
                else
                {
                    posicao.x = (float)(actionMakerSprite.bounds.center.x + 0.5);
                }
                animator.SetFloat("Direcao", (float)Animacao.Direcao.Lado);
                break;

        }

        transform.position = posicao;

        vel = alvo - posicao;

        Debug.Log("Entrou na funcao");
        ActionMakerAnimation.TrocarAnimacao("Lancando Magia");

        sprite.enabled = false;
        ativo = true;
    }

    public void SeMover()
    {
        sprite.enabled = true;
        AnimacaoNoAr();
    }

    public void AnimacaoEventoAcertarAtaque()
    {
        ActionMakerAnimation.acertandoAtaque = true;
    }

    public void AnimacaoEventoTerminarAtaque()
    {
        ActionMakerAnimation.terminandoAtaque = true;
        Destroy(gameObject);
    }

    private void AnimacaoNoAr()
    {
        seMovendo = true;
        animator.Play("No Ar");
    }

    private void AnimacaoExplodindo()
    {
        animator.Play("Explodindo");
    }
}
