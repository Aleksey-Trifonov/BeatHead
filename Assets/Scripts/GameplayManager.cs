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
    private GameObject activeSkeet;

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
    }

    private void OnDisable()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.EventLaunchSkeetClick -= OnLaunchSkeetClick;
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
            else
            {
                if (true)
                {
                    var activeSettings = gameplaySettings.SkeetLifetimeSettings.Find(s => s.MinTime >= remainingSkeetTime && s.MaxTime < remainingSkeetTime);
                    skeetFocusPercent += Time.deltaTime / activeSettings.FocusFillTime;
                }
                else
                {
                    skeetFocusPercent = 0f;
                }

                EventFocusChanged?.Invoke(skeetFocusPercent);



                if (skeetFocusPercent >= 1f)
                {
                    isSkeetLaunched = false;
                    EventRoundFinished?.Invoke(true);
                }
            }
        }
    }

    private void OnLaunchSkeetClick()
    {
        if (UnityEngine.Random.value >= 0.5f)
        {
            activeSkeet = SkeetLauncherOne.LaunchSkeet();
        }
        else
        {
            activeSkeet = SkeetLauncherTwo.LaunchSkeet();
        }

        skeetFocusPercent = 0f;
        remainingSkeetTime = gameplaySettings.SkeetTime;
        isSkeetLaunched = true;
    }
}
