using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField] TileList _tileList;
    Tilemap _coinMap;
    FileDataHandler _dataHandler;
    PatternList _patternList;
    Vector3Int _startCell;

    [SerializeField] int _startDrawYOffset = 4; //Coin patterns begin to generate at this depth
    [SerializeField] int _depthTilPatternRepeat = 2; //Empty levels between coin patterns
    int _coinValueOffset = 0;
    int _patternsGenerated = 0;
    int _prevCoinPattern = -1;

    private void Start() 
    {
        _dataHandler = new FileDataHandler(StoreCoinData.patternFileDirectory, StoreCoinData.patternFileName);
        _patternList = _dataHandler.LoadPattern();

        _coinMap = GameObject.FindGameObjectWithTag("CoinTileMap").GetComponent<Tilemap>();
        _startCell = _coinMap.WorldToCell(GameObject.FindGameObjectWithTag("StartingCell").transform.position);

        _coinMap.ClearAllTiles();
        FillCoinMap();
    }

    public void FillCoinMap()
    {
        int waitingFloor = 0;
        int localDepth = 0;
        bool drawingPattern = false;
        Array2D _coinValues = new Array2D();

        for (int y = 0; y < TileGenerator._diggableAreaY; y++)
        {
            if (y < _startDrawYOffset) continue;
            
            if (!drawingPattern)
            {
                waitingFloor++;
                continue;
            }

            if (!drawingPattern && waitingFloor == _depthTilPatternRepeat)
            {
                drawingPattern = true;
                waitingFloor = 0;

                _coinValues.SingleArray = _patternList.GetPattern(FindRandomCoinPattern());

                //Every three patterns, coin value increases
                _patternsGenerated++;
                if (_patternsGenerated % 2 == 0) UpdateCoins(1);
            }

            AssignCoinTile(localDepth, y, _coinValues);

            if (drawingPattern) localDepth++;
            else if (localDepth > _coinValues.Get_Y_Length) drawingPattern = false;
            else localDepth = 0;
        }
    }

    private void AssignCoinTile(int localDepth, int globalDepth, Array2D currentPattern)
    {
        Vector3Int currentCell;
        Tile currentTile;

        for (int x = 0; x < TileGenerator._diggableAreaX; x++)
        {
            currentCell = new Vector3Int(_startCell.x + x, _startCell.y - globalDepth, _startCell.z);
            if (currentPattern[x, localDepth] == -1) continue; //Value of -1 simply means empty, does not set any tile

            //Current tile is set using its value as an index in collectables[]
            //The offset also adds to the value, e.g. deeper you go -> offset increases -> higher the collectable value
            currentTile = _tileList._collectables[currentPattern[x, localDepth] + _coinValueOffset]._tile;
            _coinMap.SetTile(currentCell, currentTile);
        }
    }

    private int FindRandomCoinPattern() //Chooses a random pattern out of _patternList and returns its index
    {
        float rand = UnityEngine.Random.Range(0.0f, _patternList.Get_Patterns);
        int index = (int)Mathf.Floor(rand);

        //Make sure patterns don't happen twice in a row
        if (index == _prevCoinPattern)
        {
            if (index == 0) index++;
            else index--;
        }
        _prevCoinPattern = index;

        return index;
    }

    public CollectableData GetSelectedCoinData(Vector3Int selectedCell)
    {
        CollectableData selectedCoinData = null;
        TileBase coinTile = _coinMap.GetTile(selectedCell);

        for (int i = 0; i < _tileList._collectables.Count; i++)
        {
            if (_tileList._collectables[i]._tile == coinTile)
            {
                selectedCoinData = _tileList._collectables[i];
            }
        }
        return selectedCoinData;
    }

    //Used to switch out coins/valuables found as depth increases
    public void UpdateCoins(int valueIncrease)
    {
        _coinValueOffset += valueIncrease;
    }
}
