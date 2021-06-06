using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionMana : MonoBehaviour, IAction
{
    public Targeter _targeter => throw new System.NotImplementedException();

    public int PACost { get; private set; } = 0;

    public virtual PossibleTargets targetType { get; protected set; }

    public bool isInstant { get; private set; } = true;

    public bool isArea => throw new System.NotImplementedException();

    public int AOE => throw new System.NotImplementedException();

    public int range => throw new System.NotImplementedException();

    public int efeito { get; private set; } = 10;

    public float chargeTimeMax { get; private set; } = 1;

    public float chargeTime { get; private set; }

    public bool charging { get; private set; }

    public float CD => throw new System.NotImplementedException();

    public Player actionMaker { get; private set; }

    public InputSys actionMakerInput { get; private set; }

    public Transform actionChild { get; private set; }

    public GameObject mouseAuraHolder { get; private set; }

    public int onButton => throw new System.NotImplementedException();
    public Button button;

    public Transform skillHolder { get; private set; }

    public bool activated { get; private set; } = false;

    public GameObject target { get; private set; }

    public Sprite sprite => throw new System.NotImplementedException();

    void Update()
    {
        if (activated)
        {
            if (charging)
                Charge();
        }

    }

    void Start()
    {
        mouseAuraHolder = GameObject.FindGameObjectWithTag("MouseHolder");
        skillHolder = gameObject.transform.parent;
        target = FindObjectOfType<Player>().gameObject;
        //actionMakerInput.GetSkillButton(onButton);
        actionMaker = FindObjectOfType<Player>();
        actionMakerInput = actionMaker.GetComponent<InputSys>();
        actionChild = actionMaker.transform.GetChild(0);
    }

    public void Activated()
    {
        activated = true;
        actionMakerInput.holdingSkill = true;
        if (isInstant)
            ChargeIni();
    }

    public void SendTargetRequest()
    {
        throw new System.NotImplementedException();
    }
    public int PassRange()
    {
        throw new System.NotImplementedException();
    }
    public Transform PassActionMaker()
    {
        return actionMaker.transform;
    }
    public void ActivateRange()
    {
        throw new System.NotImplementedException();
    }
    public void WaitTarget()
    {
        throw new System.NotImplementedException();
    }
    public void TestDistance()
    {
        throw new System.NotImplementedException();
    }
    public void DeactivateRange()
    {
        throw new System.NotImplementedException();
    }
    public void ChargeIni()
    {

        actionMaker.GetComponent<ColorSys>().ChargingColor();
        charging = true;
        chargeTime = chargeTimeMax;
    }
    public void Charge()
    {
        chargeTime -= Time.deltaTime;
        var actionMakerMove = actionMaker.GetComponent<Movement>();
        actionMakerMove.PermitirMovimento(false);

        if (chargeTime <= 0)
        {

            ResetChargeParams();
            CustarAP();
            MakeEffect();
        }
    }
    public void ResetChargeParams()
    {
        charging = false;
        chargeTime = 0;
        if (actionMaker != null) actionMaker.GetComponent<ColorSys>().DefaultColor();
        actionMaker.GetComponent<Movement>().PermitirMovimento(true);
    }
    public void ResetTargetParams()
    {
        actionMaker.GetComponent<Movement>().Lento(false);
        actionMaker.GetComponent<Movement>().PermitirMovimento(true);
    }
    public bool Interruption()
    {
        var actionMakerInput = actionMaker.GetComponent<InputSys>();
        if (actionMakerInput.CancelPress() ||
            actionMakerInput.DashPress())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void CustarAP()
    {
        actionMaker.AlterarPA(-PACost);
    }
    public void MakeEffect()
    {
        Debug.Log($"Causou {efeito} de dano em {target}");
        target.GetComponent<Player>().AlterarMN(efeito);
        End();
    }
    public void Fail()
    {
        ResetChargeParams();
        ResetTargetParams();
        End();
    }
    public void End()
    {
        activated = false;
        actionMakerInput.holdingSkill = false;
    }
    public int FindStoredButton()
    {
        throw new System.NotImplementedException();
    }

    public void SpecificEffect()
    {
        throw new System.NotImplementedException();
    }

    public Sprite GetSkillSprite()
    {
        throw new System.NotImplementedException();
    }
}
