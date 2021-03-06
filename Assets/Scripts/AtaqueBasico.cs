using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueBasico : MonoBehaviour, IAction
{
    private Targeter _targeter;
    //IAction
    public int PACost { get; private set; } = 10;
    public bool isMagic { get; private set; } = false;
    public float range { get; private set; } = 2;
    public float chargeTimeMax { get; private set; } = 1f;
    public float chargeTime { get; private set; }
    public bool charging{ get; private set; }
    public int MNCost { get; private set; } = 0;
    public float CD { get; private set; } = .5f;
    //IClickable
    public bool IsInstant { get; private set; } = false;
    public GameObject target { get; private set; } 
    public Player actionMaker { get; private set; }
    public Transform actionChild { get; private set; }
    public int onButton { get => throw new NotImplementedException(); set => throw new NotImplementedException(); } //TODO: Implementar referencia ao indice do botao

    public bool istarget => throw new NotImplementedException(); //TODO: Implementar target ou �rea

    public bool isEnemy { get; } = true; //TODO: Implementar heal ou damage

    // Start is called before the first frame update
    void Start()
    {
        _targeter = FindObjectOfType<Targeter>();
        actionMaker = FindObjectOfType<Player>();
        actionChild = actionMaker.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        //---FOR TESTING RADIUS
        //gizmosRadius = Vector2.Distance(actionChild.position, FindObjectOfType<Creature>().transform.position);

        if (_targeter.onSearchMode == true)
            WaitTarget();
        if (charging)
            Charge();
    }
    public void Activated()
    {
        if (IsInstant)
            ChargeIni();
        else
        {
            SendTargetRequest();
            ActivateRange();
        }
        Debug.LogAssertion("Activated");
        FindObjectOfType<Movement>().AttackColor();
    }

    public void SendTargetRequest()
    {
        _targeter.onSearchMode = true;
        if (isEnemy)
            _targeter.desiredTarget = "Enemy";
        else
            _targeter.desiredTarget = "Player";
        Debug.LogAssertion("Sent Target Request");
    }

    public void ActivateRange()
    {
        Debug.Log("Range, apare�a!");
        //actionChild.lossyScale.Set(range, range, 1);
        //actionChild.localScale = new Vector3(range, range, 1);
        //actionChild.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        //actionChild.gameObject.GetComponent<CircleCollider2D>().radius = range / 3;

    }

    //------NEEDS TO GO TO A SEPARATE CLASS THAT RECEIVES RANGE FROM ANY SKILL OR SPELL. The Sending method will be ActivateRange()

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(actionChild.position, /*gizmosRadius*/range);
    //}

    public void WaitTarget()
    {
        actionMaker.GetComponent<Movement>().Lento(true);
        //Debug.Log("Waiting For Target");
        if (_targeter.targetUnit != null)
        {
            _targeter.onSearchMode = false;
            target = _targeter.targetUnit;
            Debug.LogError($"This is the target: {target}");
            _targeter.targetUnit = null;
            TestDistance();
            actionMaker.GetComponent<Movement>().Lento(false);
        }
    }
    public void TestDistance()
    {
        if (Vector2.Distance(actionMaker.transform.position, target.transform.position)<= 2)
        {
            Debug.Log($"{target} is at a distance of {Vector2.Distance(actionMaker.transform.position, target.transform.position)}");
            ChargeIni();
        }
        else
        {
            Debug.LogError($"{target} was super far! Distance: {Vector2.Distance(actionMaker.transform.position, target.transform.position)}");
            Fail();
        }
    }

    public void DeactivateRange()
    {
        Debug.Log("Range, desapare�a!");
        //actionChild.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
    public void ChargeIni()
    {
        _targeter.TargetedOutline(target);
        FindObjectOfType<Movement>().ChargingColor();
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
            charging = false;
            chargeTime = 0;

            FindObjectOfType<Movement>().DefaultColor();
            CustarAP();
            MakeEffect();
            _targeter.ResetMat(target);
            actionMakerMove.PermitirMovimento(true);
        }
    }

    public void CustarAP()
    {
        actionMaker.AlterarPA(-PACost);
    }

    public void CustarMN()
    {
        actionMaker.AlterarMN(-MNCost);
    }

    public void MakeEffect()
    {
        Debug.Log($"Causou {-20} de dano em {target}");
        target.GetComponent<Creature>().AlterarPV(-20);
    }

    public void Fail()
    {
        FindObjectOfType<Movement>().DefaultColor();
        _targeter.ResetMat(target);
    }

    public void GetButtonIndex()
    {
        throw new NotImplementedException();
    }
}
