using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainEffectsManagement : MonoBehaviour
{
    Tilemap effectsMap;
    TerrainEffects effectType;
    List<Vector3> area;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void GetEffectParams(List<Vector3> area, TerrainEffects effectType)
    {
        this.effectType = effectType;
        this.area = area;
        FillGrid();
    }

    private void FillGrid()
    {
        
    }
}
