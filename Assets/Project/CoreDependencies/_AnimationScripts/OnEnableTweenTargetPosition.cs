using DG.Tweening;
using UnityEngine;

public class OnEnableTweenTargetPosition : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 initialLocalPosition;
    [SerializeField] private Vector2 finalLocalPosition;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;

    void OnEnable() 
    {
        target.localPosition = initialLocalPosition;

        if (finalLocalPosition.x != initialLocalPosition.x)
            target.DOLocalMoveX(finalLocalPosition.x, duration).SetEase(ease).SetUpdate(true);

        if (finalLocalPosition.y != initialLocalPosition.y)
            target.DOLocalMoveY(finalLocalPosition.y, duration).SetEase(ease).SetUpdate(true);
    }

    void OnDisable()
    {
        transform.DOKill();
    }
}