using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DigSystem : MonoBehaviour
{
    TileGenerator _tileGenerator;
    MinigameSlider _miniGameSlider;

    public static bool _digMode = true;
    [SerializeField] int _digPower = 1;
    float _currentTileStrength;
    bool _playingMinigame = false;
    int _winsInARow = 0;

    [SerializeField] Tilemap _digMap;
    [SerializeField] Tilemap _highlightMap;
    [SerializeField] Tile _highlightTile;
    TileData _selectedTileData;
    Vector3Int _selectedCell;
    Vector3Int _previousCell;

    private void Start() 
    {
        _tileGenerator = GameObject.FindGameObjectWithTag("TileGenerator").GetComponent<TileGenerator>();
        _miniGameSlider = GameObject.FindGameObjectWithTag("MinigameSlider").GetComponent<MinigameSlider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_digMode) return;

        _previousCell = _selectedCell;
        _selectedCell = _digMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //_selectedCell.z = 0;

        if (_previousCell != Vector3Int.zero && _previousCell != _selectedCell)
        {
            _highlightMap.SetTile(_previousCell, null);
            _selectedTileData = _tileGenerator.GetSelectedTileData(_selectedCell);

            if (_selectedTileData != null)
            {
                _currentTileStrength = _selectedTileData._tileStrength;
            }
        } 
        _highlightMap.SetTile(_selectedCell, _highlightTile);
    }

    public void Dig()
    {
        //If no tile at selected spot, return
        if (!_selectedTileData) return;
        _playingMinigame = !_playingMinigame; //Flip-flop inputs, One starts the minigame and the other stops and calculates values

        if (_playingMinigame) _miniGameSlider.OnMinigameStart(_selectedTileData._minigameCenterWidth, _winsInARow);
        else 
        {
            float multiplier = _miniGameSlider.OnMinigameStop();
            _currentTileStrength -= (_digPower * multiplier);

            if (_miniGameSlider._successfulHit) _winsInARow++;
            else
            {
                _winsInARow = 0;
            }
        }

        if (_currentTileStrength <= 0) _digMap.SetTile(_selectedCell, null);
    }
}
