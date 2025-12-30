using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// <i>stores which grid cell a tile represents</i>
/// <list type="number">
/// <item> each inventory cell becomes tile gameobject </item>
/// <item> tiles live on <b>XZ</b> plane
/// <list type="">
///     <item><b>X</b> = column -> maps to world X  </item>
///     <item><b>Y</b> = row -> maps to world Z     </item>
/// </list>
/// </item>
/// </list>
/// </summary>
public class InventoryGridView3D : MonoBehaviour
{
    
    public InventoryComponent invComp;
    private Dictionary<int, GameObject> spawnedItemsDict = new Dictionary<int, GameObject>();


    [Header("Grid Visual")]
    public GameObject cellPrefab;
    public float cellSize = 0.25f;  //m per cell
    public float panelZ = 0f;     // plane the cells live on (XY panel)
    public float itemZ = -0.02f; // slightly in front of cells

    private Transform cellRoot;
    private Transform itemRoot;

    void Awake()
    {
        ///////////////////
        /// BUILD CELLS ///
        ///////////////////
        if(invComp==null) invComp = GetComponent<InventoryComponent>();

        cellRoot = new GameObject("Cells").transform;    //attatches transform to new empty
        cellRoot.SetParent(transform, false);               //worldPositionStays = false, 





        BuildCells();

        ////////////////////
        ///  SHOW ITEMS  ///
        ////////////////////
        invComp.Inventory.Changed += RefreshItems;

        itemRoot = new GameObject("items").transform;
        itemRoot.SetParent(transform, false);

        RefreshItems();
    }


    void OnDestroy()
    {
        if (invComp != null && invComp.Inventory != null)
            invComp.Inventory.Changed -= RefreshItems;
    }


    Vector3 CellCenterLocal(GridInventory inv, int col, int row, float z)
    {
        float offsetX = inv.Width * cellSize * 0.5f;
        float offsetY = inv.Height * cellSize * 0.5f;

        float x = (col + 0.5f) * cellSize - offsetX;
        float y = (row + 0.5f) * cellSize - offsetY; // keep your “row goes up” convention
        return new Vector3(x, y, z);
    }

    void RefreshItems()
    {
        //build list of items to remove
        GridInventory inv = invComp.Inventory;
        List<int> itemsRefresh = new List<int>();
        foreach(KeyValuePair<int, GameObject> kv_pair in spawnedItemsDict)
        {
            if (inv.GetItem(kv_pair.Key) == null)
            {
                itemsRefresh.Add(kv_pair.Key);
            }
        }
        foreach(int id in itemsRefresh)
        {
            Destroy(spawnedItemsDict[id]);
            spawnedItemsDict.Remove(id);
        }

        foreach(GridInventory.ItemInstance item in inv.Items)
        {
            
            if(!spawnedItemsDict.TryGetValue(item.instanceId, out GameObject refreshed)){
                
                //container for this item
                refreshed = new GameObject($"Item_{item.instanceId}_{item.def.name}");
                refreshed.transform.SetParent(itemRoot, false);

                //spawn model
                if (item.def.previewPrefab != null) { 
                    GameObject model = Instantiate(item.def.previewPrefab, refreshed.transform);
                    model.name = "Model";

                    //disable raycasts
                    foreach(var col in model.GetComponentsInChildren<Collider>())
                    {
                        col.enabled = false;
                    }
                }
                spawnedItemsDict[item.instanceId] = refreshed;



            }

            float offsetX = inv.Width * cellSize * 0.5f;
            float offsetY = inv.Height * cellSize * 0.5f;

            float x = (item.x + item.W * 0.5f) * cellSize - offsetX;
            float y = (item.y + item.H * 0.5f) * cellSize - offsetY;

            refreshed.transform.localPosition = new Vector3(x, y, itemZ);
            refreshed.transform.localRotation = Quaternion.identity;


        }

    }


    /// <summary>
    /// builds grid of gameobject cells, tags cells with coordinates
    /// </summary>
    void BuildCells()
    {
        //clear old cells
        for(int i = cellRoot.childCount - 1; i>=0; i--)
        {
            Destroy(cellRoot.GetChild(i).gameObject);//destroy all children
        }

        GridInventory inv = invComp.Inventory;

        //loop over (height x width) = (rows x cols) = total # cells
        for (int row = 0; row < inv.Height; row++) 
        { 
            for(int col=0;col<inv.Width; col++)
            {

                // 1. INSTANTIATE NEW GAMEOBJECT
                GameObject cell = Instantiate(cellPrefab, cellRoot);
                cell.name = $"Cell: ({col},{row})";
                //parent
                //cell.transform.SetParent(cellRoot, false); 

                // 2. CENTER OF CELL IN LOCAL SPACE
                cell.transform.localPosition = CellCenterLocal(inv, col, row, panelZ);
                cell.transform.localRotation = Quaternion.identity;


                // 3. SCALE PREFAB TO MATCH CELLSIZE (1x1 in XZ)
                Vector3 scale = cell.transform.localScale;
                cell.transform.localScale = new Vector3(cellSize, scale.y, cellSize);

                // 4. TAG CELLS WITH COORDS
                CellTag tag = cell.GetComponent<CellTag>();
                if(tag==null) tag = cell.AddComponent<CellTag>();
                tag.x = col;
                tag.y = row;
            
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
