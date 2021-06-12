using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerStats : MonoBehaviour
{
    public Text PV, PVMax, MN, MNMax, PA, PAMax; 
    public Player player;
    public Slider sliderPA, sliderPV, sliderMN;

    void Update()
    {
        SetMainStats();
        SetMaxStats();
        SetSlider();
    }

    private void SetMainStats()
    {
        PV.text = $"{player.PV}";
        MN.text = $"{player.MN}";
        PA.text = $"{player.PA}";
    }

    private void SetMaxStats()
    {
        PVMax.text = $"{player.PVMax}";
        MNMax.text = $"{player.MNMax}";
        PAMax.text = $"{player.PAMax}";
    }

    private void SetSlider()
    {
        sliderPV.value = player.PV;
        sliderPV.maxValue = player.PVMax;
        sliderMN.value = player.MN;
        sliderMN.maxValue = player.MNMax;
        sliderPA.value = player.PA;
        sliderPA.maxValue = player.PAMax;
    }
}
