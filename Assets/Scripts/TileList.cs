using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New TileList", fileName = "TileList")]
public class TileList : ScriptableObject
{
    public List<TileData> _tiles;
    public List<CollectableData> _collectables;

    //Stored list of coin patterns to be randomly generated 
    public List<Array2D<int>> _patterns = new List<Array2D<int>>();
}
