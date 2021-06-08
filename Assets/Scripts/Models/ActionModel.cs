using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionModel: MonoBehaviour, IDirect, IArea, IMagic, ISkill
{
    public virtual int range { get; protected set; }
    public virtual int PACost { get; protected set; }
    public virtual int MNCost { get; protected set; }
    public virtual PossibleTargets targetType { get; protected set; }
    public virtual bool isInstant { get; protected set; }
    public virtual bool multiTargetsOnly { get; protected set; }
    public virtual int efeito { get; protected set; }
    public virtual float chargeTimeMax { get; protected set; }
    public virtual float CD { get; protected set; }
    public virtual int AOE { get; protected set; }
    public virtual Shapes shapeType { get; private set; }
    public virtual int targetsNum { get; protected set; }
    public virtual bool HasAOEEffect { get; protected set; }
    public virtual TerrainEffects EffectType { get; protected set; }
    public List<GameObject> targets { get; protected set; }
    public float chargeTime { get; private set; }
    public bool charging { get; private set; }
    public bool activated { get; private set; }
    public int onButton { get; private set; }
    public GridManager _GridManager { get; private set; }
    public EntityModel actionMaker { get; private set; }
    public Movement actionMakerMove { get; private set; }
    public Transform actionChild { get; private set; }
    public Transform skillHolder { get; private set; }
    public Sprite sprite { get; private set; }
    public List<Vector3> AOEArea { get; private set; }
    public Vector3 pointClicked { get; private set; }
    public List<GameObject> ArcAttacks;
    private void StartingParameters()
    {
        if (AOE > 0) AOEArea = new List<Vector3>();
        actionMakerMove = actionMaker.GetComponent<Movement>();
        skillHolder = gameObject.transform.parent;
        if (actionMaker.GetComponent<Player>() != null) onButton = FindStoredButton();
        sprite = GetSkillSprite();
        //actionMakerInput.GetSkillButton(onButton);
        _GridManager = FindObjectOfType<GridManager>();
    }

    void Update()
    {
        if (activated)
        {
            Debug.Log("BBBBBBBBBBB");
            if (_GridManager.onSearchMode == true)
                WaitTarget();

            if (charging)
                Charge();
        }
    }
    public void Activate(EntityModel actionCaller)
    {
        actionMaker = actionCaller;
        if (actionMaker.PA <= PACost)
        {
            Debug.Log("No AP for execution");
            Fail();
            return;
        }
        StartingParameters();
        activated = true;
        if (isInstant)
            ChargeIni();
        else
        {
            SendTargetRequest();
        }
    }
    public virtual void SendTargetRequest()
    {
        if (AOE <= 0)
            _GridManager.StartSearchMode(true, PassRange(), PassActionMaker(), multiTargetsOnly, PassDesiredTargets());
        else if (AOE > 0)
            _GridManager.StartSearchMode(true, PassRange(), PassActionMaker(), AOE, HasAOEEffect, shapeType, PassDesiredTargets());
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
        Debug.Log("CCCCCCCCCC");
        if (actionMakerMove != null) actionMakerMove.Lento(true);
        if (_GridManager.timesTargetWasSent >= targetsNum && _GridManager.targetUnits.Any())
        {
            AOEArea = _GridManager.SendAOEArea().ToList();
            pointClicked = _GridManager.SendPointClicked();
            _GridManager.SearchMode(false);
            targets = _GridManager.targetUnits.ToList();
            _GridManager.TargetArrow(_GridManager.targetUnits);
            _GridManager.targetUnits.Clear();
            ChargeIni();
            if (actionMakerMove != null) actionMakerMove.Lento(false);
        }
        else if (Interruption())
        {
            // Debug.LogError("An interruption to targeting ocurred");
            Fail();
        }
    }
    public void ChargeIni()
    {
        //_GridManager.TargetedOutline(targets);
        if (AOE <= 0) 
        {
            foreach (var target in targets)
            {
                CreateArc(target);
            }
        }
        charging = true;
        chargeTime = chargeTimeMax;
    }

    private void CreateArc(GameObject target)
    {
        GameObject ArcAttack = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/ArcAttack"), actionMaker.transform);
        //Set begin, middle and endPoints
        ArcAttack.transform.GetChild(1).GetChild(0).position = actionMaker.transform.position;
        var localPosB = actionMaker.transform.InverseTransformPoint(target.transform.position);
        var diffX = localPosB.x;
        var diffY = localPosB.y;
        ArcAttack.transform.GetChild(1).GetChild(2).position = target.transform.position;
        ArcAttack.transform.GetChild(1).GetChild(1).position = Vector3.Lerp(
            ArcAttack.transform.GetChild(1).GetChild(0).position,
            ArcAttack.transform.GetChild(1).GetChild(2).position, 0.5f);
        //if (diffX > diffY) ArcAttack.transform.GetChild(1).GetChild(1).position = new Vector2(
        //    actionMaker.transform.position.x - target.transform.position.x,
        //    (actionMaker.transform.position.y - target.transform.position.y));
        //else if (diffX <= diffY) ArcAttack.transform.GetChild(1).GetChild(1).position = new Vector2(
        //    (actionMaker.transform.position.x - target.transform.position.x),
        //    actionMaker.transform.position.y - target.transform.position.y);
        ArcAttacks.Add(ArcAttack);
    }

    public void Charge()
    {
        chargeTime -= Time.deltaTime;
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
        actionMakerMove.PermitirMovimento(true);
    }
    public void ResetTargetParams()
    {
        actionMakerMove.Lento(false);
        _GridManager.SearchMode(false);
        if (targets != null)
        {
            if (targets.Any()) _GridManager.ResetArrow(targets);
        }
        actionMakerMove.PermitirMovimento(true);
    }
    public bool Interruption()
    {
        var actionMakerInput = actionMaker.GetComponent<InputSys>();
        if (actionMakerInput != null)
        {
            if (actionMakerInput.CancelPress() ||
                actionMakerInput.DashPress())
            {
                Debug.Log("true");
                return true;
            }
            else
            {
                Debug.Log("false");
                return false;
            }
        }
        else return false;
        
    }
    public void CustarAP()
    {
        if (PACost > 0) actionMaker.AlterarPA(-PACost);
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
        }
        SpecificEffect();
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
        if (ArcAttacks.Any()) foreach (var arc in ArcAttacks) Destroy(arc);
        activated = false;
        _GridManager.ResetParams();
        if (actionMaker.GetComponent<InputSys>() != null) actionMaker.GetComponent<InputSys>().holdingSkill = false;
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
        if (HasAOEEffect) _GridManager.ApplyEffectsOnTiles(AOEArea, EffectType);
    }

    public Sprite GetSkillSprite()
    {
        return Resources.Load<Sprite>("UISprites/" + this.GetType().Name);
    }

    public void CustarMN()
    {
        if (MNCost > 0) actionMaker.AlterarMN(-MNCost);
    }
}