using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OnEnableFadeInImage : MonoBehaviour
{
    [SerializeField] private Image _target;
    [SerializeField] private float startingAlpha;
    [SerializeField] private float targetAlpha;
    [SerializeField] private float duration;

    void OnEnable()
    {
        if (_target == null)
            return;
            
        var tempColor = _target.color;
        tempColor.a = startingAlpha;
        _target.color = tempColor;

        _target.DOFade(targetAlpha, duration).SetUpdate(true);
    }

    void OnDisable()
    {
        if(_target != null)
            _target.DOKill();
    }
}
