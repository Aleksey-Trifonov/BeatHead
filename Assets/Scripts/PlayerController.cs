using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private RectTransform crosshairsRect;

    private Camera mainCamera;

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

    private void LateUpdate()
    {
        if (!GameplayManager.Instance.isSkeetLaunched)
        {
            return;
        }

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            Ray inputRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Quaternion newRotation = Quaternion.LookRotation(inputRay.direction);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
        }
#elif UNITY_ANDROID
        if (Input.touchCount != 0)
        {
            Ray inputRay = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
            Quaternion newRotation = Quaternion.LookRotation(inputRay.direction);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
        }
#endif
    }

    private void OnRoundFinish(bool isWin)
    {
        mainCamera.transform.rotation = Quaternion.identity;
    }
}
