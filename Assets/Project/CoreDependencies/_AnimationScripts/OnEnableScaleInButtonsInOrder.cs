using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnEnableScaleInButtonsInOrder : MonoBehaviour
{
    [SerializeField] private Transform[] _gameObjectsToScaleInOrder;
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private Vector2 targetScale;
    [SerializeField] private Ease ease;
    [SerializeField] private bool selectFirstButtonOnEnable;
    public UnityEvent AudioToPlayForEachButton;
    

    void OnEnable()
    {
        ScaleIn();

        if(selectFirstButtonOnEnable)
            StartCoroutine(SelectFirstButtonAfterDelay());
    }

    void OnDisable()
    {   
        KillAllTweens();
        StopAllCoroutines();
    }

    private void ScaleIn()
    {
        for (int i = 0; i < _gameObjectsToScaleInOrder.Length; i++)
        {
            _gameObjectsToScaleInOrder[i].localScale = Vector2.zero;
            _gameObjectsToScaleInOrder[i].DOScale(targetScale, duration).SetEase(ease).SetDelay(i * 0.1f).SetUpdate(true).OnComplete(() => AudioToPlayForEachButton?.Invoke());
        }
    }

    private void KillAllTweens()
    {
        for (int i = 0; i < _gameObjectsToScaleInOrder.Length; i++)
        {
            _gameObjectsToScaleInOrder[i].DOKill();
        }
    }

    private IEnumerator SelectFirstButtonAfterDelay() 
    {
        yield return new WaitForSecondsRealtime(0.2f);     
        EventSystem.current.SetSelectedGameObject(null);   
        
        var firstButton = _gameObjectsToScaleInOrder.FirstOrDefault(x => x.gameObject.activeSelf);
        if (firstButton != null)
            firstButton.GetComponent<Button>().Select();
    }
}