using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerStats : MonoBehaviour
{
    public Text mainStats;
    public Text MaxStats;
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
        mainStats.text = $"PA: {player.PA}\nPV: {player.PV}\nMN: {player.MN}";
    }

    private void SetMaxStats()
    {
        MaxStats.text = $"{player.PAMax}\n{player.PVMax}\n{player.MNMax}";
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
