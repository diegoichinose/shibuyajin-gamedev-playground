using DG.Tweening;
using UnityEngine;

public class OnEnableShakePosition : MonoBehaviour
{
    private Tweener anim;
    private Vector3 targetOriginalPosition;
    public Transform target;
    public float duration = 0.3f;
    public Vector3 strength = new Vector3(0.05f, 0.05f, 0);
    public int  vibrato = 50;
    public int  randomness = 90;
    public bool fadeOut = false;

    void Awake()
    {
        targetOriginalPosition = target.localPosition;
    }

    void OnEnable()
    {
        target.localPosition = targetOriginalPosition;

        if (anim == null || !anim.IsActive())
            anim = target.DOShakePosition(duration, strength, vibrato, randomness, fadeOut).SetUpdate(true);
    }

    void OnDisable()
    {
        anim?.Kill();
        DOTween.Kill(target);
    }
}
