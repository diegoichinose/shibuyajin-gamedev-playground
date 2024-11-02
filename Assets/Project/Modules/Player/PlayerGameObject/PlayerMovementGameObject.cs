using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementGameObject : MonoBehaviour
{    
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Animator _anim;
    [SerializeField] private Transform _playerSprite;
    private MyInputActions _input;
    private InputAction _inputMove;
    private Rigidbody2D _rigidbody;
    
    [Header("DO NOT TOUCH - FOR INSPECTION ONLY")]
    public bool isFacingRight = true;   
    public bool isMoving = false;
    public Vector2 moveDirection = Vector2.zero;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    { 
        _input = new MyInputActions();
        _inputMove = _input.PlayerActionMap.Movement;
        _inputMove.Enable();
        _playerData.player.movement.EnableMovement();
    }

    void OnDisable()
    { 
        _input.Disable();
    }

    void Update()
    {
        if (!_playerData.player.movement.canMove)
            return;

        if (PauseResumeTimeManager.instance.isPaused)
        {
            _rigidbody.velocity = Vector2.zero;
            _anim.SetBool("IsMoving", false);
            return;
        }

        moveDirection = _inputMove.ReadValue<Vector2>();
        _rigidbody.velocity = (_playerData.player.movement.moveSpeed + 0.5f) * moveDirection;
    
        isMoving = _rigidbody.velocity.sqrMagnitude  > 0;
        _anim.SetBool("IsMoving", isMoving);

        FlipIfFacingTheWrongDirection(moveDirection.x);
    }

    private void FlipIfFacingTheWrongDirection(float horizontalMovement)
    {
        bool isNotMovingHorizontally = horizontalMovement == 0;
        if (isNotMovingHorizontally)
            return;
        
        bool isMovingAndFacingTheSameDirection = (horizontalMovement > 0 && isFacingRight) || (horizontalMovement < 0 && !isFacingRight);
        if (isMovingAndFacingTheSameDirection)
            return;

        Vector3 currentScale = _playerSprite.localScale;
        currentScale.x *= -1;
        _playerSprite.localScale = currentScale;
        isFacingRight = !isFacingRight;
    }
}