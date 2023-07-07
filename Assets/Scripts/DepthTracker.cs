using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DepthTracker : MonoBehaviour
{
    Slider _depthSlider;
    Transform _playerTrans;

    [SerializeField] float _verticalRoof = 5.0f;
    [SerializeField] float _verticalFloor = -200.0f;
    float _totalDepth;
    float _furthestReached;

    private void Awake() 
    {
        _depthSlider = GetComponent<Slider>();
        _totalDepth = (_verticalFloor - _verticalRoof) * -1;
    }

    private void Start() 
    {
        _playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() 
    {
        UpdateDepth();
    }

    private void UpdateDepth()
    {
        float depth;

        if (_playerTrans.position.y > _verticalRoof)
        {
            _depthSlider.value = 0;
        }
        else if (_playerTrans.position.y < _verticalFloor)
        {
            _depthSlider.value = 1;
        }
        else
        {
            //Depth calculated by distance from verticalRoof, adding VertRoof to yPos if y < 0 and subtracting ypos from Vertroof if not
            depth = _playerTrans.position.y < 0 ? -_playerTrans.position.y + _verticalRoof : _verticalRoof - _playerTrans.position.y;

            _depthSlider.value = depth / _totalDepth;
        }
    }
}
