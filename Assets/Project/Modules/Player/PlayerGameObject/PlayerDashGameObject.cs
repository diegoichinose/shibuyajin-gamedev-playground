using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashGameObject : MonoBehaviour
{
    private MyInputActions _input;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private PlayerMovementGameObject _playerMovementGameObject;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameObject _playerTrail;
    [SerializeField] private PlayerAudioLibrary _playerAudioLibrary;

    void Awake()
    {
        TryEnableSelf();
        _playerData.player.movement.OnDashStacksMaxUpdate += TryEnableSelf;
    }

    void OnDestroy() => _playerData.player.movement.OnDashStacksMaxUpdate -= TryEnableSelf;

    private void TryEnableSelf()
    {
        if (_playerData.player.movement.dashStacksMax > 0)
            this.enabled = true;
        else 
            this.enabled = false;
    }

    void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _input = new MyInputActions();
        _input.Enable();
        _input.PlayerActionMap.Dash.started += OnDash;

        StartCoroutine(PassivelyRecoverDashStacks());
    }

    void OnDisable()
    { 
        _input.Disable();
        _input.PlayerActionMap.Dash.started -= OnDash;
        
        StopAllCoroutines();
    }

    public void OnDash(InputAction.CallbackContext context)
	{
		if (!_playerData.player.movement.canMove)
			return;

        if (_playerData.player.movement.dashStacksCurrent == 0)
            return;

        if (_playerMovementGameObject.moveDirection == Vector2.zero)
            return;

		Vector2 cacheCurrentVelocity = _rigidbody.velocity;
        _sprite.enabled = false;
		_playerData.player.movement.canMove = false;
		_playerData.player.health.isInvincible = true;
        
		_rigidbody.velocity = _playerMovementGameObject.moveDirection * (_playerData.player.movement.moveSpeed + _playerData.player.movement.dashSpeed);

        _playerTrail.SetActive(true);
        _playerData.player.movement.IncrementDashStacksCurrent(-1);
		StartCoroutine(StopDashAfterCountdown(_playerData.player.movement.dashDuration, cacheCurrentVelocity));

        _playerData.player.movement.OnDashStart?.Invoke();
        _playerAudioLibrary.PlayDashSound();
	}
    
	private IEnumerator StopDashAfterCountdown(float duration,  Vector2 velocityResetValue) 
	{
		yield return new WaitForSeconds(duration); 
        _rigidbody.velocity = velocityResetValue;
        _sprite.enabled = true;
		_playerData.player.movement.canMove = true;
		_playerData.player.health.isInvincible = false;
        _playerTrail.SetActive(false);
        _playerData.player.movement.OnDashEnd?.Invoke();
    }
    
	private IEnumerator PassivelyRecoverDashStacks()
	{
        while (true)
        {
		    yield return new WaitForSeconds(_playerData.player.movement.dashCooldown); 
            
            if (PauseResumeTimeManager.instance.isPaused)
                continue;

            if (_playerData.player.movement.dashStacksCurrent >= _playerData.player.movement.dashStacksMax)
                continue;

            _playerData.player.movement.IncrementDashStacksCurrent(1);
        }
    }
}