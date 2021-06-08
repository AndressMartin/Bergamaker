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
    public bool skillPress { get; set; }
    public int skillNum { get; private set; }
    public bool selectPress { get; set; }
    public bool holdingSkill { get; set; }
	public bool interactPress { get; private set; }
    private GameObject SkillManager;
    public List<ActionModel> quickActions = new List<ActionModel>();

    public List<int> buttons = new List<int>();
    private void Start()
    {
        SkillManager = GameObject.FindGameObjectWithTag("SkillHolder");
        foreach (Transform child in SkillManager.transform)
        {
            quickActions.Add(child.GetComponent<ActionModel>());
            Debug.Log(child.GetComponent<ActionModel>().GetType());
        }
    }
    private void Update()
    {
        MovePress(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        DashPress(Input.GetButtonDown("Dash"));
        CancelPress(Input.GetButtonDown("Cancel"));
		
		if(Input.anyKeyDown)
            GetInteract(Input.GetButtonDown("Interact"));
		
        if (holdingSkill == false)
        {
            if (Input.GetButtonDown("Skill1"))
            {
                skillPress = Input.GetButtonDown("Skill1");
                skillNum = 1;
            }
            else if (Input.GetButtonDown("Skill2"))
            {
                skillPress = Input.GetButtonDown("Skill2");
                skillNum = 2;
            }
            else if (Input.GetButtonDown("Skill3"))
            {
                skillPress = Input.GetButtonDown("Skill3");
                skillNum = 3;
            }
            else if (Input.GetButtonDown("Skill4"))
            {
                skillPress = Input.GetButtonDown("Skill4");
                skillNum = 4;
            }
            else if (Input.GetButtonDown("Skill5"))
            {
                skillPress = Input.GetButtonDown("Skill5");
                skillNum = 5;
            }
            if (skillNum > 0) GetChosenSkill(skillNum);
        }
        GetSelectPress(Input.GetButtonDown("Select"));
        //FindSkillPressed(Input.anyKeyDown);
    }

    private void GetChosenSkill(int num)
    {
        quickActions[num-1].Activate(GetComponent<EntityModel>());
        skillPress = false;
        holdingSkill = true;
        skillNum = 0;
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

    //public void FirstSkillPress(bool _firstSkillPress)
    //{
    //    skillPress = _firstSkillPress;
    //    skillNum = 1;
    //}
    //public bool FirstSkillPress()
    //{
    //    return Input.GetButtonDown("Skill1");
    //}
    //public void SecondSkillPress(bool _secondSkillPress)
    //{
    //    skillPress = _secondSkillPress;
    //    skillNum = 2;
    //}
    //public bool SecondSkillPress()
    //{
    //    return Input.GetButtonDown("Skill2");
    //}
    public void GetSelectPress(bool _selectButtonPress)
    {
        selectPress = _selectButtonPress;
    }
    public bool GetSelectPress()
    {
        return selectPress;
    }
    public void GetSkillButton(int button)
    {
        throw new NotImplementedException();
        //buttons.Add(button);
    }
	public void GetInteract(bool _interactPress)
    {
        interactPress = _interactPress;
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