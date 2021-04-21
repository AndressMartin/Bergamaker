using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BolaDeFogo : MonoBehaviour, IAction, IMagic
{
    public Targeter _targeter { get; private set; }
    public int PACost { get; private set; } = 50;
    public int MNCost { get; private set; } = 30;
    public int range { get; private set; } = 4;
    public int efeito { get; private set; } = -30;
    public float chargeTimeMax { get; private set; } = 2f;
    public float chargeTime { get; private set; }
    public bool charging { get; private set; }
    public float CD { get; private set; } = .5f;
    public bool isInstant { get; private set; } = false;
    public bool isArea { get; private set; } = true;
    public int AOE { get; private set; } = 1;
    public bool isEnemy { get; private set; } = true;
    public GameObject target { get; private set; }
    public Player actionMaker { get; private set; }
    public InputSys actionMakerInput { get; private set; }
    public Transform actionChild { get; private set; }
    public Transform SkillHolder { get; private set; }
    public GameObject mouseAuraHolder { get; private set; }
    public int onButton { get; private set; }
    public bool activated { get; private set; }



    // Start is called before the first frame update
    void Start()
    {
        mouseAuraHolder = GameObject.FindGameObjectWithTag("MouseHolder");
        SkillHolder = gameObject.transform.parent;
        onButton = FindStoredButton();
        //actionMakerInput.GetSkillButton(onButton);
        _targeter = FindObjectOfType<Targeter>();
        actionMaker = FindObjectOfType<Player>();
        actionMakerInput = actionMaker.GetComponent<InputSys>();
        actionChild = actionMaker.transform.GetChild(0);
    }
    // Update is called once per frame
    void Update()
    {
        
        if (activated)
        {
            if (_targeter.onSearchMode == true)
                WaitTarget();

            if (charging)
                Charge(); 
            
            if (actionMaker.PA <= PACost)
            {
                Fail();
            }
            if (actionMaker.MN <= MNCost)
            {
                Fail();
            }
        }
        if (actionMakerInput.skillNum == onButton
            && actionMakerInput.skillPress)
        {
            Activated();
            actionMakerInput.skillPress = false;
        }
        //else if (actionMakerInput.skillNum != onButton)
        //{
        //    Fail();
        //}
    }
    public void Activated()
    {
        activated = true;
        actionMakerInput.holdingSkill = true;
        if (isInstant)
            ChargeIni();
        else
        {
            SendTargetRequest();
            ActivateRange();
        }
        FindObjectOfType<ColorSys>().AttackColor();
    }
    public void SendTargetRequest()
    {
        _targeter.StartSearchMode(true, PassRange(), PassActionMaker(), AOE, PassDesiredTarget());
        Debug.LogAssertion($"Sent Target Request with range {range} and desired target {PassDesiredTarget()}");
    }

    private string PassDesiredTarget()
    {
        string desiredTarget;
        if (isEnemy) desiredTarget = "Enemy";
        else desiredTarget = "Ally";
        return desiredTarget;
    }
    public int PassRange()
    {
        return range;
    }
    public Transform PassActionMaker()
    {
        return actionMaker.transform;
    }
    public void ActivateRange()
    {
        actionChild.GetComponent<LineRenderer>().enabled = true;
        actionChild.GetComponent<AuraDrawer>().enabled = true;
        actionChild.GetComponent<AuraDrawer>().update = true;
        actionChild.GetComponent<AuraDrawer>().radius = range;
        if (isArea)
        {
            mouseAuraHolder.GetComponent<LineRenderer>().enabled = true;
            mouseAuraHolder.GetComponent<AuraDrawer>().enabled = true;
            mouseAuraHolder.GetComponent<AuraDrawer>().update = true;
            mouseAuraHolder.GetComponent<AuraDrawer>().radius = AOE;
        }
    }
    public void WaitTarget()
    {
        actionMaker.GetComponent<Movement>().Lento(true);
        if (_targeter.targetUnit != null)
        {
            _targeter.SearchMode(false);
            target = _targeter.targetUnit;
            Debug.LogError($"This is the target: {target}");
            _targeter.targetUnit = null;
            TestDistance();
            actionMaker.GetComponent<Movement>().Lento(false);
        }
        else if (Interruption())
        {
            Debug.LogError("An interruption to targeting ocurred");
            ResetTargetParams();
            Fail();
        }
    }
    public void TestDistance()
    {
        if (Vector2.Distance(actionMaker.transform.position, target.transform.position) <= range)
        {
            Debug.Log($"{target} is at a distance of {Vector2.Distance(actionMaker.transform.position, target.transform.position)}");
            ChargeIni();
        }
        else
        {
            Debug.LogError($"{target} was super far! Distance: {Vector2.Distance(actionMaker.transform.position, target.transform.position)}");
            Fail();
            ResetTargetParams();
        }
    }
    public void DeactivateRange()
    {
        actionChild.GetComponent<LineRenderer>().enabled = false;
        actionChild.GetComponent<AuraDrawer>().enabled = false;
        if (isArea)
        {
            mouseAuraHolder.GetComponent<LineRenderer>().enabled = false;
            mouseAuraHolder.GetComponent<AuraDrawer>().enabled = false;
        }
    }
    public void ChargeIni()
    {
        DeactivateRange();
        _targeter.TargetedOutline(target);
        actionMaker.GetComponent<ColorSys>().ChargingColor();
        charging = true;
        chargeTime = chargeTimeMax;
    }
    public void Charge()
    {
        chargeTime -= Time.deltaTime;
        var actionMakerMove = actionMaker.GetComponent<Movement>();
        actionMakerMove.PermitirMovimento(false);
        if (chargeTime <= 0)
        {
            ResetChargeParams();
            CustarAP();
            CustarMN();
            MakeEffect();
        }
    }
    public void ResetChargeParams()
    {
        charging = false;
        chargeTime = 0;
        actionMaker.GetComponent<ColorSys>().DefaultColor();
        _targeter.ResetMat(target);
        actionMaker.GetComponent<Movement>().PermitirMovimento(true);
    }
    public void ResetTargetParams()
    {
        DeactivateRange();
        actionMaker.GetComponent<Movement>().Lento(false);
        _targeter.SearchMode(false);
        actionMaker.GetComponent<ColorSys>().DefaultColor();
        _targeter.ResetMat(target);
        actionMaker.GetComponent<Movement>().PermitirMovimento(true);
    }
    public bool Interruption()
    {
        var actionMakerInput = actionMaker.GetComponent<InputSys>();
        if (actionMakerInput.CancelPress() ||
            actionMakerInput.DashPress())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void CustarAP()
    {
        actionMaker.AlterarPA(-PACost);
    }
    public void CustarMN()
    {
        actionMaker.AlterarMN(-MNCost);
    }
    public void MakeEffect()
    {
        Debug.Log($"Causou {efeito} de dano em {target}");
        target.GetComponent<Creature>().AlterarPV(efeito);
        End();
    }
    public void Fail()
    {
        ResetChargeParams();
        ResetTargetParams();
        End();
    }
    public void End()
    {
        activated = false;
        _targeter.ResetParams();
        actionMakerInput.holdingSkill = false;
    }
    public int FindStoredButton()
    {
        var _index = -1;
        for (int i = 0; i < SkillHolder.childCount; i++)
        {
            if (SkillHolder.GetChild(i) == gameObject.transform)
            {
                if (i != 10)
                    _index = i + 1;
                else
                    _index = 0;
            }
        }
        Debug.Log("Ataque basico em " + _index);
        return _index;
    }
}
