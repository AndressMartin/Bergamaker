using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainEffectsManagement : MonoBehaviour
{
    Tilemap effectsMap;
    TilemapCollider2D tilemapCollider;
    TerrainEffects effectType;
    List<Vector3> area;
    List<List<Vector3>> areas;
    List<float> timers;
    float maxTimer = 10f;
    // Start is called before the first frame update
    void Start()
    {
        areas = new List<List<Vector3>>();
        timers = new List<float>();
        effectsMap = GetComponent<Tilemap>();
        tilemapCollider = effectsMap.GetComponent<TilemapCollider2D>();
    }

    private void Update()
    {
        foreach (var tile in area)
        {
            Debug.Log(tile);
        }
        if (areas.Any())    AreaTimer();
    }

    internal void GetEffectParams(List<Vector3> area, TerrainEffects effectType)
    {
        foreach (var tile in area)
        {
            Debug.Log(tile);
        }
        this.effectType = effectType;
        this.area = area;
        areas.Add(area);
        timers.Add(maxTimer);
    }

    void AreaTimer()
    {
        for (int i = 0; i < areas.Count; i++)
        {
            timers[i] -= Time.deltaTime;
            if (timers[i] <= 0)
            {
                foreach (var tile in areas[i])
                {
                    effectsMap.SetTile(Vector3Int.FloorToInt(tile), null);
                }
                areas[i].Clear();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger != true)
        {
            try
            {
                ThingModel colliderType = collision.transform.parent.GetComponent<ThingModel>();
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
