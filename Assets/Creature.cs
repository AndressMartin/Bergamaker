using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour, IActor
{
    public int PV { get; private set; } = 100;

    public int MN { get; private set; } 

    public int PA { get; private set; }

    public int PVMax { get; private set; } = 100;

    public int MNMax { get; private set; } = 0;

    public int PAMax { get; private set; } = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int AlterarMN(int alteracao)
    {
        throw new System.NotImplementedException();
    }

    public int AlterarPA(int alteracao)
    {
        throw new System.NotImplementedException();
    }

    public int AlterarPV(int alteracao)
    {
        PV += alteracao;
        return PV;
    }
}
