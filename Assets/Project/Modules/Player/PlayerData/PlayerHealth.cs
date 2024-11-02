using System;
using UnityEngine;

[Serializable]
public class PlayerHealth
{
    public float baseMaxHealth;
    public float maxHealth;
    public float currentHealth;
    public float healingPerSecond;
    public float defense;
    public float damageReceivedMultiplier;
    public float additionalHealingPercentageDecimal;
    public float invincibilitySeconds;
    public bool preventKnockback;
    public bool isInvincible;
    public bool isDead => currentHealth == 0;
    public float healthPercentage => currentHealth / maxHealth;
    public void EnableInvinsibility() => isInvincible = true;
    public void DisableInvinsibility() => isInvincible = false;
    public Action OnMaxHealthUpdate;
    public Action OnCurrentHealthUpdate;
    public Action OnUpdate;

    public PlayerHealth()
    {
        baseMaxHealth = 300f;
        maxHealth = baseMaxHealth;
        currentHealth = maxHealth;
        healingPerSecond = 0;
        defense = 0f;
        additionalHealingPercentageDecimal = 0f;
        damageReceivedMultiplier = 1f;
        invincibilitySeconds = 0.8f;
        isInvincible = false;
        preventKnockback = false;
    }

    public void IncrementDefense(int amount) 
    {
        defense += amount;
        OnUpdate?.Invoke();
    }

    public void IncrementMaxHealth(int amount)
    {
        baseMaxHealth += amount;
        maxHealth += amount;
        
        if(currentHealth > maxHealth)
            currentHealth = maxHealth;

        OnUpdate?.Invoke();
        OnMaxHealthUpdate?.Invoke();
    }

    public void Heal(float amount)
    {
        var amountWithModifiers = amount + (amount * additionalHealingPercentageDecimal);
        currentHealth += amountWithModifiers;

        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
            
        OnUpdate?.Invoke();
        OnCurrentHealthUpdate?.Invoke();
    }
    
    public void TakeDamage(float incomingDamage)
    {
		if (isDead)
			return;
        
	    currentHealth -= (incomingDamage - defense) * damageReceivedMultiplier;
		
	    if (currentHealth < 0.009f)
	        currentHealth = 0;

		OnUpdate?.Invoke();
        OnCurrentHealthUpdate?.Invoke();
    }
}