using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionVida : ActionModel
{
    public override int efeito => +10;
    public override float chargeTimeMax => 0f;
    public override bool isAuto => true;
    public override ActionTargets targetType => ActionTargets.Self;
}
