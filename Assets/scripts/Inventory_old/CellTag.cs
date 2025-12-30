using UnityEngine;

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
public class CellTag : MonoBehaviour
{

    public int x;   //col
    public int y;   //row

}
