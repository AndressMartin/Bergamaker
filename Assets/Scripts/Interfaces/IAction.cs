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

    void ChargeIni();
    void ChargeEnd();
    void CustarAP();
    void CustarMN();
    void MakeEffect();

}
