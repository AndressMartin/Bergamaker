using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    int PACost { get; }
    bool isEnemy { get;}
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
    Targeter _targeter { get; }
    GameObject target { get; }
    AuraDrawer auraDrawer { get; }
    void SendTargetRequest();
    int PassRange();
    string PassDesiredTarget();
    void ActivateRange();
    void TestDistance();
    void DeactivateRange();
    void WaitTarget();
}
public interface IArea : IAction
{
    int AOE { get; }
    int range { get; }
    Targeter _targeter { get; }
    GameObject mouseAuraHolder { get; }
    AuraDrawer auraDrawer { get; }
    void SendTargetRequest();
    int PassRange();
    string PassDesiredTarget();
    void ActivateRange();
    void TestDistance();
    void DeactivateRange();
    void WaitTarget();
}
public interface ISkill : IAction
{

}
public interface IMagic : IAction
{
    int MNCost { get; }
    void CustarMN();
}