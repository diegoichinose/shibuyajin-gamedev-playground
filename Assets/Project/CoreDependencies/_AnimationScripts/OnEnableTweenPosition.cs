using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class OnEnableTweenPosition : MonoBehaviour
{
    [SerializeField] private Vector2 initialLocalPosition;
    [SerializeField] private Vector2 targetLocalPosition;
    [SerializeField] private float duration;
    [SerializeField] private float delay;
    [SerializeField] private Ease ease;
    [SerializeField] private bool playSound;

    void OnEnable() 
    {
        transform.localPosition = initialLocalPosition;

        if (targetLocalPosition.x != initialLocalPosition.x)
            transform.DOLocalMoveX(targetLocalPosition.x, duration).SetDelay(delay).SetEase(ease).SetUpdate(true)
                     .OnStart(() => 
                     {
                        if (playSound)
                            AudioManager.instance.sfxLibrary.PlayGenericTweenPositionSound();
                     });

        if (targetLocalPosition.y != initialLocalPosition.y)
            transform.DOLocalMoveY(targetLocalPosition.y, duration).SetDelay(delay).SetEase(ease).SetUpdate(true)
                     .OnStart(() => 
                     {
                        if (playSound)
                            AudioManager.instance.sfxLibrary.PlayGenericTweenPositionSound();
                     });
    }

    void OnDisable()
    {
        StopAllCoroutines();
        transform.DOKill();
    }

    public void PlayReverse(float durationOverride = 0, Action onComplete = null)
    {
        var durationToUse = duration;

        if (durationOverride > 0)
            durationToUse = durationOverride;

        transform.localPosition = targetLocalPosition;
    
        if (targetLocalPosition.x != initialLocalPosition.x)
            transform.DOLocalMoveX(initialLocalPosition.x, durationToUse).SetDelay(delay).SetEase(ease).SetUpdate(true);

        if (targetLocalPosition.y != initialLocalPosition.y)
            transform.DOLocalMoveY(initialLocalPosition.y, durationToUse).SetDelay(delay).SetEase(ease).SetUpdate(true);

        if (onComplete != null)
            StartCoroutine(InvokeActionAfterTime(onComplete, durationToUse));
    }

    private IEnumerator InvokeActionAfterTime(Action action, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        action?.Invoke();
    }
}