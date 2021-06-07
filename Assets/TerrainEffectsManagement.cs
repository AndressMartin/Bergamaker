using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEffectsManagement : MonoBehaviour
{
    TerrainEffects effect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void GetEffectParams(TerrainEffects effectType)
    {
        effect = effectType;
    }
}
