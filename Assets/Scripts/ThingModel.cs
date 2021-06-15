using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingModel : MonoBehaviour, IThing
{

    public float effectMaxTime = 5f;
    public float effectTime = 0;

    public TerrainEffects status { get; protected set; }
    public UnityEngine.Object statusObject { get; protected set; }

    private void Update()
    {
        if (statusObject != null)
        {
            if (effectTime == 0) effectTime = effectMaxTime;
            else
            {
                StatusEffectTimer();
            }
        }
    }
    private void StatusEffectTimer()
    {
        effectTime -= Time.deltaTime;
        if (effectTime <= 0)
        {
            Destroy(statusObject);
            statusObject = null;
            effectTime = 0;
        }
    }

    public virtual void AlterarStatus(TerrainEffects status)
    {
        Debug.Log("StatusObject = " + statusObject);
        if (statusObject == null)
        {
            if (status == TerrainEffects.OnFire)
            {
                var effect = Resources.Load("Prefabs/Effects/OnFireEntityEffect");
                statusObject = Instantiate(effect, transform.GetComponent<SpriteRenderer>().bounds.center, transform.rotation, transform);
            }
        }
    }
}
