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
   }
   
   public void Release(Bullet bullet)
   {
      _pool.Release(bullet);
      _activeBullets--;
   } 

   
   public void Fire()
   {
      Bullet bullet = _pool.Get();
      bullet.transform.position = transform.position;
      bullet.transform.rotation = transform.rotation;
      bullet.Init(this);
      _activeBullets++;
   }
}
