using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDash : MonoBehaviour
{
    public float timeDash;
    private float startTimeDash = .4f;
    public bool dashing;
    private MyPlayer _player;
    private MyInput _input;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRend;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<MyPlayer>();
        _input = FindObjectOfType<MyInput>();
        _rb = _player.GetComponent<Rigidbody2D>();
        _spriteRend = _player.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (dashing == true)
            DoDash();
    }
    private void DoDash()
    {
        _spriteRend.color = Color.blue;
        timeDash -= Time.deltaTime;
        Debug.LogWarning($"Dashing at {_rb.velocity} with {timeDash} remaining.");
        if (timeDash <= 0)
        {
            timeDash = 0;
            dashing = false;
            _spriteRend.color = Color.red;
        }
    }

    public void startDash()
    {
        if (timeDash <= 0 && (_input.horizontal != 0 || _input.vertical != 0))
        {
            timeDash = startTimeDash;
            dashing = true;

            if (_input.horizontal == 1)
            {
                _rb.AddForce(Vector2.right * _player.velocidade / 5000, ForceMode2D.Impulse);
            }
            else if (_input.horizontal == -1)
            {
                _rb.AddForce(Vector2.left * _player.velocidade / 5000, ForceMode2D.Impulse);
            }
            if (_input.vertical == 1)
            {
                _rb.AddForce(Vector2.up * _player.velocidade / 5000, ForceMode2D.Impulse);
            }
            else if (_input.vertical == -1)
            {
                _rb.AddForce(Vector2.down * _player.velocidade / 5000, ForceMode2D.Impulse);
            }
        }
            
    }
}
