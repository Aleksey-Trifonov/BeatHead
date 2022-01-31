using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private SkeetLauncher SkeetLauncherOne;
    [SerializeField] private SkeetLauncher SkeetLauncherTwo;

    public event Action<bool> EventRoundFinished;
    public event Action<float> EventFocusChanged;
    public bool isSkeetLaunched { get; private set; }

    private GameplaySettingsSO gameplaySettings;
    private float remainingSkeetTime = 0f;
    private float skeetFocusPercent = 0f;

    public static GameplayManager Instance 
    {   get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameplayManager>();
            }
            return instance;
        }
    }

    private static GameplayManager instance = null;

    private void OnEnable()
    {
        UIManager.Instance.EventLaunchSkeetClick += OnLaunchSkeetClick;
        PlayerController.Instace.EventSkeetInFocus += OnSkeetInFocus;
    }

    private void OnDisable()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.EventLaunchSkeetClick -= OnLaunchSkeetClick;
        }

        if (PlayerController.Instace != null)
        {
            PlayerController.Instace.EventSkeetInFocus -= OnSkeetInFocus;
        }
    }

    private void Start()
    {
        gameplaySettings = Resources.Load<GameplaySettingsSO>("GameplaySettings");
    }

    private void Update()
    {
        if (isSkeetLaunched)
        {
            remainingSkeetTime -= Time.deltaTime;

            if (remainingSkeetTime <= 0f)
            {
                isSkeetLaunched = false;
                EventRoundFinished?.Invoke(Random.value <= skeetFocusPercent);
            }
        }
    }

    private void OnLaunchSkeetClick()
    {
        if (Random.value >= 0.5f)
        {
            SkeetLauncherOne.LaunchSkeet();
        }
        else
        {
            SkeetLauncherTwo.LaunchSkeet();
        }

        skeetFocusPercent = 0f;
        remainingSkeetTime = gameplaySettings.SkeetTime;
        isSkeetLaunched = true;
    }

    private void OnSkeetInFocus(bool isInFocus)
    {
        var activeSettings = gameplaySettings.SkeetLifetimeSettings.Find(s => s.MinTime >= remainingSkeetTime && s.MaxTime < remainingSkeetTime);
        if (isInFocus)
        {
            skeetFocusPercent += Time.deltaTime / activeSettings.FocusFillTime;
        }
        else
        {
            skeetFocusPercent -= Time.deltaTime / activeSettings.FocusFillTime;
            skeetFocusPercent = Mathf.Clamp(skeetFocusPercent, 0f, 1f);
        }

        EventFocusChanged?.Invoke(skeetFocusPercent);

        if (skeetFocusPercent >= 1f)
        {
            isSkeetLaunched = false;
            EventRoundFinished?.Invoke(true);
        }
    }
}
