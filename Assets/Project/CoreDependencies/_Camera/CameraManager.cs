using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private SettingsData _settingsData;
    public static CameraManager instance = null;
    private Tweener cameraAnimation;
    private Camera thisCamera;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        thisCamera = GetComponent<Camera>();
    }

    public void ShakeCamera(float duration, Vector3 strength, int vibrato, int randomness, bool fadeOut, float delay)
    {
        if (_settingsData.settings.disableScreenshake)
            return;

        if (cameraAnimation == null || !cameraAnimation.IsActive())
            cameraAnimation = gameObject.transform.DOShakePosition(duration, strength, vibrato, randomness, fadeOut).SetDelay(delay).SetUpdate(true);
    }

    public void ShakeCameraWeak(float delay = 0) => ShakeCamera(0.1f, new Vector3(0.01f, 0.01f, 0), 20, 90, false, delay);
    public void ShakeCameraModerate(float delay = 0) => ShakeCamera(0.3f, new Vector3(0.04f, 0.04f, 0), 25, 90, false, delay);
    public void ShakeCameraStrong(float delay = 0) => ShakeCamera(0.3f, new Vector3(0.05f, 0.05f, 0), 50, 90, false, delay);
    public Vector3 GetScreenToWorldPoint(Vector3 position) => thisCamera.ScreenToWorldPoint(position);
}
