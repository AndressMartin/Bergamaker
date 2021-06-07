using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BolaDeFogo : ActionModel
{
    public override int PACost => 60;
    public override int range => 4;
    public override int efeito => -30;
    public override float chargeTimeMax => 2f;
    public override float CD => .5f;
    public override bool isInstant => false;
    public override int AOE => 2;
    public override bool HasAOEEffect => true;
    public override PossibleTargets targetType => PossibleTargets.Any;
    public override Shapes shapeType => Shapes.Area;
    public GameObject fireExplosion;
    public GameObject lightArea;
    public override void SpecificEffect()
    {
        Debug.Log("Come here");
        fireExplosion = Instantiate(Resources.Load<GameObject>("Prefabs/Effects/Fire_Explosion"), pointClicked, new Quaternion(0, 0, 0, 0));
        StartCoroutine(ExplosionCoroutine());
        base.SpecificEffect();
        lightArea = Instantiate(Resources.Load<GameObject>("Prefabs/Effects/BolaDeFogoLightArea"), pointClicked, new Quaternion(0, 0, 0, 0));
    }
    IEnumerator ExplosionCoroutine()
    {
        yield return new WaitForSeconds(2);
        Destroy(fireExplosion);
    }
}