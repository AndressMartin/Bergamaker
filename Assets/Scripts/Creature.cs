using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : EntityModel
{
    public override int PV { get; protected set; } = 32;

    public override int MN { get; protected set; } 

    public override int PA { get; protected set; }

    public override int PVMax { get; protected set; } = 40;

    public override int MNMax { get; protected set; } = 0;

    public override int PAMax { get; protected set; } = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PV <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override int AlterarMN(int alteracao)
    {
        throw new System.NotImplementedException();
    }

    public override int AlterarPA(int alteracao)
    {
        throw new System.NotImplementedException();
    }

    public override int AlterarPV(int alteracao)
    {
        PV += alteracao;
        return PV;
    }
}
