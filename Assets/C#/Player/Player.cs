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
    [Header("Properties")]
    public float movement_speed = 10f;
    public int max_health = 2;
    private int current_health;
    [SerializeField] private float _fireTimerInterval = 0.1f;

    public void setMaxHealth(int h)
    {
        if (h == 0) return;
        max_health = h;
        current_health = max_health;
    }
    
    [Header("References")]
    [SerializeField] private GameObject _bodyReference;
    private PlayerAnimations _animations;
    private Gun _gun;
    private PlayerPowerups _playerPowerups;
    
    private Rigidbody2D _rigidbody2D;
    private Vector2 _move_direction;
    private float _rotation_angle;

    private float _idleTimerMax = 2.5f;
    private float _idleTimer;
    
    private bool _holdingFire = false;
    private float _fireTimer;

    private bool _freeze = false;

    private PlayerState _currentMovementState;
    public PlayerState movementState
    {
        set
        {
            _animations.UpdateState(value);
            _currentMovementState = value;
        } 
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
        if (context.started && !_freeze)
        {
            movementState = PlayerState.Moving;
            _idleTimer = 0;
        }
    }

    private void Rotate()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 startingScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        
        mouseScreenPos.x -= startingScreenPos.x;
        mouseScreenPos.y -= startingScreenPos.y;
        
        _rotation_angle = Mathf.Atan2(mouseScreenPos.y, mouseScreenPos.x) * Mathf.Rad2Deg;
        Vector3 rotation_temp = _bodyReference.transform.localEulerAngles;
        rotation_temp.z = -_rotation_angle;
        _bodyReference.transform.localEulerAngles = rotation_temp;
    }

    public void FireGun(InputAction.CallbackContext context)
    {
        if (context.performed) _holdingFire = true;
        else if (context.canceled) _holdingFire = false;
    }
    #endregion

    private void FixedUpdate()
    {
        if (_freeze) return;
        
        _rigidbody2D.linearVelocity = _move_direction * movement_speed * Time.deltaTime * 50;
        Rotate();
        
        if (_move_direction == Vector2.zero)
        {
            _idleTimer += Time.deltaTime;
            if (_idleTimer >= _idleTimerMax)
            {
                movementState = PlayerState.Idle;
            }
            movementState = PlayerState.Standing;
        }

        if (_holdingFire)
        {
            if (_fireTimer >= _fireTimerInterval)
            {
                _gun.Fire();
                _fireTimer = 0;
            }
            else _fireTimer += Time.deltaTime;
        }
    }

    #region Powerup Upgrades
    public void AttachPowerup(Powerup p)
    {
        _playerPowerups.AttachPowerup(p);
    }
    
    public void SetUpgradeStats(float newSpeed, int newHealth)
    {
        movement_speed = newSpeed == 0 ? movement_speed : newSpeed;
        setMaxHealth(newHealth);
    }

    public void UpgradeBullets(BulletStats stats)
    {
        _gun.SetBulletStats(stats);
    }

    public void UpgradeGun(GunStyleType gunType, float rateFire)
    {
        _gun.SwitchActiveStyle(gunType);
        _fireTimerInterval = rateFire == 0 ? _fireTimerInterval : rateFire;
    }
    #endregion
}
