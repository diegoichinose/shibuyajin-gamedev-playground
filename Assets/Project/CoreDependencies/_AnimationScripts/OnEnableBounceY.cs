using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableBounceY : MonoBehaviour
{
    private float originalLocalPositionY; 
    [SerializeField] private float bounceHeight;
    [SerializeField] private  float durationBeforeTouchingGround = 0.2f;
    [SerializeField] private float delay;
    [SerializeField] private  UnityEvent onComplete;

    void Awake() => originalLocalPositionY = transform.localPosition.y;

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
        transform.localPosition = new Vector3(transform.localPosition.x, originalLocalPositionY, transform.localPosition.z);
        
        transform.DOLocalMoveY(originalLocalPositionY + bounceHeight, durationBeforeTouchingGround)
                 .SetEase(Ease.OutExpo)
                 .SetUpdate(true)
                 .SetDelay(delay)
                 .OnComplete(() => 
                    transform.DOLocalMoveY(originalLocalPositionY, 0.5f)
                             .SetEase(Ease.OutBounce)
                             .SetUpdate(true)
                             .OnComplete(() => onComplete?.Invoke()));
    }
}