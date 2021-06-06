using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageButtons : MonoBehaviour
{
    public GameObject SkillManager;
    public Transform abilitySlotTemplate;
    public List<ActionModel> quickActions = new List<ActionModel>();
    public List<Button> buttons = new List<Button>();
    public List<Slider> sliders = new List<Slider>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in SkillManager.transform)
        {
            quickActions.Add(child.GetComponent<ActionModel>());
        }
        UpdateVisual();
        ManageSlider();
    }

    private void UpdateVisual()
    {
        for (int i = 0; i < quickActions.Count; i++)
        {
            ActionModel skill = quickActions[i];
            Transform abilitySlotTransform = Instantiate(abilitySlotTemplate, transform);
            abilitySlotTransform.gameObject.SetActive(true);
            buttons.Add(abilitySlotTransform.GetComponent<Button>());
            RectTransform abilitySlotRectTransform = abilitySlotTransform.GetComponent<RectTransform>();
            abilitySlotRectTransform.anchoredPosition = new Vector2(50f * i, 0f);
            abilitySlotTransform.Find("ItemImage").GetComponent<Image>().sprite = skill.GetSkillSprite();
            abilitySlotTransform.Find("NumberText").GetComponent<Text>().text = ((i + 1).ToString());
            sliders.Add(abilitySlotTransform.Find("Slider").GetComponent<Slider>());
            //abilitySlotRectTransform.GetComponent<UI_HotkeyBarAbilitySlot>().SetHotKeyAbility(skill);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < quickActions.Count; i++)
        {
            KeepTrackOfOneSlider(sliders[i], quickActions[i]);
        }
    }

    void ManageSlider()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            sliders[i] = buttons[i].GetComponentInChildren<Slider>();
            sliders[i].maxValue = quickActions[i].chargeTimeMax;
            sliders[i].value = quickActions[i].chargeTime;
        }
    }
    void KeepTrackOfOneSlider(Slider slider, IAction script)
    {
        slider.value = script.chargeTime;
            
    }
    List<Type> FindAllActions()
    {
        var type = typeof(IAction);
        Debug.Log("type: " + type);
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p)).ToList();
    }
}
