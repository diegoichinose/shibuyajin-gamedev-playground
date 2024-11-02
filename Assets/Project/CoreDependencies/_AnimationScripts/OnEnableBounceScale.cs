using DG.Tweening;
using UnityEngine;

public class OnEnableBounceScale : MonoBehaviour
{
    private Tweener anim;

    void OnEnable()
    {
        PlayBouncingAnimation();
    }

    void OnDisable()
    {
        transform.DOKill();
    }

    private void PlayBouncingAnimation()
    {
        if (anim == null || !anim.IsActive())
            anim = transform.DOShakeScale(0.2f, new Vector3(1f, 1f, 0), 10, 90, fadeOut: true);
    }
}