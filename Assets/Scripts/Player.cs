using System.Collections;
using UnityEngine;

public class Player: EntityModel
{
    
    [SerializeField] public override int PVMax { get; protected set; }
    [SerializeField] public override int MNMax { get; protected set; }
    [SerializeField] public override int PAMax { get; protected set; }
    [SerializeField] public override int PV { get; protected set; }
    [SerializeField] public override int MN { get; protected set; }
    [SerializeField] public override int PA { get; protected set; }

    [SerializeField] private int FOR, DEX, INT, MEM, ATE;

    protected float ForAtt, DexAtt, Vel, Slots, SkillPt, CritCh, Pre, EsqCh, Def, Esp, Ten;
    protected float ResAgu, ResTer, ResAr, ResFog, ResLuz, ResSom;
    

    [SerializeField] private UI_Inventory uiInventory;
    public Inventory inventory;
    private GameObject child;

    // Start is called before the first frame update
    void Start()
    {
        SetMaxTestStats();
        PVMax = PVMaxCalc();
        MNMax = MNMaxCalc();
        PAMax = PAMaxCalc();
        SetTestStats();

        inventory = new Inventory(UseItem);
        uiInventory.SetPlayer(this);
        uiInventory.SetInventory(inventory);
        child = transform.GetChild(0).gameObject;

        child.GetComponent<ItemCollision>().inventory = inventory;
    }
    //Player Methods
    private int PVMaxCalc()
    {
        return FOR * 10;
    }
    private int MNMaxCalc()
    {
        return INT * 10;
    }
    private int PAMaxCalc()
    {
        return DEX * 10;
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Inventory GetInventory()
    {
        return inventory;
    }
    public Inventory SendInventoryChild()
    {
        return inventory;
    }
   

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                Debug.Log("cura");

                inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                break;

            case Item.ItemType.ManaPotion:
                Debug.Log("mana");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });
                break;

        }
    }
    // Update is called once per frame
    
    
    private void SetMaxTestStats()
    {
        FOR = Random.Range(10, 20);
        DEX = Random.Range(10, 20);
        INT = Random.Range(10, 20);
        MEM = Random.Range(10, 20);
        ATE = Random.Range(10, 20);
    }
    private void SetTestStats()
    {
        PV = PVMax - Random.Range(60, PVMax);
        MN = MNMax - Random.Range(60, MNMax);
        PA = PAMax - Random.Range(60, PAMax);
    }

    //Regen
    
}
