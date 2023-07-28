using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StoreCoinData : MonoBehaviour
{
    public TileList _tileList;
    Tilemap _coinMap;
    Vector3Int _startCell;
    FileDataHandler _dataHandler;

    PatternList _patternList = new PatternList();
    public int _patternCount => _patternList.Get_Patterns;

    public static string patternFileDirectory = "Assets/StaticGameData";
    public static string patternFileName = "patternData.game";
    public int _patternIndex = -1;
    int _diggableAreaX = 16;
    int _coinPatternDepth = 8;

    private void InitializeVariables()
    {
        _coinMap = GameObject.FindGameObjectWithTag("CoinTileMap").GetComponent<Tilemap>();
        _startCell = _coinMap.WorldToCell(GameObject.FindGameObjectWithTag("StartingCell").transform.position);
    }

    public void LoadPatternData(bool printData)
    {
        _dataHandler = new FileDataHandler(patternFileDirectory, patternFileName);
        //Checks if patternList exists within directory, if so assign _patternList to it, else create a new instance of PatternList
        _patternList = _dataHandler.TryLoadPattern(out PatternList pList) ? pList : new PatternList(); 

        if (printData)
        {
            print($"Patterns: {_patternList.Get_Patterns}, Height: {_patternList.Get_Y_Length}, Depth: {_patternList.Get_Z_Length}");
            print($"Total Length: {_patternList.Length}, isEmpty: {_patternList.isEmpty}");
        }
    }

    //Editor function that stores the created coin pattern to TileList._coinPatterns
    public void StoreCoinPattern()
    {
        LoadPatternData(false);
        InitializeVariables();

        Vector3Int currentCoinCell;
        Array2D patternValues = new Array2D();
        for (int y = 0; y < _coinPatternDepth; y++)
        {
            for (int x = 0; x < _diggableAreaX; x++)
            {
                currentCoinCell = new Vector3Int(_startCell.x + x, _startCell.y - y, _startCell.z);

                patternValues[x, y] = CheckTileAgainstList(currentCoinCell);
            }
        }
        _patternList.Add(patternValues);
        _dataHandler.SavePattern(_patternList);
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
        LoadPatternData(false);
        InitializeVariables();

        if (index < 0 || index >= _patternList.Get_Patterns)
        {
            Debug.LogError("_patternIndex must be within range of _tileList._patterns.Count!");
            return;
        }
        _coinMap.ClearAllTiles();
        
        Vector3Int currentCell;
        Array2D patternValues = new Array2D();
        patternValues.SingleArray = _patternList.GetPattern(index);
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
        LoadPatternData(false);

        if (index < 0 || index > _patternList.Get_Patterns)
        {
            Debug.LogError("_patternIndex must be within range of _patternList.Count!");
            return;
        }

        _patternList.RemovePatternAt(index);
        _dataHandler.SavePattern(_patternList);
        print("New pattern count is: " + _patternList.Get_Patterns);
    }

    public void ClearPatterns()
    {
        LoadPatternData(false);

        _patternList.ClearPatterns();
        _dataHandler.SavePattern(_patternList);
    }
}
