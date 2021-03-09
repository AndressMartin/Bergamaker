using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetedAction : IAction
{
    GameObject target { get; }
}
