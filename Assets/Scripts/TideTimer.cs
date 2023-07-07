using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public class TideTimer : MonoBehaviour
{
    public UnityEvent _OnTimerEnd;

    [SerializeField] TMP_Text _timerText;
    [SerializeField] float _timerValue = 180;
    bool _timerIsActive = true;
    float _startTime;

    // Update is called once per frame
    void Update()
    {
        if (_timerIsActive) UpdateTimer();
    }

    public void StartTimer()
    {
        _startTime = Time.time;
        _timerIsActive = true;
    }
    public void StopTimer()
    {
        _timerIsActive = false;
    }

    private void UpdateTimer()
    {
        float timerNum = _timerValue - (Time.time - _startTime);
        if (timerNum < 0f)
        {
            timerNum = 0f;
            EndTimer();
        } 
        
        _timerText.text = $"Tide Coming in: {FormatNum(timerNum, true)}:{FormatNum(timerNum, false)}";
    }

    private void EndTimer()
    {
        _timerIsActive = false;
        _OnTimerEnd.Invoke();
    }

    private string FormatNum(float num, bool minutes)
    {
        string temp;
        if (minutes) temp = Mathf.FloorToInt(num / 60).ToString();
        else 
        {
            num = Mathf.FloorToInt(num % 60);
            
            if (num < 10) temp = $"0{num}";
            else temp = num.ToString();
        }

        return temp;
    }
}
