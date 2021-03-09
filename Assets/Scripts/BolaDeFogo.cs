using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaDeFogo : MonoBehaviour, IAction, IMagic
{
    public Targeter _targeter { get; private set; }
    public int PACost { get; private set; } = 50;
    public int range { get; private set; } = 6;
    public int efeito { get; private set; } = -60;
    public float chargeTimeMax { get; private set; } = 3f;
    public float chargeTime { get; private set; }
    public bool charging { get; private set; }
    public float CD { get; private set; } = 5f;
    public bool IsInstant { get; private set; } = false;
    public GameObject target { get; private set; }
    public Player actionMaker { get; private set; }
    public InputSys actionMakerInput { get; private set; }
    public Transform actionChild { get; private set; }
    public Transform SkillHolder { get; private set; }
    public int onButton { get; private set; }
    public bool isEnemy { get; } = true;
    public bool activated { get; private set; }

    public int MNCost { get; private set; } = 50;

    // Start is called before the first frame update
    void Start()
    {
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

        if (_targeter.onSearchMode == true && activated)
            WaitTarget();
        if (charging)
            Charge();
        if (actionMakerInput.skillNum == onButton
            && actionMakerInput.skillPress)
        {
            Activated();
            actionMakerInput.skillPress = false;
        }
    }
    public void Activated()
    {
        activated = true;
        if (IsInstant)
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
        _targeter.StartSearchMode(true, PassRange(), PassActionMaker());
        if (isEnemy)
            _targeter.desiredTarget = "Enemy";
        else
            _targeter.desiredTarget = "Player";
        Debug.LogAssertion($"Sent Target Request with {range}");
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
        actionChild.GetComponent<AuraDrawer>().radius = range;

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
        if (Vector2.Distance(actionMaker.transform.position, target.transform.position) <= 2)
        {
            Debug.Log($"{target} is at a distance of {Vector2.Distance(actionMaker.transform.position, target.transform.position)}");
            ChargeIni();
        }
        else
        {
            Debug.LogError($"{target} was super far! Distance: {Vector2.Distance(actionMaker.transform.position, target.transform.position)}");
            Fail();
        }
    }
    public void DeactivateRange()
    {
        actionChild.GetComponent<AuraDrawer>().enabled = false;
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
            MakeEffect();
        }
        if (Interruption())
        {
            Fail();
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
    public void MakeEffect()
    {
        Debug.Log($"Causou {efeito} de dano em {target}");
        target.GetComponent<Creature>().AlterarPV(efeito);
    }
    public void Fail()
    {
        activated = false;
        ResetChargeParams();
        ResetTargetParams();
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

    public void CustarMN()
    {
        throw new NotImplementedException();
    }
}
