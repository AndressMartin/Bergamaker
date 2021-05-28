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
    Player actionMaker { get; }
    InputSys actionMakerInput { get; }
    Transform actionChild { get; }
    Transform SkillHolder { get; }
    void Activated();
    void ChargeIni();
    void Charge();
    //IEnumerator ChargeEnd();
    void Fail();
    void CustarAP();
    void MakeEffect();
    void End();
    int FindStoredButton();
}
public interface ITarget: IAction
{
    int range { get; }
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
}
public interface ISkill : IAction
{

}
public interface IMagic : IAction
{
    int MNCost { get; }
    void CustarMN();
}