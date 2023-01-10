using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    public static Lives instance { get; private set; }

    [SerializeField] private GameObject _gameOverWindow;
    [SerializeField] private int _lives = 0;
    [SerializeField] private Text _livesText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    private void Start() => _lives = 10;
    public void EndGameTime()
    {
        _lives = 0;
        CheckForGameOver();
    }

    private void WrongNumber()
    {
        _lives -= 1;
        _livesText.text = $"{_lives} mistakes left";
        CheckForGameOver();
    }

    private void CheckForGameOver()
    {
        if (_lives <= 0)
        {
            GameEvents.OnGameOverMethod();
            _gameOverWindow.SetActive(true);
        }

    }

    private void OnEnable()
    {
        GameEvents.OnWrongNumber += WrongNumber;   
    }

    private void OnDisable()
    {
        GameEvents.OnWrongNumber -= WrongNumber;
    }
}
