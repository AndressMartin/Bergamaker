using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetaDeGelo : ActionModel
{
    public override int PACost => 20;
    public override int range => 6;
    public override int efeito => -10;
    public override float chargeTimeMax => 2f;
    public override float CD => .5f;
    public override bool isInstant => false;
    public override int AOE => 1;
    public override PossibleTargets targetType => PossibleTargets.Enemy;
    public override Shapes shapeType => Shapes.Line;
}
