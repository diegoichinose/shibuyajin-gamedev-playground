using DG.Tweening;
using UnityEngine;

public class OnHarvestTickAnimateTool : MonoBehaviour
{
    [SerializeField] private Ease ease;
    [SerializeField] private float duration;
    [SerializeField] private Vector3 originalRotation;
    [SerializeField] private Vector3 endRotation;
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private Vector3 endPosition;

    void Start()
    {
        GameEventsManager.instance.OnHarvestTickWithTool += AnimateTool;
    }

    void OnDestroy()
    {
        GameEventsManager.instance.OnHarvestTickWithTool -= AnimateTool;
    }

    void OnEnable()
    {
        transform.SetLocalPositionAndRotation(endPosition, Quaternion.Euler(endRotation));
    }

    void OnDisable()
    {
        transform.DOKill();
    }

    private void AnimateTool()
    {
        transform.DOKill();
        transform.SetLocalPositionAndRotation(originalPosition, Quaternion.Euler(originalRotation));
        transform.DOLocalRotate(endRotation, duration).SetEase(ease);
        transform.DOLocalMove(endPosition, duration).SetEase(ease);
    }
}