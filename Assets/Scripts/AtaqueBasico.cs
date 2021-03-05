using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueBasico : MonoBehaviour, IAction
{
    public int PACost { get; private set; }
    public bool isMagic { get; private set; }
    public float range { get; private set; }
    public float chargeTime { get; private set; }
    public int MNCost { get; private set; }
    public float CD { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChargeIni()
    {
        throw new System.NotImplementedException();
    }
    public void ChargeEnd()
    {
        throw new System.NotImplementedException();
    }

    public void CustarAP()
    {
        throw new System.NotImplementedException();
    }

    public void CustarMN()
    {
        throw new System.NotImplementedException();
    }

    public void MakeEffect()
    {
        throw new System.NotImplementedException();
    }
}
