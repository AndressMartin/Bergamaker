using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BolaDeFogo : ActionModel
{
    public override int PACost => 60;
    public override int range => 4;
    public override int efeito => -30;
    public override float chargeTimeMax => 2f;
    public override float CD => .5f;
    public override bool isInstant => false;
    public override int AOE => 2;
    public override PossibleTargets targetType => PossibleTargets.Any;
}