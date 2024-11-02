using DG.Tweening;
using UnityEngine;

public class OnEnableTweenLocalRotation : MonoBehaviour
{
    [SerializeField] private Quaternion originalRotation;
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    [SerializeField] private bool loop;
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

        transform.localRotation = originalRotation;
        tweener = transform.DOLocalRotate(targetRotation, duration, RotateMode.FastBeyond360).SetEase(ease).SetUpdate(true);

        if (loop)
            tweener.SetLoops(-1, LoopType.Incremental);
    }
}