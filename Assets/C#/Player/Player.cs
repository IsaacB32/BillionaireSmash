using System;
using System.Collections;
using TMPro;
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
    public int money;
    public float maxHealth = 100f;
    public float currentHealth;
    [SerializeField] private float _fireTimerInterval = 0.1f;
    private float _defaultFireTimer;
    [SerializeField] private float _invincibleTime = 0.33f;
    [SerializeField] private bool _canBeHurt = true;
    [SerializeField] private float _dashTime = 0.05f;
    [SerializeField] private float _defaultDashForce = 2f;
    [SerializeField] private float _defaultDashCooldown = 1f;
    private float _dashForce;
    private float _dashMultiplier = 1f;
    private float _dashCooldown;
    private bool _canDash = true;
    private SpriteRenderer _spriteRenderer;
    
    [Header("References")]
    [SerializeField] private GameObject _indicator;
    private PlayerAnimations _animations;
    private Gun _gun;
    private PlayerPowerups _playerPowerups;
    [SerializeField] private GameObject _explodePrefab; 
    private HurtFlash hurtFlash;
    
    private Rigidbody2D _rigidbody2D;
    private Vector2 _move_direction;
    private float _rotation_angle;
    
    private bool _holdingFire = false;
    private float _fireTimer;

    private bool _freeze = false;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float knockbackDuration = 0.3f;
    [SerializeField] private Material whiteMat;
    private bool _isKnockedBack =false;
    private Material _defaultMat;

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
        _dashForce = _defaultDashForce;
        hurtFlash = GetComponentInChildren<HurtFlash>();
        _dashCooldown = _defaultDashCooldown;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _defaultMat = _spriteRenderer.material;

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

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Game.Instance.state == Game.GameState.Playing) Game.Instance.Pause();
            else if (Game.Instance.state == Game.GameState.Paused) Game.Instance.Unpause();
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
        if (Game.Instance.state != Game.GameState.Playing) return;
        
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

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && _canDash)
        {
            StartCoroutine(DashCooldown());
            
            _dashMultiplier = _dashForce;
            StartCoroutine(InvincibleFlash(_dashTime));
            StartCoroutine(Invincible(_dashTime));
            Invoke(nameof(EndDash), _dashTime);
        }
    }

    private IEnumerator DashCooldown()
    {
        _canDash = false;
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }

    private void EndDash()
    {
        _dashMultiplier = 1;
    }

    public void HackAttack(InputAction.CallbackContext context)
    {
        Game.Instance.difficulty.RaiseMaxLevel();
    } 
    #endregion

    private void FixedUpdate()
    {
        if (_freeze || Game.Instance.state != Game.GameState.Playing || _isKnockedBack) return;
        
        _rigidbody2D.linearVelocity = _move_direction * movement_speed * Time.deltaTime * 50 * _dashMultiplier;
        Rotate();
        
        if (_move_direction == Vector2.zero)  movementState = PlayerState.Idle;

        // change it back if you guys don't like it
        //if (_holdingFire)  
        {
            if (_fireTimer >= _fireTimerInterval)
            {
                _gun.Fire();
                _fireTimer = 0;
            }
            else _fireTimer += Time.deltaTime;
        }

        if (currentHealth < 1)
        {
            Game.Instance.GameOver();
        }
    }

    #region Powerup Upgrades
    public void AttachPowerup(Powerup p)
    {
        _playerPowerups.AttachPowerup(p);
    }
    
    public void SetUpgradeStats(float newSpeed, int newHealth, float newDash, float newDashTime, float newCooldown)
    {
        if (newSpeed != 0) movement_speed += newSpeed;
        IncreaseHealth(newHealth);

        if (newDash != 0) _dashForce += newDash;
        if (newDashTime != 0) _dashTime += newDashTime;
        if (newCooldown != 0) _dashCooldown -= newCooldown;

        movement_speed = Mathf.Clamp(movement_speed, 0.8f, 100f);
        _dashCooldown = Mathf.Clamp(_dashCooldown, 0.2f, 2f);
    }

    public void UpgradeBullets(BulletStats stats)
    {
        _gun.SetBulletStats(stats);
    }

    public void UpgradeGun(GunStyleType gunType, float rateFire, float growthRate)
    {
        _animations.SwitchGunArt(gunType);
        _gun.SwitchActiveStyle(gunType, growthRate);
        _fireTimerInterval = rateFire == 0 ? _defaultFireTimer : rateFire;
    }
    #endregion
    
    public GameObject GetExplode() { return _explodePrefab; }
    public GunStyleType GetStyle() {return _gun.GetActiveStyle();}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;
        if (!_canBeHurt) return;
        
        DecreaseHealth(other.gameObject.GetComponent<Enemy>());
        if (currentHealth <= 0)
        {
            Game.Instance.GameOver();
        }
        if (!_isKnockedBack) StartCoroutine(Knockback(other.transform));
        StartCoroutine(FlashColor());
        Game.Instance.cameraShake.PlayModerate();
    }
    
    private IEnumerator Knockback(Transform other)
    {
        _isKnockedBack = true;
        Vector2 knockbackDirection = (_rigidbody2D.position - (Vector2)other.position).normalized;
        
        _rigidbody2D.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        _isKnockedBack = false;
        _rigidbody2D.linearVelocity = Vector2.zero;
    }

    private IEnumerator FlashColor()
    {
        _spriteRenderer.material = whiteMat;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.material = _defaultMat;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.material = whiteMat;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.material = _defaultMat;
    }
    
    private void IncreaseHealth(int h)
    {
        if (h == 0) return;
        currentHealth += h;
    }


    private void DecreaseHealth(Enemy e)
    {
        if (!_canBeHurt) return;
        
        currentHealth = Mathf.Clamp(currentHealth - e.currentStats.value * 2, 0, maxHealth);
        
        hurtFlash.Flash();
        Game.Instance.audioManager.PlayPlayerHit();
        StartCoroutine(Invincible(_invincibleTime));
    }

    IEnumerator Invincible(float time)
    {
        _canBeHurt = false;
        yield return new WaitForSeconds(time);
        _canBeHurt = true;
    }

    IEnumerator InvincibleFlash(float time)
    {
        _spriteRenderer.material = whiteMat;
        yield return new WaitForSeconds(time-.05f);
        _spriteRenderer.material = _defaultMat;
    }

    public int GetDamage()
    {
        return _gun.GetStats().damage;
    }
}
