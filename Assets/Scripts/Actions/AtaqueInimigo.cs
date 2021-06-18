using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueInimigo : ActionModel
{
    public override int PACost => 10;
    public override int range => 2;
    public override int efeito => -10;
    public override float chargeTimeMax => 0.5f;
    public override float CD => .5f;
    public override bool isInstant => false;
    public override bool isAuto => true;
    public override int targetsNum => 1;
    public override bool multiTargetsOnly => false;
    public override ActionTargets targetType => ActionTargets.Ally;

    public override void PlayAnimation()
    {
        actionMakerAnimation.ResetarParametrosDasAnimacoes();
        actionMakerAnimation.TrocarAnimacao("Ataque Basico");
    }

}
