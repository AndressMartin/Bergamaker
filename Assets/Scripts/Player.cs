using System.Collections;
using UnityEngine;

public class Player: MonoBehaviour, IActor
{

    [SerializeField] public int PVMax { get; private set; }
    [SerializeField] public int MNMax { get; private set; }
    [SerializeField] public int PAMax { get; private set; }
    [SerializeField] public int PV { get; private set; }
    [SerializeField] public int MN { get; private set; }
    [SerializeField] public int PA { get; private set; }

    private bool pvRegen, paRegen, mnRegen;
    private float pvRegenRate = 1.0f;
    private float paRegenRate = 1.0f;
    private float mnRegenRate = 1.0f;
    private int pvRegenAmout = 1;
    private int paRegenAmout = 10;
    private int mnRegenAmout = 5;
    private float invinciTimer = 1.0f;

    [SerializeField] private int FOR, DEX, INT, MEM, ATE;

    protected float ForAtt, DexAtt, Vel, Slots, SkillPt, CritCh, Pre, EsqCh, Def, Esp, Ten;
    protected float ResAgu, ResTer, ResAr, ResFog, ResLuz, ResSom;
    private bool invinciFrames;

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

        if (PV <= 0)
            Die();
    }

    //IActor Methods
    public int AlterarPA(int alteracao)
    {
        PA += alteracao;
        return PA;
    }
    public int AlterarPV(int alteracao)
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
    public int AlterarMN(int alteracao)
    {
        MN += alteracao;
        return MN;
    }
    public IEnumerator Invincibility()
    {
        invinciFrames = true;
        yield return new WaitForSeconds(invinciTimer);
        invinciFrames = false;
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
