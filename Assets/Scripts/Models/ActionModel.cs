using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionModel: MonoBehaviour, IDirect, IArea, IMagic, ISkill
{
    public virtual int range { get; protected set; }
    public virtual int PACost { get; protected set; }
    public virtual int MNCost { get; protected set; }
    public virtual ActionTargets targetType { get; protected set; }
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
    public virtual bool isAuto { get; private set; }
    public List<GameObject> targets { get; protected set; }
    public float chargeTime { get; private set; }
    public bool charging { get; private set; }
    public bool activated { get; private set; }
    public int onButton { get; private set; }
    public GridEntity _MyGrid { get; private set; }
    public EntityModel actionMaker { get; private set; }
    public MovementModel actionMakerMove { get; private set; }
    public Animacao actionMakerMoveAnimation { get; private set; }
    public Transform actionChild { get; private set; }
    public Transform skillHolder { get; private set; }
    public Sprite sprite { get; private set; }
    public List<Vector3> AOEArea { get; private set; }
    public Vector3 pointClicked { get; private set; }
    public Vector3 centerOfAOE { get; private set; }
    public List<GameObject> ArcAttacks { get; private set; }
    private bool playingAnimation;

    private void StartingParameters()
    {
        if (AOE > 0) AOEArea = new List<Vector3>();
        targets = new List<GameObject>();
        ArcAttacks = new List<GameObject>();
        actionMakerMove = actionMaker.GetComponent<MovementModel>();
        actionMakerMoveAnimation = actionMakerMove.GetComponent<Animacao>();
        skillHolder = gameObject.transform.parent;
        if (actionMaker.GetComponent<Player>() != null) onButton = FindStoredButton();
        sprite = GetSkillSprite();
        //actionMakerInput.GetSkillButton(onButton);
         _MyGrid = actionMaker.gameObject.GetComponent<GridEntity>();
        playingAnimation = false;
    }

    void Update()
    {
        if (activated)
        {
            if (_MyGrid.onSearchMode == true)
                WaitTarget();

            if (charging)
                Charge();

            if (playingAnimation)
            {
                Animating();
            }
        }
    }
    public void Activate(EntityModel actionCaller)
    {
        actionMaker = actionCaller;
        StartingParameters();
        if (actionMaker.PA <= PACost)
        {
            Debug.Log("No AP for execution");
            End();
            return;
        }
        activated = true;
        if (targetType == ActionTargets.Self) 
        {
            Debug.Log(actionMaker.gameObject);
            Debug.Log(targets);
            targets.Add(actionMaker.gameObject);
            ChargeIni(); 
        }
        else
        {
            SendTargetRequest();
        }
    }
    public virtual void SendTargetRequest()
    {
        if (isAuto)
            _MyGrid.StartSearchMode(true, PassRange(), PassActionMaker(), PassDesiredTargets(), isAuto);
        else if (AOE <= 0)
            _MyGrid.StartSearchMode(true, PassRange(), PassActionMaker(), multiTargetsOnly, PassDesiredTargets());
        else if (AOE > 0)
            _MyGrid.StartSearchMode(true, PassRange(), PassActionMaker(), AOE, HasAOEEffect, shapeType, PassDesiredTargets());
    }
    public List<string> PassDesiredTargets()
    {
        List<string> desiredTargets = new List<string>();
        if (targetType == ActionTargets.Enemy)
            desiredTargets.Add("Enemy");
        else if (targetType == ActionTargets.Ally)
            desiredTargets.Add("Player");
        else if (targetType == ActionTargets.SelfAndAllies)
        {
            desiredTargets.Add("Ally");
            desiredTargets.Add("Player");
        }
        else if (targetType == ActionTargets.Any)
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
        if (actionMakerMove != null) actionMakerMove.Lento(true);
        if (_MyGrid.gridIni == false)
        {
            _MyGrid.TargetingLoop();
            _MyGrid.gridIni = true;
        }
        if ((_MyGrid.timesTargetWasSent >= targetsNum && _MyGrid.targetUnits.Any()) || _MyGrid.canAttack)
        {
            AOEArea = _MyGrid.PassAOEArea().ToList();
            pointClicked = _MyGrid.PassPointClicked();
            centerOfAOE = _MyGrid.PassCenterOfAOE();
            _MyGrid.SearchMode(false);
            targets = _MyGrid.targetUnits.ToList();
            _MyGrid.TargetArrow(_MyGrid.targetUnits);
            _MyGrid.targetUnits.Clear();
            ChargeIni();
            if (actionMakerMove != null) actionMakerMove.Lento(false);
        }
        else if (isAuto)
        {
            Debug.Log("Didn't find target");
            Fail();
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
        Debug.Log("Bla");
        if (AOE <= 1) 
        {
            foreach (var target in targets)
            {
                if (targetType != ActionTargets.Self) CreateArc(target);
            }
        }
        else
        {
            CreateArc(centerOfAOE);
        }
        charging = true;
        chargeTime = chargeTimeMax;

        actionMakerMove.PermitirMovimento(false);
        actionMakerMoveAnimation.AnimacaoIniciarCasting();
        actionMakerMoveAnimation.DefinirDirecaoAtaque(AOE, pointClicked, targets);
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
        Gradient gradient = new Gradient();
        Color myColor = new Color();
        if (actionMaker is Player) myColor = Color.green;
        else if (actionMaker is Creature) myColor = Color.red;
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(myColor, 0.0f), new GradientColorKey(myColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(0.0f, 1.0f) }
        );
        ArcAttack.transform.GetChild(1).GetComponent<LineRenderer>().colorGradient = gradient;
        ArcAttack.transform.GetChild(1).GetChild(2).GetComponent<ParabolaEndTargetFollow>().targetPos = target.transform.position;
    }
    private void CreateArc(Vector3 target)
    {
        GameObject ArcAttack = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/ArcAttack"), actionMaker.transform);
        //Set begin, middle and endPoints
        ArcAttack.transform.GetChild(1).GetChild(0).position = actionMaker.transform.position;
        var localPosB = actionMaker.transform.InverseTransformPoint(target);
        var diffX = localPosB.x;
        var diffY = localPosB.y;
        ArcAttack.transform.GetChild(1).GetChild(2).position = target;
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
        Gradient gradient = new Gradient();
        Color myColor = new Color();
        if (actionMaker is Player) myColor = Color.green;
        else if (actionMaker is Creature) myColor = Color.red;
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(myColor, 0.0f), new GradientColorKey(myColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(0.0f, 1.0f) }
        );
        ArcAttack.transform.GetChild(1).GetComponent<LineRenderer>().colorGradient = gradient;
        ArcAttack.transform.GetChild(1).GetChild(2).GetComponent<ParabolaEndTargetFollow>().targetPos = target;
    }

    public void Charge()
    {
        Debug.Log("Bla");
        chargeTime -= Time.deltaTime;
        actionMakerMove.PermitirMovimento(false);
        if (chargeTime <= 0)
        {
            ChargeEnd();
            PlayAnimation();
            playingAnimation = true;
        }
    }

    public virtual void PlayAnimation()
    {
        if (AOE > 0) CheckTargetsRemainingInArea();
        MakeEffect();
        End();
        actionMakerMove.Lento(false);
        actionMakerMove.PermitirMovimento(true);
        playingAnimation = false;
    }

    public void Animating()
    {
        if (actionMakerMoveAnimation.acertandoAtaque)
        {
            //Debug.Log("Acertou");
            actionMakerMoveAnimation.acertandoAtaque = false;
            if (AOE > 0) CheckTargetsRemainingInArea();
            MakeEffect();
        }

        if (actionMakerMoveAnimation.terminandoAtaque)
        {
            //Debug.Log("Finalizou");
            actionMakerMoveAnimation.terminandoAtaque = false;
            End();
            actionMakerMove.Lento(false);
            actionMakerMove.PermitirMovimento(true);
            playingAnimation = false;
        }
    }

    private void CheckTargetsRemainingInArea()
    {
        var localtargets = targets.ToList();
        foreach(var target in localtargets)
        {
            if (AOEArea.Contains(_MyGrid._gridGlobal.grid.LocalToCell(target.transform.position)) == false)
            {
                targets.Remove(target);
            }
        }
        var newTargets = _MyGrid.FindEntities(PassDesiredTargets(), AOEArea);
        targets.AddRange(newTargets);
    }
    private void ChargeEnd()
    {
        ResetChargeParams();
        ResetTargetParams();
        CustarAP();
    }

    public void ResetChargeParams()
    {
        charging = false;
        chargeTime = 0;
    }
    public void ResetTargetParams()
    {
        _MyGrid.SearchMode(false);
        if (targets != null)
        {
            if (targets.Any()) _MyGrid.ResetArrow(targets);
        }
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
                Debug.Log(efeito + " de dano em " + target);
                target.GetComponent<EntityModel>().AlterarPV(efeito);
            }
            catch (Exception)
            {
                Debug.LogError("EntityModel was null while applying effect");
            }
        }
        SpecificEffect();
    }
    public void Fail()
    {
        ResetChargeParams();
        ResetTargetParams();
        End();
    }
    public void End()
    {
        Debug.Log("Ending");
        if (ArcAttacks.Any()) foreach (var arc in ArcAttacks) Destroy(arc);
        activated = false;
        _MyGrid.gridIni = false;
        if (_MyGrid != null) _MyGrid.ResetParams();
        if (actionMaker.GetComponent<InputSys>() != null)   actionMaker.GetComponent<InputSys>().holdingSkill = false;
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
        if (HasAOEEffect) _MyGrid._gridGlobal.ApplyEffectsOnTiles(AOEArea, EffectType);
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