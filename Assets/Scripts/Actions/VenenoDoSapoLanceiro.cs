using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class VenenoDoSapoLanceiro : ActionModel
{
    public override int PACost => 40;
    public override int range => 5;
    public override int efeito => -20;
    public override float chargeTimeMax => 1f;
    public override float CD => .5f;
    public override bool isInstant => false;
    public override bool isAuto => true;
    public override int targetsNum => 1;
    public override bool multiTargetsOnly => false;
    public override ActionTargets targetType => ActionTargets.Ally;

    public GameObject venenoDoSapoLanceiroAnimacao;
    private VenenoDoSapoLanceiro_Script venenoDoSapoLanceiroAnimacaoScript;

    public override void PlayChargeAnimation()
    {
        actionMakerAnimation.TrocarAnimacao("Castando Magia");
    }

    public override void PlayAnimation()
    {
        venenoDoSapoLanceiroAnimacao = Instantiate(Resources.Load<GameObject>("Prefabs/Magias/VenenoDoSapoLanceiro_Animacao"), new Vector3 (0, 0, 0), new Quaternion(0, 0, 0, 0));
        venenoDoSapoLanceiroAnimacaoScript = venenoDoSapoLanceiroAnimacao.GetComponent<VenenoDoSapoLanceiro_Script>();
        venenoDoSapoLanceiroAnimacaoScript.Iniciar(actionMaker, targets);
    }
}