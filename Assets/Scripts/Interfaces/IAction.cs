using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    int PACost { get; }
    bool isMagic { get; }
    bool istarget { get; }
    bool isEnemy { get;}
    int range { get; }
    float chargeTimeMax { get; }
    float chargeTime { get; }
    bool charging { get; }
    int MNCost { get; }
    float CD { get; }
    GameObject target { get; }
    Player actionMaker { get; }
    InputSys actionMakerInput { get; }
    Transform actionChild { get; }
    int onButton { get; }
    Transform SkillHolder { get; }
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
    void CustarMN();
    void MakeEffect();
    int FindStoredButton();
    int SendButtonToInput();
}
