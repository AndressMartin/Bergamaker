using UnityEngine;

public class ColorSys : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChargingColor()
    {
        spriteRenderer.color = Color.yellow;
    }

    public void DefaultColor()
    {
        spriteRenderer.color = Color.green;
    }
    public void AttackColor()
    {
        spriteRenderer.color = Color.red;
    }

    public void DashingColor()
    {
        spriteRenderer.color = Color.blue;
    }
}
