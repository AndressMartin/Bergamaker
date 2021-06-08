using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : EntityModel
{
    public override int PV { get; protected set; } = 32;

    public override int MN { get; protected set; } = 0;

    public override int PA { get; protected set; } = 30;

    public override int PVMax { get; protected set; } = 40;

    public override int MNMax { get; protected set; } = 0;

    public override int PAMax { get; protected set; } = 0;
    
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
