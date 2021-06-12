using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainEffectsManagement : MonoBehaviour
{
    Tilemap effectsMap;
    TilemapCollider2D tilemapCollider;
    TerrainEffects effectType;
    List<Vector3> area;
    // Start is called before the first frame update
    void Start()
    {
        effectsMap = GetComponent<Tilemap>();
        tilemapCollider = effectsMap.GetComponent<TilemapCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void GetEffectParams(List<Vector3> area, TerrainEffects effectType)
    {
        this.effectType = effectType;
        this.area = area;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger != true)
        {
            try
            {
                EntityModel colliderType = collision.transform.parent.GetComponent<EntityModel>();
                if (colliderType != null)
                {
                    colliderType.AlterarStatus(effectType);
                }
            }
            catch (Exception)
            {
                Debug.LogError("Tried to get a nill collider parent: " + collision.gameObject);
            }
        }
        
    }
}
