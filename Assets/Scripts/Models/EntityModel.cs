using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EntityModel : MonoBehaviour, IActor
{
    public virtual int PV { get; protected set; }

    public virtual int MN { get; protected set; }

    public virtual int PA { get; protected set; }

    public virtual int PVMax { get; protected set; }

    public virtual int MNMax { get; protected set; }

    public virtual int PAMax { get; protected set; }

    public TerrainEffects status { get; protected set; }
    public UnityEngine.Object statusObject { get; protected set; }

    public virtual int AlterarMN(int alteracao)
    {
        throw new NotImplementedException();
    }

    public virtual int AlterarPA(int alteracao)
    {
        throw new NotImplementedException();
    }

    public virtual int AlterarPV(int alteracao)
    {
        throw new NotImplementedException();
    }

    public virtual void AlterarStatus(TerrainEffects status)
    {
        if (statusObject == null)
        {
            if (status == TerrainEffects.OnFire)
            {
                var effect = Resources.Load("Prefabs/Effects/OnFireEntityEffect");
                statusObject = Instantiate(effect, transform.position, transform.rotation, transform);
            }
        }
    }
}