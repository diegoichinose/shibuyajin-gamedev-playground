using DG.Tweening;
using UnityEngine;

public class OnEnableLoopScale : MonoBehaviour
{
    [SerializeField] private Vector3 originalScale;
    [SerializeField] private Vector3 targetScale;
    [SerializeField] private float duration;    
    [SerializeField] private Ease ease;

    void OnEnable()
    {    
        transform.localScale = originalScale;

        transform
            .DOScale(targetScale, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(ease)
            .SetUpdate(true);
    }

    void OnDisable()
    {
        transform.localScale = targetScale;
        transform.DOKill();
    }
}
