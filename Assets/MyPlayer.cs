using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{

    public float velocidade = 4f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRend;
    private MyDash _dash;
    private MyInput _input;

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
        if (!_dash.dashing) 
            rb.velocity = new Vector2(horizontal, vertical).normalized * velocidade;

    }
}
