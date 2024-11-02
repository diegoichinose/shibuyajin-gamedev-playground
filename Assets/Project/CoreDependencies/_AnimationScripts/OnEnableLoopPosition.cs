using DG.Tweening;
using UnityEngine;

public class OnEnableLoopPositionToFrom : MonoBehaviour
{
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;

    void OnEnable()
    {    
        transform.localPosition = originalPosition;

        transform
            .DOLocalMove(targetPosition, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(ease)
            .SetUpdate(true);
    }

    void OnDisable()
    {
        transform.localPosition = originalPosition;
        transform.DOKill();
    }
}
