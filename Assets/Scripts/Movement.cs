using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MovementModel
{
    private Rigidbody2D rb;
    private Dash _dash;
    private InputSys _input;
    private BoxCollider2D bColl2d;
    public bool isClimbing;
    public List<float> lastCoordinates;
    public Animacao animacao;

    void Start()
    {
        lastCoordinates = new List<float>() { 0, 0 };
        rb = GetComponent<Rigidbody2D>();
        _dash = GetComponent<Dash>();
        _input = GetComponent<InputSys>();
        bColl2d = GetComponent<BoxCollider2D>();
        animacao = GetComponent<Animacao>();
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
            animacao.AnimacaoMovimento(horizontal, vertical);
        }
        else
        {
            velocidadeM = 0;
        }

        //Faz o personagem se mover, utilizando sua velocidade base e o modificador da velocidade
        rb.velocity = new Vector2(horizontal, vertical).normalized * (velocidade * velocidadeM);

        //Roda a animacao
        animacao.Animar();
    }
}