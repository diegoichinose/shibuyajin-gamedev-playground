using DG.Tweening;
using UnityEngine;

public class OnEnableShakeScale : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private Vector3 strength;
    [SerializeField] private int vibrato;
    [SerializeField] private float randomness;
    [SerializeField] private bool fadeOut;
    private Vector3 originalLocalScale;
    private Tweener anim;

    void Awake()
    {
        originalLocalScale = transform.localScale;
    }

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
            anim = transform.DOShakeScale(duration, strength, vibrato, randomness, fadeOut).OnComplete(() => transform.localScale = originalLocalScale);
    }
}