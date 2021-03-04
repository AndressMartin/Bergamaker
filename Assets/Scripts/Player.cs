using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour, IActor
{

    [SerializeField] public int PVMax { get; private set; }
    [SerializeField] public int MNMax { get; private set; }
    [SerializeField] public int PAMax { get; private set; }
    [SerializeField] public int PV { get; private set; }
    [SerializeField] public int MN { get; private set; }
    [SerializeField] public int PA { get; private set; }

    private bool pvRegen, paRegen, mnRegen = false;
    private float pvRegenRate, paRegenRate, mnRegenRate = 1.0f;

    [SerializeField] private int FOR, DEX, INT, MEM, ATE;

    protected float ForAtt, DexAtt, Vel, Slots, SkillPt, CritCh, Pre, EsqCh, Def, Esp, Ten;
    protected float ResAgu, ResTer, ResAr, ResFog, ResLuz, ResSom;

    // Start is called before the first frame update
    void Start()
    {
        SetMaxTestStats();
        PVMax = PVMaxCalc();
        MNMax = MNMaxCalc();
        PAMax = PAMaxCalc();
        SetTestStats();
    }

    // Update is called once per frame
    void Update()
    {
        checkPAMinMax();    
        checkPVMinMax();
        checkMNMinMax();
    }

    //IActor Methods
    public int AlterarPA(int alteracao)
    {
        PA += alteracao;
        return PA;
    }
    public int AlterarPV(int alteracao)
    {
        PV += alteracao;
        return PV;
    }
    public int AlterarMN(int alteracao)
    {
        MN += alteracao;
        return MN;
    }

    //Player Methods
    private int PVMaxCalc()
    {
        return FOR * 10;
    }
    private int MNMaxCalc()
    {
        return INT * 10;
    }
    private int PAMaxCalc()
    {
        return DEX * 10;
    }
    private void SetMaxTestStats()
    {
        FOR = Random.Range(10, 20);
        DEX = Random.Range(10, 20);
        INT = Random.Range(10, 20);
        MEM = Random.Range(10, 20);
        ATE = Random.Range(10, 20);
    }
    private void SetTestStats()
    {
        PV = PVMax - Random.Range(60, PVMax);
        MN = MNMax - Random.Range(60, MNMax);
        PA = PAMax - Random.Range(60, PAMax);
    }

    //Regen
    private void checkPVMinMax()
    {
        if (PV != PVMax && !pvRegen)
        {
            StartCoroutine(PVRegen());
        }
    }
    private IEnumerator PVRegen()
    {
        pvRegen = true;
        while (PV < PVMax)
        {
            PV = AlterarPV(1);
            yield return new WaitForSeconds(1);
        }
        pvRegen = false;
    }

    private void checkPAMinMax()
    {
        if (PA != PAMax && !paRegen)
        {
            StartCoroutine(PARegen());
        }
    }
    private IEnumerator PARegen()
    {
        paRegen = true;
        while (PA < PAMax)
        {
            PA = AlterarPA(10);
            yield return new WaitForSeconds(1);
        }
        paRegen = false;
    }

    private void checkMNMinMax()
    {
        if (MN != MNMax && !mnRegen)
        {
            StartCoroutine(MNRegen());
        }
    }
    private IEnumerator MNRegen()
    {
        mnRegen = true;
        while (MN < MNMax)
        {
            MN = AlterarMN(5);
            yield return new WaitForSeconds(1);
        }
        mnRegen = false;
    }
}
