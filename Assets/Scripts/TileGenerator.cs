using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] Tilemap _diggableTileMap;
    [SerializeField] Tilemap _coinMap;
    public TileList _tileList;
    List<TileData> _activeTilesInRow = new List<TileData>();
    int[,] _cellValues;
    Array2D<int> _coinValues;

    [SerializeField] Transform _startTrans;
    [SerializeField] Transform _rightMostTrans;
    [SerializeField] Transform _bottomMostTrans;
    Vector3Int _startCell;
    Vector3Int _rightMostCell;
    Vector3Int _bottomMostCell;
    int _diggableAreaX;
    int _diggableAreaY;

    [Space]
    [SerializeField] float _noiseMagnification = 7f;
    [SerializeField] bool _randomOffsets = true; //Each time tiles are generated, offsets are randomized
    [SerializeField] float _noiseXOffset = 0f;
    [SerializeField] float _noiseYOffset = 0f;

    [Space]
    public int _patternIndex = -1;
    [SerializeField] int _coinStartYOffset = 4; //Coin patterns begin to generate at this depth
    [SerializeField] int _coinPatternDepth = 8; //Coin patterns are only this many units tall
    int _coinValueOffset = 0;

    private void Awake() 
    {
        DeclareVariables();
    }

    private void DeclareVariables()
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

    public void FillCoinMap() //Needs to be fixed
    {
        Vector3Int currentCell;
        for (int y = 0; y < _diggableAreaY; y++)
        {
            if (y == _coinStartYOffset || (y - _coinStartYOffset) % _coinPatternDepth == 0) 
            {
                _patternIndex = FindRandomCoinPattern();
                _coinValues = _tileList._patterns[_patternIndex];
            }

            for (int x = 0; x < _diggableAreaX; x++)
            {
                currentCell = new Vector3Int(_startCell.x + x, _startCell.y - y, _startCell.z);

                //If _coinValues[x,y] has a value, set its tile by using its value as an index in collectables[]
                //The offset also adds to the value, e.g. deeper you go -> higher the value
                if (_coinValues[x, y] == -1) continue;
                _coinMap.SetTile(currentCell, _tileList._collectables[_coinValues[x, y] + _coinValueOffset]._tile);
            }
        }
    }
    private int FindRandomCoinPattern()
    {
        float index = UnityEngine.Random.Range(0.0f, _tileList._patterns.Count + 1f);
        return (int)Mathf.Floor(index);
    }

    //Used to switch out coins/valuables found as depth increases
    public void UpdateCoins(int indexOffset)
    {
        _coinValueOffset += indexOffset;
    }

//-------=== Editor Functions ===-------

    //Editor function that stores the created coin pattern to TileList._coinPatterns
    public void StoreCoinPattern()
    {
        DeclareVariables();

        Vector3Int currentCoinCell;
        Array2D<int> patternValues = new Array2D<int>(_diggableAreaX, _coinPatternDepth);
        for (int y = 0; y < _coinPatternDepth; y++)
        {
            for (int x = 0; x < _diggableAreaX; x++)
            {
                currentCoinCell = new Vector3Int(_startCell.x + x, _startCell.y - y, _startCell.z);

                patternValues[x, y] = CheckTileAgainstList(currentCoinCell);
            }
        }
        _tileList._patterns.Add(patternValues);
        print("New pattern count is: " + _tileList._patterns.Count);
    }
    private int CheckTileAgainstList(Vector3Int cell) //May be able to be a generalized method?
    {
        int index = -1; //TODO:add an error tile and set this as the error tile's index

        if (_coinMap.GetTile(cell) == null) return -1;
        //Compares current tile against the list of collectables and assigns _coinValues[x,y] to the index if found.
        for (int c = 0; c < _tileList._collectables.Count; c++)
        {
            if (_coinMap.GetTile(cell) == _tileList._collectables[c]._tile)
            {
                index = c;
                break;
            }
        }

        return index;
    }

    //Editor function used to check individual saved patterns
    public void FillCoinPattern(int index)
    {
        DeclareVariables();
        
        if (index < 0 || index > _tileList._patterns.Count)
        {
            Debug.LogError("_patternIndex must be within range of _tileList._patterns.Count!");
            return;
        }

        _coinMap.ClearAllTiles();
        
        Vector3Int currentCell;
        Array2D<int> patternValues =  _tileList._patterns[index];
        for (int y = 0; y < patternValues.Get_Y_Length; y++)
        {
            for (int x = 0; x < patternValues.Get_X_Length; x++)
            {
                currentCell = new Vector3Int(_startCell.x + x, _startCell.y - y, _startCell.z);

                //If _patternValues[x,y] has a value, set its tile by using its value as an index in collectables[]
                if (patternValues[x, y] == -1) continue;
                _coinMap.SetTile(currentCell, _tileList._collectables[patternValues[x, y]]._tile);
            }
        }
    }

    //Editor Function to remove saved coin patterns
    public void RemoveCoinPattern(int index)
    {
        if (index < 0 || index > _tileList._patterns.Count)
        {
            Debug.LogError("_patternIndex must be within range of _tileList._patterns.Count!");
            return;
        }

        _tileList._patterns.RemoveAt(index);
        print("New pattern count is: " + _tileList._patterns.Count);
    }
}
