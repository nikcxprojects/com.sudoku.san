using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [Header("Settimgs References")]
    [SerializeField] private Image[] _difficulty;
    [SerializeField] private Image[] _hints;
    [SerializeField] private Image[] _times;

    [SerializeField] private Color _selectedColor; // 87F882

    private void Start()
    {
        LoaderSettigns();
    }

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoaderSettigns()
    {
        if (!PlayerPrefs.HasKey(PrefsKey.difficulity))
            PlayerPrefs.SetString(PrefsKey.difficulity, "Easy");

        if (!PlayerPrefs.HasKey(PrefsKey.hints))
            PlayerPrefs.SetInt(PrefsKey.hints, 9);

        if (!PlayerPrefs.HasKey(PrefsKey.timelimit))
            PlayerPrefs.SetInt(PrefsKey.timelimit, 0);

        switch (PlayerPrefs.GetString(PrefsKey.difficulity))
        {
            case "Easy":
                _difficulty[0].color = _selectedColor;
                _difficulty[1].color = Color.white;
                _difficulty[2].color = Color.white;
                break;
            case "Medium":
                _difficulty[0].color = Color.white;
                _difficulty[1].color = _selectedColor;
                _difficulty[2].color = Color.white;
                break;
            case "Hard":
                _difficulty[0].color = Color.white;
                _difficulty[1].color = Color.white;
                _difficulty[2].color = _selectedColor;
                break;
        }

        switch (PlayerPrefs.GetInt(PrefsKey.hints))
        {
            case 9:
                _hints[0].color = _selectedColor;
                _hints[1].color = Color.white;
                _hints[2].color = Color.white;
                break;
            case 5:
                _hints[0].color = Color.white;
                _hints[1].color = _selectedColor;
                _hints[2].color = Color.white;
                break;
            case 3:
                _hints[0].color = Color.white;
                _hints[1].color = Color.white;
                _hints[2].color = _selectedColor;
                break;
        }

        switch (PlayerPrefs.GetInt(PrefsKey.timelimit))
        {
            case 0:
                _times[0].color = _selectedColor;
                _times[1].color = Color.white;
                _times[2].color = Color.white;
                break;
            case 10:
                _times[0].color = Color.white;
                _times[1].color = _selectedColor;
                _times[2].color = Color.white;
                break;
            case 5:
                _times[0].color = Color.white;
                _times[1].color = Color.white;
                _times[2].color = _selectedColor;
                break;
        }
    }

    public void DifficultyChange(string difficulty)
    {
        PlayerPrefs.SetString(PrefsKey.difficulity, difficulty);
        LoaderSettigns();
    }

    public void HintsChange(int hint)
    {
        PlayerPrefs.SetInt(PrefsKey.hints, hint);
        LoaderSettigns();
    }

    public void TimeChange(int time)
    {
        PlayerPrefs.SetInt(PrefsKey.timelimit, time);
        LoaderSettigns();
    }
}
