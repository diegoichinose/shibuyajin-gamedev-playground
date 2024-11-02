using System.Collections;
using DG.Tweening;
using UnityEngine;

public class OnEnableDestroySelfAfterTime : MonoBehaviour
{
    [SerializeField] private float selfDestructionTimer;

    void OnEnable()
    {
        StartCoroutine(DestroySelfAfterTime());
    } 

    void OnDisable()
    {
        transform.DOKill();
        StopAllCoroutines();
        Destroy(gameObject);
    }

	private IEnumerator DestroySelfAfterTime() 
	{
        while (true)
        {
            yield return new WaitForSeconds(selfDestructionTimer); 
            
            if (PauseResumeTimeManager.instance.isPaused)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            Destroy(gameObject);
        }
    }
}