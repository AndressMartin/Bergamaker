using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarController : MonoBehaviour
{
    public static ToolbarController Instance { get; private set; }

    [SerializeField] private PlayerSwapWeapons player;
    [SerializeField] private UI_HotkeyBar uiHotkeyBar;

    public Sprite pistolSprite;
    public Sprite shotgunSprite;
    public Sprite swordSprite;
    public Sprite punchSprite;
    public Sprite healthPotionSprite;

    private HotkeyAbilitySystem hotkeyAbilitySystem;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        hotkeyAbilitySystem = new HotkeyAbilitySystem(player);
        uiHotkeyBar.SetHotKeyAbilitySystem(hotkeyAbilitySystem);
    }
    private void Update()
    {
        hotkeyAbilitySystem.Update();
    }
}
