using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : EntityModel
{
    public override int PV { get; protected set; } = 32;

    public override int MN { get; protected set; } = 0;

    public override int PA { get; protected set; } = 1000;

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
}
