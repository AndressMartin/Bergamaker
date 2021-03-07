using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    int PACost { get; }
    bool isMagic { get; }
    public bool istarget { get; }
    public bool isEnemy { get;}
    int range { get; }
    public float chargeTimeMax { get; }
    public float chargeTime { get; }
    public bool charging { get; }
    public int MNCost { get; }
    public float CD { get; }
    GameObject target { get; }
    Player actionMaker { get; }
    Transform actionChild { get; }
    public int onButton { get; set; }

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
    void GetButtonIndex();
}
