using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_HotkeyBarAbilitySlot : MonoBehaviour, IPointerDownHandler
{
    private Canvas canvas;
    public HotkeyAbilitySystem.HotkeyAbility hotkeyAbility;

    public void SetHotKeyAbility(HotkeyAbilitySystem.HotkeyAbility hotkeyAbility)
    {
        this.hotkeyAbility = hotkeyAbility;
    }
    //Event gets called whenever we press down on this object
    public void OnPointerDown(PointerEventData eventData)
    {
        hotkeyAbility.activateAbilityAction();
    }
}
