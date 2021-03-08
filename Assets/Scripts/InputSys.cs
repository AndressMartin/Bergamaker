using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InputSys : MonoBehaviour
{
    public float horizontal { get; private set; }
    public float vertical { get; private set; }
    public bool dashPress { get; private set; }
    public bool cancelPress { get; private set; }
    public bool skillPress { get; private set; }
    public int skillNum { get; private set; }
    public bool interacPress { get; private set; }
    public List<int> buttons = new List<int>();
    private void Start()
    {
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            InteractPress(Input.GetButtonDown("Interact"));
            DashPress(Input.GetButtonDown("Dash"));
            CancelPress(Input.GetButtonDown("Cancel"));
            //FindSkillPressed(Input.anyKeyDown);
            FirstSkillPress(Input.GetButtonDown("Skill1"));
        }
        MovePress(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    }

    public void InteractPress(bool _interacPress)
    {
        Debug.LogWarning("Interarc press");
        interacPress = _interacPress;
    }

    public void DashPress(bool _dashPress)
    {
        dashPress = _dashPress;
    }
    public bool DashPress()
    {
        return Input.GetButtonDown("Dash");
    }
    public void MovePress(float _horizontal, float _vertical)
    {
        horizontal = _horizontal;
        vertical = _vertical;
    }
    public void CancelPress(bool _cancelPress)
    {
        cancelPress = _cancelPress;
    }
    public bool CancelPress()
    {
        return Input.GetButtonDown("Cancel");
    }

    public void FirstSkillPress(bool _firstSkillPress)
    {
        skillPress = _firstSkillPress;
        skillNum = 1;
    }
    public bool FirstSkillPress()
    {
        return Input.GetButtonDown("Skill1");
    }
    public void GetSkillButton(int button)
    {
        throw new NotImplementedException();
        //buttons.Add(button);
    }
    //public int FindSkillPressed(KeyCode keyCode)
    //{
    //    switch (keyCode)
    //    {
    //        case KeyCode.Alpha1:
    //            return 1;
    //            break;
    //        case KeyCode.Alpha2:
    //            return 2;
    //            break;
    //        case KeyCode.Alpha3:
    //            return 3;
    //            break;
    //        default:
    //            return 100; //TODO: Find a better way
    //            break;
    //    }
    //}
}
/*if (timeDash <= 0 && (horizontal != 0 || vertical != 0))
                startDash();
*/