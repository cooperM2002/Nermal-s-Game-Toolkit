using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// stores:
/// <list type="number">
///     <item> IMPORTANT, a given grid entry (x,y) in matrix form is (col,row)->[j][i], since x->horizontal->col, y->vertical->row</item>
///     <item> <b>int width</b>: (n) grids wide, int height: (n) grid high                         </item>
///     <item> <b>int cells[]</b>; 1d array, where the (n-1)th entry = (row y) * width + (col x)   </item>
///     <item> <b>int nextId</b>: assigns next items Id                                            </item>
///     <item> footprint (W,H) base on rotated                                              </item>
///     <item> even Action Changed                                                          </item>
/// </list>
/// </summary>
public class GridInventory
{
    ///////////////////////
    ///    1. FIELDS    /// <--- STATE, persistent
    ///////////////////////
    
    private List<ItemInstance> items = new List<ItemInstance>();
    public IReadOnlyList<ItemInstance> Items => items;  // read only list of items

    private int width = 8;
    private int height = 8;
    private int nextId = 1; // -1 represents empty grid item
    private int[] cells;

    public int Width => width;
    public int Height => height;

    public event Action Changed;


    ////////////////////////////
    ///   2. ITEM INSTANCE   /// <---- placed items
    ////////////////////////////

    /// <summary>
    /// Item instance class, represents an instance of an item. Fields:
    /// <list type="bullet">
    /// 
    ///     <item>  <b>int instanceId</b>:   unique identifier for item instance         </item>
    ///     <item>  <b>itemDef def</b>:      scriptable object data                      </item>
    ///     <item>  <b>bool rotated</b>:     if items inventory rectangles (x,y)->(y,x)  </item>
    ///     <item>  <b>int x</b>:            x-coord of top left of rectangle, column    </item>
    ///     <item>  <b>int y</b>:            y-coord of top left of rectangle, row       </item>
    ///     
    /// </list>
    /// </summary>

    public class ItemInstance
    {
        public int instanceId;
        public ItemDef def;
        public bool rotated;

        public int x;   // top left of grid (0,0)
        public int y;

        public int W => rotated ? def.height : def.width;    // public int W{get{if (rotated) { return def.h; } else { return def.w};}}}
        public int H => rotated ? def.width : def.height;    // public int H{get{if (rotated) { return def.h; }else { return def.w; }}}
    }


    /// <summary> 
    /// resets/(re)configures to default w,h sized inventory grid 
    /// </summary>
    public void Init(int w, int h)
    {
        //define size
        width = Mathf.Max(1,w);     //minimum default of (1,1)
        height = Mathf.Max(1,h);
        cells = new int[width * height];

        //reset to all empty (-1)s
        for (int i = 0; i < cells.Length; i++){
            cells[i] = -1;        
        }
        
        items.Clear();      //clear item list
        nextId = 1;         //starts first ID @ 1, increments nextId++ when new item added
        Changed?.Invoke();  // ???
    }


    /////////////////////////////
    ///   3. HELPER METHODS   ///
    /////////////////////////////

    /// <summary>
    /// Converts a 2D grid coord (x,y) to a 1D array index, output is 0-index'd. ex) CellToIndex(5, 0) = 0*8 + 5 = 5 
    /// </summary>
    /// <returns> array index of grid coords</returns>
    private int CellToIndex(int x, int y)
    {
        /*
        Occupancy grid stored as flat array: cells[width * height]; maps grid coords (x, y)
        row y is width cells so start of row y: @ index y * width" start of row 5 is 5*width
        */
        return y * width + x; 
    }


    /// <summary>
    /// Returns information about cells Owner, emptiness/bounds
    /// <list type="bullet">
    ///     <item> <b>int x</b>: column index </item>
    ///     <item> <b>int y</b>: row index </item>
    /// </list>
    /// </summary>
    /// <returns> <b>(-2)</b>: cell is out of bounds, <b>(-1)</b>: cell is empty, else (instanceId) of occupant item </returns>
    public int GetCellOwner(int x, int y)
    {
        // (x,y) negative or larger than defined size, object too big or negative size
        if(x < 0 || y < 0 || x >=width || y >= height) return -2;
        return cells[CellToIndex(x, y)];
    }


    /// <summary>
    /// GetItem, iterates through items List, if given Id found in List of Items Ids, returns item with that Id.
    /// </summary>
    /// <returns> returns the item which the given Id belongs to.</returns>
    public ItemInstance GetItem(int instanceId)
    {
        for(int i=0; i < items.Count; i++){
            if(items[i].instanceId == instanceId) return items[i]; //ID match in cell
        }
        return null;
    }


    ///////////////////////////
    /// 4. *CHECK FOR FIT*  /// <--Core rules of grid placement
    ///////////////////////////

