using System;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    private int _minutes = 0;

    private bool _isLimited = false;
    private Text _clockText;
    private float _deltaTime;
    private bool _isEnable = true;

    private void Awake()
    {
        _clockText = gameObject.GetComponent<Text>();
        _deltaTime = 0.0f;
        if (PlayerPrefs.GetInt(PrefsKey.timelimit) == 0)
            _isLimited = false;
        else
        {
            _isLimited = true;
            _minutes = PlayerPrefs.GetInt(PrefsKey.timelimit);
        }

    }

    private void Update()
    {
        if (!_isEnable) return;

        _deltaTime += Time.deltaTime;
        TimeSpan span = TimeSpan.FromSeconds(_deltaTime);
        string hour = LeadingZero(span.Hours);
        string minute = LeadingZero(span.Minutes);
        string seconds = LeadingZero(span.Seconds);

        _clockText.text = hour + ":" + minute + ":" + seconds;
        if (_isLimited && _minutes == span.Minutes)
        {
            Lives.instance.EndGameTime();
            _isEnable = false;
        }    
    }

    private string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    public void OnGameOver()
    {
        _isEnable = false;
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= OnGameOver;    
    }
}
