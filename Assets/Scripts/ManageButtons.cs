using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ManageButtons : MonoBehaviour
{
    public List<ActionModel> quickActions = new List<ActionModel>();
    public List<Button> buttons = new List<Button>();
    public List<Slider> sliders = new List<Slider>();

    // Start is called before the first frame update
    void Start()
    {
        ManageSlider();
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
        for (int i = 0; i <= buttons.Count; i++)
        {
            sliders[i] = buttons[i].GetComponentInChildren<Slider>();
            sliders[i].maxValue = quickActions[i].chargeTimeMax;
            sliders[i].value = quickActions[i].chargeTime;
        }
    }
    void KeepTrackOfOneSlider(Slider slider, IAction script)
    {
        slider.value = script.chargeTime;
        //Debug.Log(script.efeito);
            
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
