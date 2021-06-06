using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionModel: MonoBehaviour, IDirect, IArea
{
    public virtual int range { get; protected set; }
    public virtual int PACost { get; protected set; }
    public virtual PossibleTargets targetType { get; protected set; }
    public virtual bool isInstant { get; protected set; }
    public virtual bool multiTargetsOnly { get; protected set; }
    public virtual int efeito { get; protected set; }
    public virtual float chargeTimeMax { get; protected set; }
    public virtual float CD { get; protected set; }
    public virtual int AOE { get; protected set; }
    public virtual Shapes shapeType { get; private set; }
    public virtual int targetsNum { get; protected set; }
    public List<GameObject> targets { get; protected set; }
    public float chargeTime { get; private set; }
    public bool charging { get; private set; }
    public bool activated { get; private set; }
    public int onButton { get; private set; }
    public GridManager _GridManager { get; private set; }
    public Player actionMaker { get; private set; }
    public InputSys actionMakerInput { get; private set; }
    public Transform actionChild { get; private set; }
    public Transform skillHolder { get; private set; }
    public Sprite sprite { get; private set; }

    void Start()
    {
        actionMaker = FindObjectOfType<Player>();
        actionMakerInput = actionMaker.GetComponent<InputSys>();
        skillHolder = gameObject.transform.parent;
        onButton = FindStoredButton();
        sprite = GetSkillSprite();
        //actionMakerInput.GetSkillButton(onButton);
        _GridManager = FindObjectOfType<GridManager>();
    }

    void Update()
    {

        if (activated)
        {
            if (actionMaker.PA <= PACost)
            {
                Fail();
            }
            if (_GridManager.onSearchMode == true)
                WaitTarget();

            if (charging)
                Charge();
        }
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
        actionMakerInput.holdingSkill = true;
        if (isInstant)
            ChargeIni();
        else
        {
            SendTargetRequest();
        }
        actionMaker.GetComponent<ColorSys>().AttackColor();
    }
    public virtual void SendTargetRequest()
    {
        if (AOE <= 0)
            _GridManager.StartSearchMode(true, PassRange(), PassActionMaker(), multiTargetsOnly, PassDesiredTargets());
        else if (AOE > 0)
            _GridManager.StartSearchMode(true, PassRange(), PassActionMaker(), AOE, shapeType, PassDesiredTargets());
    }
    public List<string> PassDesiredTargets()
    {
        List<string> desiredTargets = new List<string>();
        if (targetType == PossibleTargets.Enemy)
            desiredTargets.Add("Enemy");
        else if (targetType == PossibleTargets.Ally)
            desiredTargets.Add("Ally");
        else if (targetType == PossibleTargets.SelfAndAlly)
        {
            desiredTargets.Add("Ally");
            desiredTargets.Add("Player");
        }
        else if (targetType == PossibleTargets.Any)
        {
            desiredTargets.Add("Enemy");
            desiredTargets.Add("Ally");
            desiredTargets.Add("Player");
        }
        return desiredTargets;
    }
    public int PassRange()
    {
        return range;
    }
    public Transform PassActionMaker()
    {
        return actionMaker.transform;
    }
    public virtual void WaitTarget()
    {
        actionMaker.GetComponent<Movement>().Lento(true);
        if (_GridManager.timesTargetWasSent >= targetsNum && _GridManager.targetUnits.Any())
        {
            _GridManager.SearchMode(false);
            targets = _GridManager.targetUnits.ToList();
            _GridManager.TargetArrow(_GridManager.targetUnits);
            _GridManager.targetUnits.Clear();
            ChargeIni();
            actionMaker.GetComponent<Movement>().Lento(false);
        }
        else if (Interruption())
        {
            // Debug.LogError("An interruption to targeting ocurred");
            ResetTargetParams();
            Fail();
        }
    }
    public void ChargeIni()
    {
        //_GridManager.TargetedOutline(targets);
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
            ResetTargetParams();
            CustarAP();
            MakeEffect();
        }
    }
    public void ResetChargeParams()
    {
        charging = false;
        chargeTime = 0;
        if (actionMaker != null) actionMaker.GetComponent<ColorSys>().DefaultColor();
        actionMaker.GetComponent<Movement>().PermitirMovimento(true);
    }
    public void ResetTargetParams()
    {
        actionMaker.GetComponent<Movement>().Lento(false);
        _GridManager.SearchMode(false);
        if (targets != null)
        {
            if (targets.Any()) _GridManager.ResetArrow(targets);
        }
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
        foreach (GameObject target in targets)
        {
            try
            {
                target.GetComponent<EntityModel>().AlterarPV(efeito);
            }
            catch (Exception)
            {
                Debug.LogError("EntityModel was null while applying effect");
            }
            SpecificEffect();
        }
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
        _GridManager.ResetParams();
        actionMakerInput.holdingSkill = false;
    }
    public int FindStoredButton()
    {
        var _index = -1;
        for (int i = 0; i < skillHolder.childCount; i++)
        {
            if (skillHolder.GetChild(i) == gameObject.transform)
            {
                if (i != 10)
                    _index = i + 1;
                else
                    _index = 0;
            }
        }
        // Debug.Log("Ataque basico em " + _index);
        return _index;
    }

    public virtual void SpecificEffect()
    {
        return;
    }

    public Sprite GetSkillSprite()
    {
        return Resources.Load<Sprite>("Sprites/" + this.GetType().Name);
    }
}