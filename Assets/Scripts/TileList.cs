using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New TileList", fileName = "TileList")]
public class TileList : ScriptableObject
{
    public List<TileData> _tiles;
}
