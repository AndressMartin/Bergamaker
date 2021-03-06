using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThing
{

}


public interface IActor: IThing
{
    int PV { get; }
    int MN { get; }
    int PA { get; }
    int PVMax { get; }
    int MNMax { get; }
    int PAMax { get; }
    TerrainEffects status { get; }
    Object statusObject { get; }
    int AlterarPV(int alteracao);
    int AlterarMN(int alteracao);
    int AlterarPA(int alteracao);
    void AlterarStatus(TerrainEffects status);
}
