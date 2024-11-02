using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OnEnableBounceYSkippable : MonoBehaviour
{
    [SerializeField] private float bounceHeight;
    [SerializeField] private  float durationBeforeTouchingGround = 0.2f;
    [SerializeField] private  UnityEvent onComplete;
    private MyInputActions _input;
    private float originalLocalPositionY; 
    private bool canSkip;

    void Awake() => originalLocalPositionY = transform.localPosition.y;

    void OnEnable()
    {
        _input = new MyInputActions();
        _input.Enable();
        _input.UserIntefaceActionMap.RightClick.performed += Skip;

        canSkip = true;
        PlayAnimation();
    }
    
    void OnDisable()
    {
        _input.Disable();
        _input.UserIntefaceActionMap.RightClick.performed -= Skip;

        transform.DOKill();
        StopAllCoroutines();
    }

    public void PlayAnimation()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, originalLocalPositionY, transform.localPosition.z);
        
        transform.DOLocalMoveY(originalLocalPositionY + bounceHeight, durationBeforeTouchingGround)
                 .SetEase(Ease.OutExpo)
                 .SetUpdate(true)
                 .OnComplete(() => 
                    transform.DOLocalMoveY(originalLocalPositionY, 0.5f)
                             .SetEase(Ease.OutBounce)
                             .SetUpdate(true)
                             .OnComplete(() => onComplete?.Invoke()));
    }
    
    private void Skip(InputAction.CallbackContext context)
    {
        if (canSkip == false)
            return;
        
        canSkip = false;
        transform.DOKill();
        StopAllCoroutines();
        
        transform.localPosition = new Vector3(transform.localPosition.x, originalLocalPositionY, transform.localPosition.z);
        onComplete?.Invoke();
    }
}