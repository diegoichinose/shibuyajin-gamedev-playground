using System.Linq;
using DG.Tweening;
using UnityEngine;

public class OnEnableTweenPositionRandomly : MonoBehaviour
{
    [SerializeField] private Vector2 initialLocalPosition;
    [SerializeField] private float[] targetXPositionVariants;
    [SerializeField] private float[] targetYPositionVariants;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;

    void OnEnable() 
    {
        transform.localPosition = initialLocalPosition;

        int x = Random.Range(0, targetXPositionVariants.Count());
        int y = Random.Range(0, targetYPositionVariants.Count());

        if (targetXPositionVariants[x] != initialLocalPosition.x)
            transform.DOLocalMoveX(targetXPositionVariants[x], duration).SetEase(ease).SetUpdate(true).OnComplete(() => transform.localPosition = initialLocalPosition);

        if (targetYPositionVariants[y] != initialLocalPosition.y)
            transform.DOLocalMoveY(targetYPositionVariants[y], duration).SetEase(ease).SetUpdate(true).OnComplete(() => transform.localPosition = initialLocalPosition);
    }

    void OnDisable()
    {
        transform.DOKill();
    }
}