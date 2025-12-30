using UnityEngine;



/// <summary>
/// Represents a <b>physical representation of a pickup</b> 
/// that can be acquired by the player.
/// Connects an object to a data definition.
/// </summary>
public class ItemInteractable : Interactable
{


    /*
    * REQUIREMENTS (important members to add later)
    * 
    * Variables
    * 
    *   - reference to definition/data (GenericDefinitionSO)
    *   - a gate flag (bool) (example: can be used)
    *
    * Methods to implement later
    * 
    *   - a method to provide the definition
    *   - a method to "consume" or remove this object when picked up
    */



    ////////////////
    ///  FIELDS  ///
    ////////////////

    //reference definition
    [SerializeField] private ItemDefinition itemDef;

    //gate flag bool, can be used
    [SerializeField] private bool b;
    public ItemController controller;




    /////////////////
    ///  METHODS  ///
    /////////////////
    //- a method to provide the definition
    //- a method to "consume" or remove this object when picked up

    private void Awake()
    {
        // TODO: initialization

    }


    public override void Interact(GameObject fromObject)
    {
        if (controller == null) { 
            Debug.Log("could not find ItemController, auto assigning");
            controller = fromObject.GetComponentInParent<ItemController>();
            //fromObject.transform.root.GetComponent<ItemController>()
        }

        if (controller == null || itemDef == null)
        {
            return;
        }

        
        bool success = controller.TryPickupItem(itemDef);
        
        if (success)
        {
            Consume();
        }
        else
        {
            Debug.Log("cannot consume");
            return;
        }
    }


    public ItemDefinition GetItemDef()
    {
        // TODO: return the data reference
        return default;
    }

    public void Consume()
    {
        // TODO: remove/disable this object (no implementation yet)
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
