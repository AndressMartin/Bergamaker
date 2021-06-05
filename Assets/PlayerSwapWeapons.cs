using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwapWeapons : MonoBehaviour
{
    public enum WeaponType
    {
        Pistol,
        Shotgun,
        Sword,
        Punch
    }

    public void SetWeaponType(WeaponType weaponType)
    {
        Debug.Log("Changed weapon type to " + weaponType);
    }
    public void ConsumeHealthPotion()
    {
        Debug.Log("Restored Health");

    }
    public void ConsumeManaPotion()
    {
        Debug.Log("Restored MANA");
    }
}