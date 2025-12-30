using UnityEngine;

[CreateAssetMenu(fileName = "ItemDef", menuName = "Scriptable Objects/ItemDef")]

public class ItemDef : ScriptableObject
{
    public string id = "item_id";
    public GameObject previewPrefab;

    [Min(1)] public int width = 1;
    [Min(1)] public int height = 1;

    public bool rotatable = true;
}
