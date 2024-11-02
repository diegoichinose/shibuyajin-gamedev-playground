using System;
using UnityEngine;

[Serializable]
public class PlayerMovement
{
    public Vector2 playerPosition;
    public bool canMove;
    public float moveSpeed;
    public float moveSpeedCache;
    public float dashCooldown;
    public float dashSpeed;
    public float dashDuration;
    public int dashStacksMax;
    public int dashStacksCurrent;
    public bool preventMoveSpeedReductionFromWeaponTrigger;
    public Action OnDashStart;
    public Action OnDashEnd;
    public Action OnDashStacksMaxUpdate;
    public Action OnDashStacksCurrentUpdate;
    public Action OnMoveSpeedUpdate;

    // CALLED ON NEW SAVE FILE & ON RUN END
    public PlayerMovement()
    {
        playerPosition = Vector2.zero;
        canMove = true;
        moveSpeed = 1f;
        moveSpeedCache = moveSpeed;
        dashSpeed = 4f;
        dashDuration = 0.2f;
        dashCooldown = 3f;
        dashStacksMax = 0;
        dashStacksCurrent = dashStacksMax;
        preventMoveSpeedReductionFromWeaponTrigger = false;
    }
    
    public void EnableMovement() => canMove = true;
    public void DisableMovement() => canMove = false;
    private void UpdateMoveSpeedCache() => moveSpeedCache = moveSpeed;
    public void IncreaseDashCooldown(float amount) => dashCooldown += amount;
    public void DecreaseDashCooldown(float amount) => dashCooldown -= amount;
    public void IncrementDashStacksMax(int amount) => SetDashStacksMax(dashStacksMax + amount);

    public void SetDashStacksMax(int amount)
    {
        dashStacksMax = amount;

        if (dashStacksMax < 0)
            dashStacksMax = 0;

        if (dashStacksCurrent > dashStacksMax)
            dashStacksCurrent = dashStacksMax;

        OnDashStacksMaxUpdate?.Invoke();
    }

    public void IncrementDashStacksCurrent(int amount)
    {
        dashStacksCurrent += amount;
        OnDashStacksCurrentUpdate?.Invoke();
    }

    public void ResetToOriginalMoveSpeed()
    {
        moveSpeed = moveSpeedCache;
        UpdateMoveSpeedCache();
    }

    public void ApplyTemporaryMoveSpeedChange(float decimalPercentage)
    {
        if (decimalPercentage == 0)
            return;
            
        moveSpeed = moveSpeedCache + (moveSpeedCache * decimalPercentage);

        if (moveSpeed < 0)
            moveSpeed = 0.1f;

        OnMoveSpeedUpdate?.Invoke();
    }

    public void ApplyPermanentMoveSpeedChange(float decimalPercentage)
    {
        moveSpeed = moveSpeedCache + (moveSpeedCache * decimalPercentage);

        if (moveSpeed < 0)
            moveSpeed = 1f;

        UpdateMoveSpeedCache();
        OnMoveSpeedUpdate?.Invoke();
    }
    
    public void ApplyPermanentMoveSpeedIncrement(float decimalPercentageToIncrement)
    {
        moveSpeed = moveSpeedCache + decimalPercentageToIncrement;

        if (moveSpeed < 0)
            moveSpeed = 1f;

        UpdateMoveSpeedCache();
        OnMoveSpeedUpdate?.Invoke();
    }

    public void IncrementMoveSpeed(float amount)
    {
        moveSpeed = moveSpeedCache + amount;

        if (moveSpeed < 0)
            moveSpeed = 1f;

        UpdateMoveSpeedCache();
        OnMoveSpeedUpdate?.Invoke();
    }
}