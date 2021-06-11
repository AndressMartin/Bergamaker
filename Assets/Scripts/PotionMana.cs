using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionMana : ActionModel
{
    public override int efeito => 0;
    public override float chargeTimeMax => 0f;
    public override bool isAuto => true;
    public override ActionTargets targetType => ActionTargets.Self;
    public override void SpecificEffect()
    {
        foreach (var target in targets) target.GetComponent<EntityModel>().AlterarMN(+30);
    }
}
