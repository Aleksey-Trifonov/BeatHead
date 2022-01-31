using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;

    public event Action<bool> EventSkeetInFocus;

    private Camera mainCamera;

    public static PlayerController Instace
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerController>();
            }
            return instance;
        }
    }

    private static PlayerController instance = null;

    private void OnEnable()
    {
        GameplayManager.Instance.EventRoundFinished += OnRoundFinish;
    }

    private void OnDisable()
    {
        if (GameplayManager.Instance != null)
        {
            GameplayManager.Instance.EventRoundFinished -= OnRoundFinish;
        }
    }

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!GameplayManager.Instance.isSkeetLaunched)
        {
            return;
        }

        EventSkeetInFocus?.Invoke(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, 100f));
    }

    private void LateUpdate()
    {
        if (!GameplayManager.Instance.isSkeetLaunched)
        {
            return;
        }

#if UNITY_EDITOR
        var newDirection = mainCamera.transform.forward + new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
        Quaternion newRotation = Quaternion.LookRotation(newDirection);
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount != 0)
        {
            var touch = Input.GetTouch(0);
            var newDirection = mainCamera.transform.forward + new Vector3(touch.deltaPosition.x, touch.deltaPosition.y, 0);
            Quaternion newRotation = Quaternion.LookRotation(newDirection);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
        }
#endif
    }

    private void OnRoundFinish(bool isWin)
    {
        mainCamera.transform.rotation = Quaternion.identity;
    }
}
