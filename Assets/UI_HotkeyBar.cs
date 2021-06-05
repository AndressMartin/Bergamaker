using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HotkeyBar : MonoBehaviour
{
    private Transform abilitySlotTemplate;
    private HotkeyAbilitySystem hotkeyAbilitySystem;
    private void Awake()
    {
        abilitySlotTemplate = transform.Find("AbilitySlotTemplate");
        abilitySlotTemplate.gameObject.SetActive(false);
    }

    public void SetHotKeyAbilitySystem(HotkeyAbilitySystem hotKeyAbilitySystem)
    {
        this.hotkeyAbilitySystem = hotKeyAbilitySystem;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        List<HotkeyAbilitySystem.HotkeyAbility> hotkeyAbilityList = hotkeyAbilitySystem.GetHotkeyAbilityList();
        for (int i = 0; i < hotkeyAbilityList.Count; i++)
        {
            HotkeyAbilitySystem.HotkeyAbility hotkeyAbility = hotkeyAbilityList[i];
            Transform abilitySlotTransform = Instantiate(abilitySlotTemplate, transform);
            abilitySlotTransform.gameObject.SetActive(true);
            RectTransform abilitySlotRectTransform = abilitySlotTransform.GetComponent<RectTransform>();
            abilitySlotRectTransform.anchoredPosition = new Vector2(100f * i, 0f);
            abilitySlotTransform.Find("ItemImage").GetComponent<Image>().sprite = hotkeyAbility.GetSprite();
            abilitySlotTransform.Find("NumberText").GetComponent<TextMeshProUGUI>().SetText((i + 1).ToString());
            abilitySlotRectTransform.GetComponent<UI_HotkeyBarAbilitySlot>().SetHotKeyAbility(hotkeyAbility);
        }
    }
}
