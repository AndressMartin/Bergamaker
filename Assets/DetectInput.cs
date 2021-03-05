using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class DetectInput : MonoBehaviour
{
    public InputMaster controls;
    public InputActionMap playerMap;

    private void Awake()
    {
        //playerMap = controls.asset.FindActionMap("Player");
        //playerMap.Dash
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
}
