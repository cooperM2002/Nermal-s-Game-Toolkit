using UnityEngine;
using UnityEngine.UIElements;

public class InventoryComponent : MonoBehaviour
{

    public int width = 8;
    public int height = 6;

    public GridInventory inventory = new GridInventory();
    //public int W => rotated ? def.height : def.width;    
    // public int W{get{if (rotated) { return def.h; } else { return def.w};}}}
    public GridInventory Inventory => inventory;




    public void Awake()
    {
        if(inventory == null) inventory = new GridInventory();
        inventory.Init(width, height);  
    }

}
