using UnityEngine;

public class ColorSys : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public bool debug;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChargingColor()
    {
        if (debug) spriteRenderer.color = Color.yellow;
    }

    public void DefaultColor()
    {
        if (debug) spriteRenderer.color = Color.white;
    }
    public void AttackColor()
    {
        if (debug) spriteRenderer.color = Color.red;
    }

    public void DashingColor()
    {
        if (debug) spriteRenderer.color = Color.blue;
    }
}
