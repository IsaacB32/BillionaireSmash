using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState {
    Idle,
    Moving
}
public class Player : MonoBehaviour
{
    [Header("Properties")]
    public float movement_speed = 10f;
    public int max_health = 2;
    public int money;
    private int current_health;
    [SerializeField] private float _fireTimerInterval = 0.1f;
    private float _defaultFireTimer;

    public void setMaxHealth(int h)
    {
        if (h == 0) return;
        max_health = h;
        current_health = max_health;
    }
    
    [Header("References")]
    [SerializeField] private GameObject _indicator;
    private PlayerAnimations _animations;
    private Gun _gun;
    private PlayerPowerups _playerPowerups;
    [SerializeField] private GameObject _explodePrefab;
    
    private Rigidbody2D _rigidbody2D;
    private Vector2 _move_direction;
    private float _rotation_angle;
    
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
        _playerPowerups = GetComponent<PlayerPowerups>();
        _defaultFireTimer = _fireTimerInterval;
    }

    #region Input
    public void Move(InputAction.CallbackContext context)
    {
        _move_direction = context.ReadValue<Vector2>();
        if (context.started && !_freeze)
        {
            movementState = PlayerState.Moving;
        }
    }

    private void Rotate()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 startingScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        
        mouseScreenPos.x -= startingScreenPos.x;
        mouseScreenPos.y -= startingScreenPos.y;
        
        _rotation_angle = Mathf.Atan2(mouseScreenPos.y, mouseScreenPos.x) * Mathf.Rad2Deg;
        Vector3 rotation_temp = _indicator.transform.localEulerAngles;
        rotation_temp.z = _rotation_angle;
        _indicator.transform.localEulerAngles = rotation_temp;
    }

    public void FireGun(InputAction.CallbackContext context)
    {
        if (Game.Instance.state == Game.GameState.Paused)
        {
            _holdingFire = false;
            return;
        }
        
        if (context.performed) _holdingFire = true;
        else if (context.canceled)
        {
            _holdingFire = false;
            _gun.FireEnded();
        }
    }
    #endregion

    private void FixedUpdate()
    {
        if (_freeze) return;
        
        _rigidbody2D.linearVelocity = _move_direction * movement_speed * Time.deltaTime * 50;
        Rotate();
        
        if (_move_direction == Vector2.zero)  movementState = PlayerState.Idle;

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
        if (newSpeed != 0) movement_speed += newSpeed;
        setMaxHealth(newHealth);
    }

    public void UpgradeBullets(BulletStats stats)
    {
        _gun.SetBulletStats(stats);
    }

    public void UpgradeGun(GunStyleType gunType, float rateFire, float growthRate)
    {
        _gun.SwitchActiveStyle(gunType, growthRate);
        _fireTimerInterval = rateFire == 0 ? _defaultFireTimer : rateFire;
    }
    #endregion
    
    public GameObject GetExplode() { return _explodePrefab; }
    public GunStyleType GetStyle() {return _gun.GetActiveStyle();}
}
