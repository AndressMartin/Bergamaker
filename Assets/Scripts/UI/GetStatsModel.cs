using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetStatsModel : MonoBehaviour
{
    public Creature creature;
    public Slider sliderPV;

    void Update()
    {
        SetSlider();
    }

    private void SetSlider()
    {
        sliderPV.value = creature.PV;
        sliderPV.maxValue = creature.PVMax;
    }
}
