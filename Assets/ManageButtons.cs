using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ManageButtons : MonoBehaviour
{
    public AtaqueBasico _ataque;
    public BolaDeFogo _bola;
    public List<Button> buttons = new List<Button>();
    public List<Slider> sliders = new List<Slider>();
    private List<Type> generic = new List<Type>();

    // Start is called before the first frame update
    void Start()
    {
        ManageSlider();
        //actions = AddFirstAction();

        //----------------------FUNCIONA!
        //generic = FindAllActions();
        //Debug.Log(generic[0]);
        //Debug.Log(generic[1]);


    }

    // Update is called once per frame
    void Update()
    {
        KeepTrackOfOneSlider();

    }

    void ManageSlider()
    {
        foreach (Button button in buttons)
        {
            int cont = 0;
            sliders[cont] = button.GetComponentInChildren<Slider>();
            sliders[cont].maxValue = _ataque.chargeTimeMax;
            sliders[cont].value = _ataque.chargeTime;
            cont++;
        }
    }
    void KeepTrackOfOneSlider()
    {
        sliders[0].value = _ataque.chargeTime;
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
