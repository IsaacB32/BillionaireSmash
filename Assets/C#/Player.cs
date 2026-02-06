using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState {
    Idle,
    Standing,
    Moving
}
public class Player : MonoBehaviour
{
    public float movement_speed = 10f;
    [Header("References")]
    [SerializeField] private GameObject _bodyReference;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _move_direction;
    private float _rotation_angle;

    private float _idleTimerMax = 2.5f;
    private float _idleTimer;
    
    private PlayerAnimations _animations;
    private Gun _gun;

    private bool _holdingFire = false;

    public PlayerState movementState
    {
        set => _animations.UpdateState(value);
    }

    private void Start()
    {
        _rigidbody2D = GetComponentInChildren<Rigidbody2D>();
        _animations = GetComponentInChildren<PlayerAnimations>();
        _gun = GetComponentInChildren<Gun>();
    }

    #region Input
    public void Move(InputAction.CallbackContext context)
    {
        _move_direction = context.ReadValue<Vector2>();
        if (context.started)
        {
            movementState = PlayerState.Moving;
            _idleTimer = 0;
        }
    }

    public void MousePoint(InputAction.CallbackContext context)
    {
        Vector2 mouseScreenPos = context.ReadValue<Vector2>();
        Vector2 startingScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        mouseScreenPos.x -= startingScreenPos.x;
        mouseScreenPos.y -= startingScreenPos.y;
        _rotation_angle = Mathf.Atan2(mouseScreenPos.y, mouseScreenPos.x) * Mathf.Rad2Deg;
    }

    public void FireGun(InputAction.CallbackContext context)
    {
        if (context.performed) _holdingFire = true;
        else if (context.canceled) _holdingFire = false;
    }
    #endregion

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = _move_direction * movement_speed * Time.deltaTime * 50;
        
        Vector3 rotation_temp = _bodyReference.transform.localEulerAngles;
        rotation_temp.z = _rotation_angle;
        _bodyReference.transform.localEulerAngles = rotation_temp;

        if (_move_direction == Vector2.zero)
        {
            _idleTimer += Time.deltaTime;
            if (_idleTimer >= _idleTimerMax)
            {
                movementState = PlayerState.Idle;
            }
            movementState = PlayerState.Standing;
        }
        
        if (_holdingFire) _gun.Fire();
    }
}
