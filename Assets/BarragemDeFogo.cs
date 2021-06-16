using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarragemDeFogo : ActionModel
{
    public override int PACost => 50;
    public override int range => 6;
    public override int efeito => -10;
    public override float chargeTimeMax => 2f;
    public override float CD => .5f;
    public override bool isInstant => false;
    public override bool multiTargetsOnly => false;
    public override int targetsNum => 5;
    public override ActionTargets targetType => ActionTargets.Enemy;

    public override void PlayChargeAnimation()
    {
        actionMakerMoveAnimation.TrocarAnimacao("Castando Magia");
    }
}
