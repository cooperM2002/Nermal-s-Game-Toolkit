using UnityEngine;

public class InventoryTestRunner : MonoBehaviour
{

    public InventoryComponent inv;
    public ItemDef[] testItems;
    public KeyCode openInventory = KeyCode.Tab;


    private void Reset()
    {
        inv= GetComponent<InventoryComponent>();
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(openInventory) && inv != null && testItems.Length > 0)
        {
            ItemDef def = testItems[Random.Range(0, testItems.Length)];
            bool ok = inv.Inventory.TryAdd(def, allowRotate: true, out var placed);
            Debug.Log(ok
                ? $"Placed {def.name} #{placed.instanceId} at ({placed.x},{placed.y}) size {placed.W}x{placed.H}"
                : $"No space for {def.name}");


        }
    }
}