    /// <summary>
    /// CanPlaceRect
    /// <list type="bullet">
    ///     <item>  <b>int x, int y</b>: grid coords (x,y)->(col,row), top left anchor cell for the rectangle  </item>
    ///     <item>  <b>int w, int h</b>: rectangle size in cells                                               </item>
    ///     <item>  <b>int ignoreId = -1</b>: treats cells occupied by this id as if they were empty           </item>
    /// </list>
    /// </summary>
    /// <returns><b>True</b>: if rectangle <b>(1)</b> is in grid bounds, <b>(2)</b> does not overlap any other items, except its own spot (=-1). <b>False</b> otherwise</returns>
    public bool CanPlaceRect(int x, int y,  int w, int h, int ignoreId = -1)
    {
        //bounds check
        if (x < 0 || y < 0) return false;               //min bounds, no negative values
        if (y+h > height || x+w > width) return false;  //max bounds
        
        for(int dy=0; dy<h; dy++){      //for each row index => along the vertical   => y => row(y) 
            for(int dx=0; dx<w; dx++){  //for each col index => along the horizontal => x => col(x) 

                int owner = cells[CellToIndex(x+dx, y+dy)];
                if(owner != ignoreId && owner !=-1) return false;   //if NOT overlapping owned spot, AND NOT empty (taken) => cant place
            }
        }
        return true;//can place true
    }


    /// <summary>
    /// Fill Rect
    /// <list type="bullet">
    ///     <item>  <b>int ignoreId = -1</b>: treats cells occupied by this id as if they were empty   </item>
    ///     <item>  <b>int x, y</b>: grid coords, top left anchor cell for the rectangle               </item>
    ///     <item>  <b>int w,h</b>: rectangle size in cells                                            </item>
    /// </list>
    /// </summary>
    private void FillRect(int instanceId, int y, int x,  int w, int h)
    {
        for (int dy = 0; dy < h; dy++){         //for each row(y) => along the vertical  
            for (int dx = 0; dx < w; dx++){     //for each col(x) => along the horizontal 
                cells[CellToIndex(x + dx, y + dy)] = instanceId; //fill in rectangle @ (x,y)=>(col, row) coord
            }
        }
    }


    /// <summary>
    /// clears cells filled with given instanceId, replaces cells with -1 => empty
    /// <list type="bullet">
    ///     <item>takes int instanceId, checks which cells are occupied by <i>instance</i> with id, matches, clears them</item>
    /// </list>
    /// </summary>
    private void ClearRect(int instanceId)
    {
        for(int i=0; i<cells.Length; i++){
            if (cells[i] == instanceId){
                cells[i] = -1;//fill with -1 => empty
            }
        }
    }


    //////////////////////////////
    /// 5. FIND 1ST EMPTY SPOT /// <-------- first fit
    //////////////////////////////


    /// <summary>
    /// TryFindFit
    /// <list type="bullet">
    ///     <item>w,h => size of object to be placed</item>
    ///     <item>out foundX(col) foundY(row): found position, top left anchor</item>
    ///     <item>out: if return is true, foundX,foundY are the first valid spot, if return false, ... </item>
    /// </list>
    /// </summary>
    private bool TryFindFit(int w, int h, out int foundX, out int foundY)
    {

        for(int y=0; y<=height-h; y++){
            for(int x=0; x<=width-w; x++){
                if (CanPlaceRect(x, y, w, h)){
                    foundX = x;
                    foundY = y;
                    return true;
                }
            }
        }
        foundX = 0;
        foundY = 0;
        return false;
    }

    private bool TryFindFirstFit(
        ItemDef def, 
        bool allowRotate,
        out int foundX,
        out int foundY, 
        out bool rotated
        )
    {

        //is not rotated
        if(TryFindFit(def.width, def.height, out foundX, out foundY))
        {
            rotated = false;
            return true;
        }

        //is rotated
        if(allowRotate && def.rotatable && def.width != def.height){                //rotatable object, not sqaure
            if (TryFindFit(def.height, def.width, out foundX, out foundY)){

                rotated = true;
                return true;

            }
        }

        foundX = 0; foundY = 0; rotated = false;
        return false;
    }



    //////////////////
    ///  Add item  ///
    //////////////////

    public bool TryAdd(ItemDef def, bool allowRotate, out ItemInstance placed)
    {

        placed = null;

        if (def == null) return false;

        if (!TryFindFirstFit(def, allowRotate, out int x, out int y, out bool rotated)) return false;

        ItemInstance item = new ItemInstance();

        item.instanceId = nextId++;
        item.def = def;

        item.x = x;
        item.y = y;

        item.rotated = rotated; 

        items.Add(item);


        FillRect(item.instanceId, item.x, item.y, item.W, item.H);
        placed = item;

        Changed?.Invoke();
        return true;
    }

    public bool TryMove(int instanceId, int newX, int newY, bool rotated)
    {

        ItemInstance item = GetItem(instanceId);

        if (item == null) return false;


        int newW = rotated ? item.def.height : item.def.width;
        int newH = rotated ? item.def.width : item.def.height;


        // transaction pattern
        ClearRect(instanceId);

        bool canPlace = CanPlaceRect(newX, newY, newW, newH, ignoreId: instanceId);
        if (!canPlace)
        {
            // rollback
            FillRect(item.instanceId, item.y, item.x, item.W, item.H);
            return false;
        }

        // commit
        item.x = newX;
        item.y = newY;
        item.rotated = rotated;

        FillRect(item.instanceId, item.y, item.x, item.W, item.H);
        Changed?.Invoke();

        return true;
    }

    public bool TryRotateInPlace(int instanceId)
    {
        ItemInstance it = GetItem(instanceId);
        if (it == null) return false;
        if (!it.def.rotatable) return false;

        return TryMove(instanceId, it.x, it.y, !it.rotated);
    }

}



