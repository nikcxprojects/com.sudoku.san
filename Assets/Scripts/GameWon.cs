using UnityEngine;

public class GameWon : MonoBehaviour
{
    [SerializeField] private GameObject _winPopup;

    private void Start()
    {
        _winPopup.SetActive(false);
    }

    private void OnBoardCompleted()
    {
        _winPopup.SetActive(true);
    }

    private void OnEnable()
    {
        GameEvents.OnBoardCompleted += OnBoardCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= OnBoardCompleted;
    }
}
