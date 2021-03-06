using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
    public bool IsInstant { get; }
    void OnClick();
}
