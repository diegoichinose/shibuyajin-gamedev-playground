using UnityEngine;
using DG.Tweening;

public class OnEnableFadeIn : MonoBehaviour
{
    private Vector3 originalLocalPosition;
    private Vector3 originalScale;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private float delay;
    
    void Awake()
    {
        originalLocalPosition = transform.localPosition;
        originalScale = transform.localScale;
    }

    void OnEnable()
    {
        transform.localPosition = Vector3.zero; 
        transform.localScale = Vector3.zero; 

        transform.DOScale(originalScale, animationDuration).SetEase(Ease.InOutBack).SetUpdate(true).SetDelay(delay);
        transform.DOLocalMove(originalLocalPosition, animationDuration).SetEase(Ease.OutBounce).SetUpdate(true).SetDelay(delay);
    }
    
    void OnDisable()
    {
        transform.DOKill();
    }
}