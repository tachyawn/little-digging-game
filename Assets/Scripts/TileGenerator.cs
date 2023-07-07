using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] Tilemap _diggableTileMap;
    [SerializeField] TileList _tileList;
    List<TileData> _activeTilesInRow = new List<TileData>();
    int[,] _cellValues;

    [SerializeField] Transform _startTrans;
    [SerializeField] Transform _rightMostTrans;
    [SerializeField] Transform _bottomMostTrans;
    Vector3Int _startCell;
    Vector3Int _rightMostCell;
    Vector3Int _bottomMostCell;

    [Space]
    [SerializeField] float _noiseMagnification = 7f;
    [SerializeField] bool _randomOffsets = true;
    [SerializeField] float _noiseXOffset = 0f;
    [SerializeField] float _noiseYOffset = 0f;
    int _diggableAreaX;
    int _diggableAreaY;

    private void Awake() 
    {
        _startCell = _diggableTileMap.WorldToCell(_startTrans.position);
        _rightMostCell = _diggableTileMap.WorldToCell(_rightMostTrans.position);
        _bottomMostCell = _diggableTileMap.WorldToCell(_bottomMostTrans.position);

        _diggableAreaX = (_rightMostCell.x - _startCell.x) + 1; //Add one to compensate for dist. e.g (0.5 -> 1.5) should be 2 cells,
        _diggableAreaY = (_startCell.y - _bottomMostCell.y) + 1; //But subtraction makes it so we get one less than intended
        _cellValues = new int[_diggableAreaX, _diggableAreaY];
    }

    // Start is called before the first frame update
    private void Start()
    {
        GenerateNewTiles();
    }

    public TileData GetSelectedTileData(Vector3Int selectedCell)
    {
        TileData selectedTileData = null;
        selectedCell.x = _diggableAreaX - ((_rightMostCell.x - selectedCell.x) + 1);
        selectedCell.y = _diggableAreaY - ((selectedCell.y - _bottomMostCell.y) + 1);

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

    private void AssignCellTileValues()//TODO add params for stuff like 
    {
        for (int y = 0; y < _diggableAreaY; y++)
        {
            _activeTilesInRow.Clear();
            for (int t = 0; t < _tileList._tiles.Count; t++)
            {
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

        temp = Mathf.Clamp(temp, 0.0f, 1.0f); //Clamp so values arent above one (can happen)
        temp *= tiles; //Scale the noise by the number of tiles

        if (temp == tiles) temp--; //Prevents a value greater than num of tiles from appearing

        return Mathf.FloorToInt(temp);
    }

    private void FillTileMap()
    {
        for (int y = 0; y < _diggableAreaY; y++)
        {
            for (int x = 0; x < _diggableAreaX; x++)
            {
                Vector3Int currentCell = new Vector3Int(_startCell.x + x, _startCell.y - y, _startCell.z);
                _diggableTileMap.SetTile(currentCell, _tileList._tiles[_cellValues[x, y]]._tile);
            }
        }
    }
}
