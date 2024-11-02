using DG.Tweening;
using UnityEngine;

public class OnEnableLoopRotation : MonoBehaviour
{
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    private Quaternion originalLocalRotation;

    void OnEnable()
    {    
        originalLocalRotation = transform.localRotation;

        transform
            .DOLocalRotate(targetRotation, duration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(ease)
            .SetUpdate(true);
    }

    void OnDisable()
    {
        transform.localRotation = originalLocalRotation;
        transform.DOKill();
    }
}
