using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "New TileData", fileName = "TileData")]
public class TileData : ScriptableObject
{
    public string _tileName;
    public TileBase _tile;
    public float _tileStrength;
    public int _appearsAtDepth;
    public int _disappearsAtDepth;
    public float _minigameCenterWidth;
}
