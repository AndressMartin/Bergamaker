using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotkeyAbilitySystem
{
    public enum AbilityType
    {
        Pistol,
        Shotgun,
        Sword,
        Punch,
        HealthPotion,
    }
    private PlayerSwapWeapons player;
    private List<HotkeyAbility> hotkeyAbilityList;
    public HotkeyAbilitySystem(PlayerSwapWeapons player)
    {
        this.player = player;
        hotkeyAbilityList = new List<HotkeyAbility>();
        hotkeyAbilityList.Add(new HotkeyAbility
        //Pistol
        {
            abilityType = AbilityType.Pistol,
            activateAbilityAction = () => player.SetWeaponType(PlayerSwapWeapons.WeaponType.Pistol)
        });
        //Shotgun
        hotkeyAbilityList.Add(new HotkeyAbility
        {
            abilityType = AbilityType.Shotgun,
            activateAbilityAction = () => player.SetWeaponType(PlayerSwapWeapons.WeaponType.Shotgun)
        });
        //Sword
        hotkeyAbilityList.Add(new HotkeyAbility
        {
            abilityType = AbilityType.Sword,
            activateAbilityAction = () => player.SetWeaponType(PlayerSwapWeapons.WeaponType.Sword)
        });
        //Punch
        hotkeyAbilityList.Add(new HotkeyAbility
        {
            abilityType = AbilityType.Punch,
            activateAbilityAction = () => player.SetWeaponType(PlayerSwapWeapons.WeaponType.Punch)
        });
        hotkeyAbilityList.Add(new HotkeyAbility
        {
            abilityType = AbilityType.HealthPotion,
            activateAbilityAction = () => player.ConsumeHealthPotion()
        });
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            hotkeyAbilityList[0].activateAbilityAction();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            hotkeyAbilityList[1].activateAbilityAction();

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            hotkeyAbilityList[2].activateAbilityAction();

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            hotkeyAbilityList[3].activateAbilityAction();

        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            hotkeyAbilityList[4].activateAbilityAction();

        }
    }
    public List<HotkeyAbility> GetHotkeyAbilityList()
    {
        return hotkeyAbilityList;
    }
    public class HotkeyAbility
    {
        public AbilityType abilityType;
        public Action activateAbilityAction;

        public Sprite GetSprite()
        {
            switch (abilityType)
            {
                default:
                case AbilityType.Pistol:        return Testing.Instance.pistolSprite;
                case AbilityType.Shotgun:       return Testing.Instance.shotgunSprite;
                case AbilityType.Sword:         return Testing.Instance.swordSprite;
                case AbilityType.Punch:         return Testing.Instance.punchSprite;
                case AbilityType.HealthPotion:  return Testing.Instance.healthPotionSprite;

            }
        }
    }
}
