using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    Targeter _targeter { get; }
    int PACost { get; }
    bool isEnemy { get;}
    bool isInstant { get; }
    bool isArea { get; }
    int AOE { get; }
    int range { get; }
    int efeito { get; }
    float chargeTimeMax { get; }
    float chargeTime { get; }
    bool charging { get; }
    float CD { get; }
    Player actionMaker { get; }
    InputSys actionMakerInput { get; }
    Transform actionChild { get; }
    GameObject mouseAuraHolder { get; }
    int onButton { get; }
    Transform SkillHolder { get; }
    bool activated { get; }
    void Activated();
    void SendTargetRequest();
    int PassRange();
    void ActivateRange();
    void TestDistance();
    void DeactivateRange();
    void WaitTarget();
    void ChargeIni();
    void Charge();
    //IEnumerator ChargeEnd();
    void Fail();
    void CustarAP();
    void MakeEffect();
    void End();
    int FindStoredButton();
}
