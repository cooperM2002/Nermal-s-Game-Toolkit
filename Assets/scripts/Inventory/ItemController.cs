using UnityEngine;
/// <summary>
/// GenericControllerMB
/// 
/// <list type="table">
/// <item>
/// A MonoBehaviour on the player that <b>manages single equipped/held item slot.</b>
/// </item>
/// 
/// <item>
/// <b>ownership:</b>
/// <list type="number">
///     <item> Ref to <b>transform</b> where a held visual is <b>attached.</b></item>
///     <item> Ref to currently equipped definition/data object <b>what the player is holding.</b></item>
///     <item> Ref to currently spawned held visual instance the <b>in-hand object.</b></item>
/// </list>
/// </item>
/// 
/// <item>
/// <b>actions:</b>
/// <list type="number">
///     <item> <b>Pickup</b> (pick up) from a ItemPickup source.</item>
///     <item> <b>Equip</b> (attach/spawn held representation).</item>
///     <item> <b>Drop</b> (spawn a world pickup representation and clear held state).</item>
///     <item> <b>Clear</b> (unequip cleanup).</item>
/// </list>
/// </item>
/// 
/// <item>
/// </item>
/// </list>
/// </summary>
public class ItemController : MonoBehaviour
{

    /*
    * REQUIREMENTS (1-slot equip/drop controller)
    * 
    * Variables
    * 
    *   - a "socket" reference (Transform) (example: where held object goes)
    *   - current data reference (GenericDefinitionSO) (the one equipped)
    *   - current spawned object reference (GameObject) (the visual)
    *
    * Methods to implement later (important ones)
    * 
    *   - HasSomething()
    *   - TryAcquire(GenericPickupMB source)
    *   - Equip(GenericDefinitionSO data)
    *   - Drop()
    *   - Clear()
    */





    ////////////////
    ///  FIELDS  ///
    ////////////////

    //* Position    ->  socket: reference for where held object goes (Transform) 
    //* Attributes  ->  itemdata: reference for current item's data
    //* Object:     ->  physical object: reference for current spawned object reference (GameObject) -> (the visual)

    //socket for held item
    [SerializeField] private Transform itemHoldRoot;//weaponRoot on player

    private ItemDefinition ItemDef;
    private GameObject itemCurrent;



    /////////////////
    ///  METHODS  ///
    /////////////////
    
    //* HasItem()
    //* TryPickupItem(GenericPickupMB source)
    //* EquipItem(GenericDefinitionSO data)
    //* DropItem()
    //* ClearItem()

    private void Awake()
    {
        // TODO: initialization (no implementation yet)
    }

    public bool HasItem()
    {
        // TODO: check if something is equipped
        return default(bool);
    }

    public bool TryPickupItem(ItemDefinition source)
    {
        // TODO: acquire from a source (pickup)
        return default(bool);
    }

    public void EquipItem(ItemDefinition data)
    {
        // TODO: set equipped data + spawn/attach visual
    }

    public void DropItem()
    {
        // TODO: spawn a world object + clear equipped
    }

    public void ClearItem()
    {
        // TODO: clear equipped data + destroy/disable visual
    }


    //void Start()
    //{
    //}
    //void Update()
    //{
    //}
}
