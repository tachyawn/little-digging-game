using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameSlider : MonoBehaviour
{
    Slider _minigameSlider;
    [SerializeField] GameObject _sliderComponents;
    [SerializeField] Image _sliderCenter;

    public bool _successfulHit = false;
    [SerializeField] bool _minigameIsActive = false;
    [SerializeField] float _frequency = 2f;
    [SerializeField] float _totalFrames = 32f;
    float _currentFrame = 0f;
    float _frameRatio;
    float _xValue;
    float _sineValue;
    float _successMin;
    float _successMax;
    float _midSuccessDist;

    private void Awake() 
    {
        _minigameSlider = GetComponent<Slider>();
        _sliderComponents.SetActive(false);
    }

    public void OnMinigameStart(float centerWidth, int speed) //Pass in values between 0 and 1
    {
        _sliderComponents.SetActive(true);
        //On successive wins, the speed of the slider increases to a max
        if (speed >= 2) speed = 2;
        _frequency = 2 + (1 * speed);

        float width = this.GetComponent<RectTransform>().rect.width;
        if (centerWidth > 1) centerWidth = 1;
        else if (centerWidth < 0) centerWidth = 0.1f;

        _successMin = 0.5f - (centerWidth / 2);
        _successMax = 0.5f + (centerWidth / 2);
        _sliderCenter.rectTransform.localScale = new Vector3(centerWidth, 1f, 1f);

        _successfulHit = false;
        _minigameIsActive = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_minigameIsActive) return;

        //Slider moves up and down like a sine wave:
        if (_currentFrame == _totalFrames) _currentFrame = 0f;

        _frameRatio = _currentFrame/_totalFrames; //Lerp percent for updating position along sine wave
        _xValue = Mathf.Lerp(0f, 2.0f * Mathf.PI, _frameRatio);

        //sine wave formula restricted to 0-1 for slider value
        _sineValue = 0.5f * Mathf.Sin(_xValue * _frequency) + 0.5f;

        _minigameSlider.value = _sineValue;
        _currentFrame++;
    }

    public float OnMinigameStop()
    {
        _minigameIsActive = false;
        float _digMultiplier = 0.5f;

        if (_minigameSlider.value == 0.5f) //Double Damage for perfect hits
        {
            _digMultiplier = 2.0f;
            _successfulHit = true;
        }
        else if (_minigameSlider.value > _successMin && _minigameSlider.value < _successMax)
        {
            _digMultiplier = 1.5f;
            _successfulHit = true;
        }
        else if (_successMin - _minigameSlider.value == _midSuccessDist || _minigameSlider.value - _successMax == _midSuccessDist)
        {
            _digMultiplier = 1.0f;
        }

        _sliderComponents.SetActive(false);
        return _digMultiplier;
    }
}
