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

    public bool invinciFrames;
    public bool pvRegen, paRegen, mnRegen;
    public float pvRegenRate = 1.0f;
    public float paRegenRate = 1.0f;
    public float mnRegenRate = 1.0f;
    public int pvRegenAmout = 1;
    public int paRegenAmout = 10;
    public int mnRegenAmout = 5;
    public float invinciTimer = 1.0f;

    public float effectMaxTime = 5f;
    public float effectTime = 0;

    public TerrainEffects status { get; protected set; }
    public UnityEngine.Object statusObject { get; protected set; }
    void Update()
    {
        checkPAMinMax();
        checkPVMinMax();
        checkMNMinMax();

        if (PV <= 0)
            Die();
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

    public virtual int AlterarMN(int alteracao)
    {
        MN += alteracao;
        return MN;
    }

    public virtual int AlterarPA(int alteracao)
    {
        PA += alteracao;
        return PA;
    }

    public virtual int AlterarPV(int alteracao)
    {
        if (!invinciFrames)
        {
            PV += alteracao;
            if (PV + alteracao < PV)
                StartCoroutine(Invincibility());
            return PV;
        }
        return PV;
    }
    public IEnumerator Invincibility()
    {
        invinciFrames = true;
        yield return new WaitForSeconds(invinciTimer);
        invinciFrames = false;
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
    private void checkPVMinMax()
    {
        if (PV != PVMax && !pvRegen)
        {
            StartCoroutine(PVRegen());
        }
        if (PV > PVMax)
        {
            PV = PVMax;
        }
    }
    private IEnumerator PVRegen()
    {
        pvRegen = true;
        while (PV < PVMax)
        {
            yield return new WaitForSeconds(pvRegenRate);
            PV = AlterarPV(pvRegenAmout);
        }
        pvRegen = false;
    }

    private void checkPAMinMax()
    {
        if (PA != PAMax && !paRegen)
        {
            StartCoroutine(PARegen());
        }
        if (PA > PAMax)
        {
            PA = PAMax;
        }
    }
    private IEnumerator PARegen()
    {
        paRegen = true;
        while (PA < PAMax)
        {
            yield return new WaitForSeconds(paRegenRate);
            PA = AlterarPA(paRegenAmout);
        }
        paRegen = false;
    }

    private void checkMNMinMax()
    {
        if (MN != MNMax && !mnRegen)
        {
            StartCoroutine(MNRegen());
        }
        if (MN > MNMax)
        {
            MN = MNMax;
        }
    }
    private IEnumerator MNRegen()
    {
        mnRegen = true;
        while (MN < MNMax)
        {
            yield return new WaitForSeconds(mnRegenRate);
            MN = AlterarMN(mnRegenAmout);
        }
        mnRegen = false;
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}