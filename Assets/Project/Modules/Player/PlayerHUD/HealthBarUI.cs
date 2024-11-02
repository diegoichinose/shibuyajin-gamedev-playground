using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    [SerializeField] private Image damageCircularSectionUI;
    [SerializeField] private Image damageLinearSectionUI;
    [SerializeField] private Gradient damageGradient;

    [SerializeField] private RectTransform parent;
    [SerializeField] private Image outerCircularSectionUI;
    [SerializeField] private Image outerLinearSectionUI;
    [SerializeField] private Image innerCircularSectionUI;
    [SerializeField] private Image innerLinearSectionUI;
    [SerializeField] private Image linearCapUI;
    [SerializeField] private Image portrait;
    [SerializeField] private RectTransform rotatorUI;
    [Range(0,1)] public float healthPercentageCache;

    private  int barLength;
    private float ringDiameter = 200f;
    private float TotalRingCircumference { get { return Mathf.PI * ringDiameter; } }
    private float TotalRinglength { get { return TotalRingCircumference * 0.75f; } }
    private float circlePercentage;    
    private float damageTime = 1f;
    private float damageReceived = 0f;

    void Start()
    { 
        UpdateHealthBarUI();
        _playerData.player.health.OnUpdate += OnPlayerDataUpdate;
        _playerData.player.health.OnMaxHealthUpdate += UpdateHealthBarUI;
        _playerData.player.health.OnCurrentHealthUpdate += UpdateCurrentHealthUI;
    }

    void OnDestroy()
    { 
        _playerData.player.health.OnUpdate -= OnPlayerDataUpdate;
        _playerData.player.health.OnMaxHealthUpdate -= UpdateHealthBarUI;
        _playerData.player.health.OnCurrentHealthUpdate -= UpdateCurrentHealthUI;
    }

    private void OnPlayerDataUpdate()
    {
        UpdateHealthBarUI();
        UpdateCurrentHealthUI();
    }

    private void UpdateCurrentHealthUI()
    {
        if (PauseResumeTimeManager.instance.isPaused)
            return;
            
        if (healthPercentageCache > _playerData.player.health.healthPercentage)
            damageReceived = healthPercentageCache - _playerData.player.health.healthPercentage;

        healthPercentageCache = _playerData.player.health.healthPercentage;

        UpdateCircularSection(healthPercentageCache);
        UpdateLinearSection(healthPercentageCache);
        
        if (damageReceived > 0)
            OnDamageReceived();
    }

    private void UpdateHealthBarUI()
    {
        barLength = (int) _playerData.player.health.maxHealth;
        UpdateCurrentHealthUI();
        ResetDamageIndicators();
    }

    private void ResetDamageIndicators()
    {
        damageLinearSectionUI.rectTransform.sizeDelta = innerLinearSectionUI.rectTransform.sizeDelta;
        damageLinearSectionUI.fillAmount = innerLinearSectionUI.fillAmount;
        damageCircularSectionUI.fillAmount = innerCircularSectionUI.fillAmount;
    }

    private void UpdateCircularSection(float fillPercentage)
    {
        outerCircularSectionUI.fillAmount = Mathf.Lerp(0, 0.75f, (barLength / TotalRinglength));
        circlePercentage = Mathf.Clamp01(outerCircularSectionUI.fillAmount * TotalRingCircumference / barLength);
        
        innerCircularSectionUI.fillAmount = Mathf.Lerp(0f, outerCircularSectionUI.fillAmount, (fillPercentage/circlePercentage));

        float requiredRotation = Mathf.Lerp(0,-360, outerCircularSectionUI.fillAmount);
        rotatorUI.eulerAngles = new Vector3(0,0, requiredRotation);

        if(barLength - TotalRinglength > 0)
            rotatorUI.gameObject.SetActive(false);
        else
            rotatorUI.gameObject.SetActive(true);
    }

    private void UpdateLinearSection(float fillPercentage)
    {
        outerLinearSectionUI.rectTransform.sizeDelta = new Vector2(barLength - TotalRinglength, innerLinearSectionUI.rectTransform.sizeDelta.y);
        innerLinearSectionUI.rectTransform.sizeDelta = new Vector2(barLength - TotalRinglength, outerLinearSectionUI.rectTransform.sizeDelta.y);

        innerLinearSectionUI.fillAmount = Mathf.Lerp(0, 1, ((fillPercentage - circlePercentage) / (1 - circlePercentage)));

        linearCapUI.enabled = outerLinearSectionUI.rectTransform.sizeDelta.x > 0 ? true : false;
    }

    public void OnDamageReceived()
    {
        ShakeHealthBar();

		StartCoroutine(SlowlyResizeDamageBarToCurrentHealth());
		StartCoroutine(SlowlyChangeDamageBarColor());

        damageCircularSectionUI.color = damageGradient.Evaluate(1);
        damageLinearSectionUI.color = damageGradient.Evaluate(1);
        damageReceived = 0f;
    }

	private IEnumerator SlowlyResizeDamageBarToCurrentHealth()
	{
        if (damageLinearSectionUI.fillAmount != 0)
        while (damageLinearSectionUI.fillAmount >= innerLinearSectionUI.fillAmount)
        {
            damageLinearSectionUI.fillAmount -= 0.002f;
		    yield return new WaitForSeconds(0.001f); 
        }

        if (damageCircularSectionUI.fillAmount != 0)
        while (damageCircularSectionUI.fillAmount >= innerCircularSectionUI.fillAmount)
        {
            damageCircularSectionUI.fillAmount -= 0.002f;
		    yield return new WaitForSeconds(0.001f); 
        }
	}

    private IEnumerator SlowlyChangeDamageBarColor()
	{
        float damageTimer = damageTime;
        while (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
            
            if (damageTimer <= 0)
                damageTimer = 0;
                
            damageCircularSectionUI.color = damageGradient.Evaluate(1 - (damageTimer / damageTime));
            damageLinearSectionUI.color = damageGradient.Evaluate(1 - (damageTimer / damageTime));
		    yield return new WaitForSeconds(0.001f); 
        }
	}

	private void ShakeHealthBar() => parent.DOShakePosition(0.3f, new Vector2(20f, 20f), 50, 90, false);
}