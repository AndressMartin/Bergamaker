using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    int PACost { get; }
    bool isMagic { get; }
    float range { get; }
    public float chargeTime { get; }
    public int MNCost { get; }
    public float CD { get; }
    GameObject target { get; }
    Player actionMaker { get; }
    Transform actionChild { get; }
    void Activated();
    void SendTargetRequest();
    void ActivateRange();
    void TestDistance();
    void DeactivateRange();
    void WaitTarget();
    void ChargeIni();
    IEnumerator ChargeEnd();
    void Fail();
    void CustarAP();
    void CustarMN();
    void MakeEffect();
}
