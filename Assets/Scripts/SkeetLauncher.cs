using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkeetLauncher : MonoBehaviour
{
    [SerializeField] private Transform launchPoint;
    [SerializeField] private GameObject skeetPrefab;

    private GameplaySettingsSO gameplaySettings;
    private GameObject skeetGo;
    private Rigidbody skeetRb;
    private TrailRenderer skeetTr;

    private void OnEnable()
    {
        GameplayManager.Instance.EventRoundFinished += OnRoundFinished;
    }

    private void OnDisable()
    {
        if (GameplayManager.Instance != null)
        {
            GameplayManager.Instance.EventRoundFinished -= OnRoundFinished;
        }
    }

    private void Start()
    {
        gameplaySettings = Resources.Load<GameplaySettingsSO>("GameplaySettings");
        skeetGo = Instantiate(skeetPrefab, launchPoint.position, launchPoint.rotation);
        skeetRb = skeetGo.GetComponent<Rigidbody>();
        skeetRb.isKinematic = true;
        skeetTr = skeetGo.GetComponent<TrailRenderer>();
    }

    private void OnRoundFinished(bool isWin)
    {
        skeetTr.enabled = false;
        skeetRb.isKinematic = true;
        skeetRb.velocity = Vector3.zero;
        skeetRb.angularVelocity = Vector3.zero;
        skeetGo.transform.SetPositionAndRotation(launchPoint.position, launchPoint.rotation);
    }

    public GameObject LaunchSkeet()
    {
        skeetTr.enabled = true;
        skeetRb.isKinematic = false;
        Vector3 forceDirection = gameplaySettings.LaunchForce * skeetGo.transform.forward;
        skeetRb.AddForceAtPosition(forceDirection, skeetGo.transform.position + skeetGo.transform.forward * 10, ForceMode.Impulse);
        return skeetGo;
    }
}
