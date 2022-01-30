using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hitCounterText;
    [SerializeField] private Button launchButton;
    [SerializeField] private Image focusImage;
    [SerializeField] private RectTransform crosshairsRect;

    public event Action EventLaunchSkeetClick;

    private Camera mainCamera;
    private Vector2 crosshairsScreenPos;
    private float crosshairsWidth;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    private static UIManager instance = null;

    private int hitCounter = 0;

    private void OnEnable()
    {
        GameplayManager.Instance.EventFocusChanged += OnFocusChanged;
        GameplayManager.Instance.EventRoundFinished += OnRoundFinished;
        launchButton.onClick.AddListener(OnLaunchClick);
    }

    private void OnDisable()
    {
        if (GameplayManager.Instance != null)
        {
            GameplayManager.Instance.EventFocusChanged -= OnFocusChanged;
            GameplayManager.Instance.EventRoundFinished -= OnRoundFinished;
        }
        
        launchButton.onClick.RemoveListener(OnLaunchClick);
    }

    private void Start()
    {
        mainCamera = Camera.main;
        crosshairsScreenPos = mainCamera.WorldToScreenPoint(crosshairsRect.transform.position);
        focusImage.fillAmount = 0f;
        crosshairsWidth = crosshairsRect.rect.width / 2f;
    }

    private void OnLaunchClick()
    {
        launchButton.gameObject.SetActive(false);
        EventLaunchSkeetClick?.Invoke();
    }

    private void OnRoundFinished(bool isWin)
    {
        launchButton.gameObject.SetActive(true);
        focusImage.fillAmount = 0f;
        hitCounter = isWin ? ++hitCounter : 0;
        hitCounterText.text = hitCounter.ToString();
    }

    private void OnFocusChanged(float focusValue)
    {
        focusImage.fillAmount = focusValue;
    }

    public bool IsSkeetInCrosshairs(GameObject skeetGo)
    {
        var skeetScreenPos = mainCamera.WorldToScreenPoint(skeetGo.transform.position);
        var distance = Vector2.Distance(skeetScreenPos, crosshairsScreenPos);
        Debug.Log(distance);
        if (distance < crosshairsWidth)
        {
            return true;
        }
        return false;
    }
}
