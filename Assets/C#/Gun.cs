using UnityEngine;

public class Gun : MonoBehaviour
{
   [SerializeField] private GameObject _bulletPrefab;

   public void Fire()
   {
      //pooling
      Instantiate(_bulletPrefab, transform.position, transform.parent.rotation);
   }
}
