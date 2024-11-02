using DG.Tweening;
using UnityEngine;

public class OnEnableTweenRotation : MonoBehaviour
{
    [SerializeField] private Vector3 originalRotation;
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    [SerializeField] private bool loop;
    [SerializeField] private float delay;
    private Tweener tweener;

    void OnEnable() 
    {
        PlayAnimation();
    }

    void OnDisable()
    {
        transform.DOKill();
    }

    public void PlayAnimation()
    {
        if (tweener != null && tweener.IsActive())
            return;

        transform.rotation = Quaternion.Euler(originalRotation);
        tweener = transform.DORotate(targetRotation, duration, RotateMode.FastBeyond360).SetEase(ease).SetDelay(delay).SetUpdate(true);

        if (loop)
            tweener.SetLoops(-1, LoopType.Incremental);
    }
}