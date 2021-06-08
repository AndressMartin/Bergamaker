using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    int PACost { get; }
    PossibleTargets targetType { get;}
    bool isInstant { get; }
    int efeito { get; }
    float chargeTimeMax { get; }
    float chargeTime { get; }
    bool charging { get; }
    float CD { get; }
    int onButton { get; }
    bool activated { get; }
    EntityModel actionMaker { get; }
    Movement actionMakerMove { get; }
    Transform actionChild { get; }
    Transform skillHolder { get; }
    Sprite sprite { get; }
    void Activate(EntityModel actionCaller);
    void ChargeIni();
    void Charge();
    //IEnumerator ChargeEnd();
    void Fail();
    void CustarAP();
    void MakeEffect();
    void SpecificEffect();
    void End();
    int FindStoredButton();
    Sprite GetSkillSprite();
}
public interface ITarget: IAction
{
    int range { get; }
    Vector3 pointClicked { get; }
    GridManager _GridManager { get; }
    void SendTargetRequest();
    int PassRange();
    List<string> PassDesiredTargets();
    void WaitTarget();
}
public interface IDirect : ITarget
{
    List<GameObject> targets { get; }
    int targetsNum { get; }
    bool multiTargetsOnly { get; }
}
public interface IArea : ITarget
{
    int AOE { get; }
    List<Vector3> AOEArea { get; }
    Shapes shapeType { get; }
}
public interface ISkill : IAction
{

}
public interface IMagic : IAction
{
    int MNCost { get; }
    TerrainEffects EffectType { get; }
    bool HasAOEEffect { get; }
    void CustarMN();
}