using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PossibleTargets
{
    Enemy,
    Ally,
    Self,
    SelfAndAlly,
    Any
}

public enum Shapes
{
    Area,
    Cone,
    Line
}

public enum TerrainEffects
{
    OnFire,
    Wet,
    Oil,
    Frozen
}
