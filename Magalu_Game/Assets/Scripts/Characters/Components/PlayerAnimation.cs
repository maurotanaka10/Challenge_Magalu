using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private CharacterController _characterControllerRef;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private PlayerManager _playerManager;

    private int _velocityHash;
    private int _isJumpingHash;
    private int _isSlashingHash;
    private int _isDeathHash;

    private bool _isJumping;
    private bool _isAttacking;
    private bool _isDeath;
    private float _currentVelocity;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        GetAnimatorParameters();
        
        PlayerManager.HandleJumpInput += JumpAnimation;
        PlayerManager.HandleAttackInput += AttackAnimation;
        PlayerManager.HandlePlayerDeath += DeathAnimation;
    }

    private void DeathAnimation(bool playerDeath)
    {
        _isDeath = playerDeath;
        bool isDeathAnimation = _animator.GetBool(_isDeathHash);

        if (_isDeath && !isDeathAnimation)
        {
            _animator.SetBool(_isDeathHash, true);
        }
        else if (!_isDeath && isDeathAnimation)
        {
            _animator.SetBool(_isDeathHash, false);
        }

        if (_playerManager._lifes == 0 && !isDeathAnimation)
        {
            _animator.SetBool(_isDeathHash, true);
        }
        else if (_playerManager._lifes == 0 && isDeathAnimation)
        {
            _animator.SetBool(_isDeathHash, true);
        }
    }

    private void Update()
    {
        _characterControllerRef = PlayerManager.CharacterControllerRef?.Invoke();
        MoveAnimation();
        
        if (_gameManager._gameIsOver)
        {
            _animator.SetBool("gameIsOver", true);
        }
    }

    private void MoveAnimation()
    {
        _currentVelocity = _characterControllerRef.velocity.magnitude;
        _animator.SetFloat(_velocityHash, _currentVelocity);
    }

    private void JumpAnimation(bool jumpPressed, float jumpPower)
    {
        this._isJumping = jumpPressed;
        bool isJumpingAnimation = _animator.GetBool(_isJumpingHash);

        if (_isJumping && !isJumpingAnimation && _characterControllerRef.isGrounded)
        {
            _animator.SetBool(_isJumpingHash, true);
        }
        else if (isJumpingAnimation || _characterControllerRef.isGrounded)
        {
            _animator.SetBool(_isJumpingHash, false);
        }
    }

    private void AttackAnimation(bool attackPressed)
    {
        this._isAttacking = attackPressed;
        bool isAttackingAnimation = _animator.GetBool(_isSlashingHash);

        if (_isAttacking && !isAttackingAnimation)
        {
            _animator.SetBool(_isSlashingHash, true);
        }
        else if (isAttackingAnimation && !this._isAttacking)
        {
            _animator.SetBool(_isSlashingHash, false);
        }
    }

    private void GetAnimatorParameters()
    {
        _velocityHash = Animator.StringToHash("velocity");
        _isJumpingHash = Animator.StringToHash("isJumping");
        _isSlashingHash = Animator.StringToHash("isSlashing");
        _isDeathHash = Animator.StringToHash("isDeath");
    }

    private void OnDisable()
    {
        PlayerManager.HandleJumpInput -= JumpAnimation;
        PlayerManager.HandleAttackInput -= AttackAnimation;
        PlayerManager.HandlePlayerDeath -= DeathAnimation;
    }
}
