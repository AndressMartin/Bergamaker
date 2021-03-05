using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MyInput : MonoBehaviour
{
    private MyDash _dash;
    private MyPlayer _player;

    public float horizontal, vertical;
    private void Start()
    {
        _dash = FindObjectOfType<MyDash>();
        _player = FindObjectOfType<MyPlayer>();
    }
    private void Update()
    {
        GetDash(Input.GetKeyDown(KeyCode.Space));
        GetMove(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    public void GetDash(bool input)
    {
        if (input)
        {
            _dash.startDash();
        }
    }
    public void GetMove(float _horizontal, float _vertical)
    {
        horizontal = _horizontal;
        vertical = _vertical;
    }
}
/*if (timeDash <= 0 && (horizontal != 0 || vertical != 0))
                startDash();
*/