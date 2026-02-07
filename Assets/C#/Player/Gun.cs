using Unity.VisualScripting;
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
   private BulletStats stats;
   public void SetBulletStats(BulletStats s) {stats = s;}
   
   [Header("Gun Styles")]
   private GunStyleType _activeStyle;

   private void Awake()
   {
      _pool = new ObjectPool<Bullet>(
         () => Instantiate(bulletPrefab, transform.position, transform.parent.rotation).GetComponent<Bullet>(),
         e => e.gameObject.SetActive(true),
         e => e.gameObject.SetActive(false),
         e => Destroy(e.gameObject),
         true,
         objectPoolDefaultCapacity,
         objectPoolMaxCapacity
      );
      _activeStyle = GunStyleType.Default;
   }
   
   public void Release(Bullet bullet)
   {
      _pool.Release(bullet);
      _activeBullets--;
   } 
   
   public void Fire()
   {
      switch (_activeStyle)
      {
         case GunStyleType.Default:
            DefaultFire();
            break;
         case GunStyleType.Shotgun:
            ShotgunFire();
            break;
         case GunStyleType.SemiAutomatic:
            SemiAutomaticFire();
            break;
         case GunStyleType.DuelBarrel:
            DuelBarrelFire();
            break;
         case GunStyleType.TwinShot:
            TwinFire();
            break;
         case GunStyleType.RocketLauncher:
            RocketFire();
            break;
         case GunStyleType.ChargeGun:
            ChargeFire();
            break;
      }
   }

   public Bullet CreateBullet(Vector3 pos, Quaternion rot)
   {
      Bullet bullet = _pool.Get();
      bullet.SetStats(stats);
      bullet.transform.position = pos;
      bullet.transform.rotation = rot;
      return bullet;
   }

   public void EnableBullet(Bullet b)
   {
      b.Init(this);
      _activeStyle++;
   }

   public void SwitchActiveStyle(GunStyleType gunType)
   {
      _activeStyle = gunType;
   }
   
   #region Gun Fire Styles

   public void DefaultFire()
   {
      Bullet bullet = CreateBullet(transform.position, transform.rotation);
      EnableBullet(bullet);
   }

   public void ShotgunFire()
   {
      
   }

   public void SemiAutomaticFire()
   {
      
   }

   public void RocketFire()
   {
      
   }

   public void DuelBarrelFire()
   {
      
   }

   public void TwinFire()
   {
      
   }

   public void ChargeFire()
   {
      
   }
   #endregion
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
