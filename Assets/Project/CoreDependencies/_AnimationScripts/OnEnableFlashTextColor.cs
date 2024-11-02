using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class OnEnableFlashTextColor : MonoBehaviour
{
    [SerializeField] private Color flashColor;
    [SerializeField] private float duration;
    private Color initialColor;
    private Coroutine coroutine = null;

    void Awake()
    {
        initialColor = GetComponent<TMP_Text>().color;
    }

    void OnEnable() 
    {
        if (coroutine != null)
            return;    

        var text = GetComponent<TMP_Text>();
        text.color = flashColor;

        coroutine = StartCoroutine(WaitAndResetTextColor());
    }

    void OnDisable()
    {   
        StopAllCoroutines();
        coroutine = null;
    }

	private IEnumerator WaitAndResetTextColor() 
	{
		yield return new WaitForSecondsRealtime(duration); 
        GetComponent<TMP_Text>().color = initialColor;    
        coroutine = null;
    }
}