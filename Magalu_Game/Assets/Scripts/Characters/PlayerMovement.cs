using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;

    [Header("Movement Variables")]
    public Vector2 CharacterMovementInput;
    private Vector3 _characterMovement;
    private Vector3 _positionToLookAt;

    [Header("Jump Variables")]
    [SerializeField] private Transform _groundVerify;
    [SerializeField] private LayerMask _sceneryMask;

    [Header("Physics")]
    [SerializeField] private float gravity = -9.81f;
    private float _verticalVelocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        SetMovement();
        RotationHandler();
        SetJump();
    }

    private void SetMovement()
    {
        _characterMovement = new Vector3(PlayerManager.Instance.GetCharacterMovementInput().x, _verticalVelocity, PlayerManager.Instance.GetCharacterMovementInput().y);

        PlayerManager.Instance.SetIsMoving(PlayerManager.Instance.GetCharacterMovementInput().x != 0 || PlayerManager.Instance.GetCharacterMovementInput().y != 0);

        _characterController.Move(_characterMovement * PlayerManager.Instance.GetCharacterVelocity() * Time.deltaTime);

        _verticalVelocity += gravity * Time.deltaTime;
    }

    private void RotationHandler()
    {
        _positionToLookAt = new Vector3(_characterMovement.x, 0, _characterMovement.z);
        Quaternion currentRotation = transform.rotation;

        if (PlayerManager.Instance.GetIsMoving())
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(_positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, lookAtRotation, PlayerManager.Instance.GetRotationVelocity() * Time.deltaTime);
        }
    }

    private void SetJump()
    {
        PlayerManager.Instance.SetIsGrounded(Physics.CheckSphere(_groundVerify.position, 0.3f, _sceneryMask));

        if (PlayerManager.Instance.GetJumpPressed() && PlayerManager.Instance.GetIsGrounded())
        {
            _verticalVelocity = Mathf.Sqrt(PlayerManager.Instance.GetJumpHeight() * -2f * gravity);
        }

        if (PlayerManager.Instance.GetIsGrounded() && _verticalVelocity < 0)
        {
            _verticalVelocity = -1f;
        }
    }
}
