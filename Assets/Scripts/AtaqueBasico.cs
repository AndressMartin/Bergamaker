using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AtaqueBasico : TargetSkillModel
{
    public override int PACost{get { return 10; } protected set { } }
    public override int range { get { return 2; } protected set { } }
    public override int efeito { get { return -10; } protected set { } }
    public override float chargeTimeMax { get { return 1f; } protected set { } }
    public override float CD { get { return .5f; } protected set { } }
    public override bool isInstant { get { return false; } protected set { } }
    public override bool isEnemy { get { return true; } protected set { } }
}
