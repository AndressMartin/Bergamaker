using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InputSys : MonoBehaviour
{
    public float horizontal { get; private set; }
    public float vertical { get; private set; }
    public bool dashPress { get; private set; }
    private void Start()
    {
    }
    private void Update()
    {
        GetDash(UnityEngine.Input.GetKeyDown(KeyCode.Space));
        GetMove(UnityEngine.Input.GetAxisRaw("Horizontal"), UnityEngine.Input.GetAxisRaw("Vertical"));
    }

    public void GetDash(bool _dashPress)
    {
        dashPress = _dashPress;
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