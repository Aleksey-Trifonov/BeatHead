using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SkeetFocusSettings
{
    public float MinTime;
    public float MaxTime;
    public float FocusFillTime;
}

[CreateAssetMenu(fileName = "GameplaySettings", menuName = "ScriptableObjects/CreateGameplaySettingsScriptableObject", order = 1)]
public class GameplaySettingsSO : ScriptableObject
{
    public float SkeetTime = 5f;
    public float LaunchForce = 20f;
    public List<SkeetFocusSettings> SkeetLifetimeSettings;
}
