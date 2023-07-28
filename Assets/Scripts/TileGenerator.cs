using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] TileList _tileList;
    Tilemap _diggableTileMap;
    List<TileData> _activeTilesInRow = new List<TileData>();
    Array2D _cellValues;
    Vector3Int _startCell;
    
    public static int _diggableAreaX = 16;
    public static int _diggableAreaY = 16;

    [Space]
    [SerializeField] float _noiseMagnification = 7f;
    [SerializeField] bool _randomOffsets = true; //Each time tiles are generated, offsets are randomized
    [SerializeField] float _noiseXOffset = 0f;
    [SerializeField] float _noiseYOffset = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        _cellValues = new Array2D(_diggableAreaX, _diggableAreaY);
        _diggableTileMap = GameObject.FindGameObjectWithTag("DiggableTileMap").GetComponent<Tilemap>();
        _startCell = _diggableTileMap.WorldToCell(GameObject.FindGameObjectWithTag("StartingCell").transform.position);

        GenerateNewTiles();
    }

    public TileData GetSelectedTileData(Vector3Int selectedCell)
    {
        TileData selectedTileData = null;
        //Selected x and y are calculated based on their distance from the furthest x and y values, e.g. _diggableAreaX and _diggableAreaY
        selectedCell.x = _diggableAreaX - ((_diggableAreaX - selectedCell.x) + 1); //May be broken?
        selectedCell.y = _diggableAreaY - ((selectedCell.y - _diggableAreaY) + 1); //May be broken?

        if (selectedCell.x > -1 && selectedCell.y > -1 && selectedCell.x < _diggableAreaX && selectedCell.y < _diggableAreaY)
        {
            print("Valid tile selected");
            selectedTileData = _tileList._tiles[_cellValues[selectedCell.x, selectedCell.y]];
        }
        return selectedTileData;
    }

    public void GenerateNewTiles()
    {
        if (_randomOffsets)
        {
            _noiseXOffset = UnityEngine.Random.Range(0f, 50f);
            _noiseYOffset = UnityEngine.Random.Range(0f, 50f);
        }
        AssignCellTileValues();
        FillTileMap();
    }

    private void AssignCellTileValues()
    {
        for (int y = 0; y < _diggableAreaY; y++)
        {
            _activeTilesInRow.Clear();
            for (int t = 0; t < _tileList._tiles.Count; t++)
            {
                //Updates _activeTilesInRow each y iteration, based on the tileData's appearsAt and dissapearsAt values
                if (y >= _tileList._tiles[t]._appearsAtDepth && y < _tileList._tiles[t]._disappearsAtDepth)
                {
                    _activeTilesInRow.Add(_tileList._tiles[t]);
                }
            }

            for (int x = 0; x < _diggableAreaX; x++)
            {
                //If there's only one possible tile, skip PerlinNoiseValue() and just assign the cell value manually
                if (_activeTilesInRow.Count == 1) _cellValues[x, y] = _tileList._tiles.IndexOf(_activeTilesInRow[0]);
                else
                {
                    //Sets cellvalues[x, y] to the index of whichever active tile in the full tilelist.
                    _cellValues[x, y] = _tileList._tiles.IndexOf(_activeTilesInRow[PerlinNoiseValue(x, y, _activeTilesInRow.Count)]);
                }
            }
        }
    }
    private int PerlinNoiseValue(int xPos, int yPos, int tiles)
    {
        float temp = Mathf.PerlinNoise(
            (xPos - _noiseXOffset) / _noiseMagnification, 
            (yPos - _noiseYOffset) / _noiseMagnification);

        temp = Mathf.Clamp(temp, 0.0f, 1.0f); //Clamp so values arent above one (can apparently happen)
        temp *= tiles; //Scale the noise by the number of tiles

        if (temp == tiles) temp--; //Prevents a value greater than num of tiles from appearing

        return Mathf.FloorToInt(temp);
    }

    private void FillTileMap()
    {
        Vector3Int currentCell;
        for (int y = 0; y < _diggableAreaY; y++)
        {
            for (int x = 0; x < _diggableAreaX; x++)
            {
                currentCell = new Vector3Int(_startCell.x + x, _startCell.y - y, _startCell.z);
                _diggableTileMap.SetTile(currentCell, _tileList._tiles[_cellValues[x, y]]._tile);
            }
        }
    }
}
