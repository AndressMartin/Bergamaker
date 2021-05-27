using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class AreaSkillModel : ActionModel
{
    public override void SendTargetRequest()
    {
        _GridManager.StartSearchMode(true, PassRange(), PassActionMaker(), AOE, PassDesiredTargets());

    }
    public override void WaitTarget()
    {
        actionMaker.GetComponent<Movement>().Lento(true);
        if (_GridManager.targetUnits != null && _GridManager.targetUnits.Any())
        {
            _GridManager.SearchMode(false);
            targets = _GridManager.targetUnits.ToList();
            _GridManager.targetUnits.Clear();
            ChargeIni();
            actionMaker.GetComponent<Movement>().Lento(false);
        }
        else if (Interruption())
        {
            // Debug.LogError("An interruption to targeting ocurred");
            ResetTargetParams();
            Fail();
        }
    }
}
