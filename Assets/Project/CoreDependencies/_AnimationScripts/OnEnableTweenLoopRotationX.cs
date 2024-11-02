using DG.Tweening;
using UnityEngine;

public class OnEnableTweenLoopRotationX : MonoBehaviour
{
    [SerializeField] private Vector2 targetRotation;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;

    void OnEnable()
    {    
        transform.DOLocalRotate(targetRotation, duration, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(ease);
    }

    void OnDisable()
    {
        transform.DOKill();
    }
}