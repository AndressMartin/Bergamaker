using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaDeFogo_Script : MonoBehaviour
{
    //Variaveis
    private bool ativo = false,
                 seMovendo = false;

    private Vector3 posicao, //Guarda a posicao da magia no mapa do jogo
                    vel; //Guarda a velocidade da magia em direcao ao seu alvo

    private Animator animator; //Guarda o Animator da magia

    private float tempo, //Guarda o tempo para ser usado na animacao da magia
                  tempoMax, //Guarda o tempo te duracao da animacao da magia
                  alturaP, //Guarda a altura do movimento de parabola que a magia ira simular
                  angulo; //Guarda o angulo para a magia fazer o movimento de subir e descer

    public EntityModel actionMaker;//Guarda o objeto que esta lancando a magia
    private SpriteRenderer actionMakerSprite, //Guarda o sprite do objeto que esta lancando a magia
                           sprite; //Guarda o sprite da magia
    public Animacao actionMakerAnimation { get; private set; } //Guarda o script de animacao do objeto que esta lancando a magia

    private Vector3 alvo; //Guarda a posicao que a magia deve acertar

    // Update is called once per frame
    void Update()
    {
        if (ativo == true)
        {
            if(tempo < 0.5)
            {
                tempo += Time.deltaTime;

                if (tempo >= 0.5)
                {
                    actionMakerAnimation.ResetarParametrosDasAnimacoes();
                    actionMakerAnimation.TrocarAnimacao("Lancando Magia");
                }
            }

            if(actionMakerAnimation.animacao == "Lancando Magia - Looping")
            {
                ativo = false;
                seMovendo = true;
                tempo = 0;
            }
        }

        if(seMovendo == true)
        {
            posicao += vel / tempoMax * Time.deltaTime;
            posicao.y += alturaP * Mathf.Sin(angulo) * Time.deltaTime;
            //Debug.Log("Vel: " + (alturaP * Mathf.Sin(angulo) * Time.deltaTime) + "\nPosicao Y: " + posicao.y);
            //Debug.Log("Angulo: " + angulo + "\nSeno: " + Mathf.Sin(angulo) + "\nPosicao Y: " + posicao.y);

            angulo += (6.28f / tempoMax) * Time.deltaTime;
            tempo += Time.deltaTime;

            transform.position = posicao;

            if(angulo <= 3.14f)
            {
                sprite.flipY = true;
            }
            else
            {
                sprite.flipY = false;
            }

            if (tempo >= tempoMax)
            {
                sprite.flipY = false;
                seMovendo = false;
                AnimacaoExplodindo();
            }
        }
    }

    public void Iniciar(EntityModel actionCaller, Vector3 centerOfAOE)
    {
        posicao = transform.position;
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        tempo = 0;
        tempoMax = 1.5f;
        angulo = 0;

        alturaP = 5;

        actionMaker = actionCaller;
        actionMakerSprite = actionMaker.GetComponent<SpriteRenderer>();
        actionMakerAnimation = actionMaker.GetComponent<Animacao>();
        alvo = centerOfAOE;

        posicao.x = actionMakerSprite.bounds.center.x;
        posicao.y = (float)(actionMakerSprite.bounds.center.y + (actionMakerSprite.bounds.size.y / 2));

        transform.position = posicao;

        vel = alvo - posicao;

        animator.Play("Surgindo");
    }

    public void AnimacaoEventoAcertarAtaque()
    {
        actionMakerAnimation.acertandoAtaque = true;
    }

    public void AnimacaoEventoTerminarAtaque()
    {
        actionMakerAnimation.terminandoAtaque = true;
        Destroy(gameObject);
    }

    private void AnimacaoNoAr()
    {
        ativo = true;
        animator.Play("NoAr");
    }

    private void AnimacaoExplodindo()
    {
        animator.Play("Explodindo");
    }
}