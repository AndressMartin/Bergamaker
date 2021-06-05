using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public  class ItemSounds : MonoBehaviour
{
    [SerializeField]
    private AudioSource Sword;
    [SerializeField]
    private AudioSource HealthPotion;
    [SerializeField]
    private AudioSource ManaPotion;
    [SerializeField]
    private AudioSource Coin;
    [SerializeField]
    private AudioSource Medkit;

    private  string nomeitem;
    public  ItemSounds Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
   
    public void main(Item item)
    {
        /*nomeitem = "";
        nomeitem = item.GetItemtype();
        PlaySound(SelectSound(nomeitem));*/
        PlaySound(SelectSound(item.GetItemtype()));
   

    }
    private  AudioSource SelectSound(string item)
    {
        switch (item)
        {
            default:
            case "Sword":  return Sword;
            case "HealthPotion": return HealthPotion;
            case "ManaPotion": return ManaPotion;
            case "Coin": return Coin;
            case "Medkit": return Medkit;
        }
    }
    private void PlaySound(AudioSource som)
    {
        som.Play();
    }



}
