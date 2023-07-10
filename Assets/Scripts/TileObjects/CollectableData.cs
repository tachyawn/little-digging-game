using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "New CollectableData", fileName = "CollectableData")]
public class CollectableData : ScriptableObject
{
    public string _name;
    public Tile _tile;
    public int _value = 0;
}
