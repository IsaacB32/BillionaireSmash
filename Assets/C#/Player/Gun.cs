using System;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
   [SerializeField] private GameObject bulletPrefab;
   
   [Header("Object Pooler")]
   [SerializeField] private int objectPoolDefaultCapacity = 100;
   [SerializeField] private int objectPoolMaxCapacity = 500;

   private int _activeBullets = 0;
   private IObjectPool<Bullet> _pool;
   
   //Bullet Powerups
   public BulletStats stats; //default stats
   public void SetBulletStats(BulletStats s)
   {
      stats.size += s.size;
      stats.speed += s.speed;
      stats.pierce += s.pierce;
      stats.fire += s.fire;
      stats.electric += s.electric;
      stats.slowing += s.slowing;
      stats.chain += s.chain;
      stats.explode += s.explode;
   }
   public BulletStats GetStats() {return stats;}
   
   [Header("Gun Styles")]
   private GunStyleType _activeStyle = GunStyleType.Default;
   private Action _FireMethod;

   public GunStyleType debugStyle;


   [Header("Additional Bullet Spawn")]
   [SerializeField] private Transform _behindSpawn;
   [SerializeField] private Transform _leftSpawn;
   [SerializeField] private Transform _rightSpawn;
   [SerializeField] private Transform _barrel1;
   [SerializeField] private Transform _barrel2;
   
   [Space]
   [Header("Charge Shot")]
   public SpriteRenderer chargeIndicator;
   private float _startChargeSize = 0.01f;
   private float _chargeGrowthRate = 2.3f;

   private void Awake()
   {
      //DEBUG
      _activeStyle = debugStyle;
      
      _pool = new ObjectPool<Bullet>(
         () => Instantiate(bulletPrefab, transform.position, transform.parent.rotation).GetComponent<Bullet>(),
         e => e.gameObject.SetActive(true),
         e => e.gameObject.SetActive(false),
         e => Destroy(e.gameObject),
         true,
         objectPoolDefaultCapacity,
         objectPoolMaxCapacity
      );
      SwitchActiveStyle(_activeStyle, _chargeGrowthRate);
      HideCharge();
   }
   
   public void Release(Bullet bullet)
   {
      _pool.Release(bullet);
      _activeBullets--;
   } 
   
   public void Fire()
   {
      _FireMethod.Invoke();
   }
   
   private void HideCharge()
   {
      chargeIndicator.enabled = false;
      chargeIndicator.transform.localScale = Vector3.one * _startChargeSize;
   }
   
   public void CreateBullet(Vector3 pos, Quaternion rot)
   {
      Bullet bullet = _pool.Get();
      bullet.transform.position = pos;
      bullet.transform.rotation = rot;
      bullet.Init(this);
      _activeBullets++;
   }

   public void SwitchActiveStyle(GunStyleType gunType, float chargeGrowthRate = 0)
   {
      _activeStyle = gunType;
      switch (_activeStyle)
      {
         case GunStyleType.Default:
            _FireMethod = DefaultFire;
            break;
         case GunStyleType.Shotgun:
            _FireMethod = ShotgunFire;
            break;
         case GunStyleType.SemiAutomatic:
            _FireMethod = SemiAutomaticFire;
            break;
         case GunStyleType.DuelBarrel:
            _FireMethod = DuelBarrelFire;
            break;
         case GunStyleType.TwinShot:
            _FireMethod = TwinFire;
            break;
         case GunStyleType.RocketLauncher:
            _FireMethod = RocketFire;
            break;
         case GunStyleType.ChargeGun:
            _FireMethod = ChargeFire;
            _chargeGrowthRate = chargeGrowthRate;
            break;
      }
   }
   
   #region Gun Fire Styles

   public void DefaultFire()
   {
      CreateBullet(transform.position, transform.rotation);
   }

   public void ShotgunFire()
   {
      CreateBullet(transform.position, transform.rotation);
      Quaternion rot = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 45);
      CreateBullet(transform.position, rot);
      rot = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 45);
      CreateBullet(transform.position, rot);
   }

   public void SemiAutomaticFire()
   {
      
   }

   public void RocketFire()
   {
      
   }

   public void DuelBarrelFire()
   {
      CreateBullet(_barrel1.position, transform.rotation);
      CreateBullet(_barrel2.position, transform.rotation);
   }

   public void TwinFire()
   {
      Quaternion rot = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 180);
      CreateBullet(transform.position, transform.rotation);
      CreateBullet(_behindSpawn.position, rot);
   }

   public void ChargeFire()
   {
      chargeIndicator.enabled = true;
      chargeIndicator.transform.localScale += Vector3.one * _chargeGrowthRate * Time.deltaTime;
   }
   #endregion
   
   public void FireEnded()
   {
      float scale = chargeIndicator.transform.localScale.x;
      
      Bullet bullet = _pool.Get();
      bullet.transform.position = transform.position;
      bullet.transform.rotation = transform.rotation;
      bullet.Init(this);
      float percentage = Mathf.Clamp(scale / 1.5f, 0, 1f);
      bullet.OverrideSizeSpeed(scale * 1.5f, -1/scale, (int)(percentage * 10));
      _activeBullets++;

      HideCharge();
   }
}

public enum GunStyleType
{
    Default,
    Shotgun,
    SemiAutomatic, 
    RocketLauncher, 
    DuelBarrel,
    TwinShot,
    ChargeGun
}
