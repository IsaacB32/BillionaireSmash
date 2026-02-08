using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HurtFlash : MonoBehaviour
{
   [SerializeField] private float _flashTime = 0.2f;
   private SpriteRenderer _sprite;

   private void Start()
   {
      _sprite = GetComponent<SpriteRenderer>();
   }

   public void Flash()
   {
      StartCoroutine(FlashEffect());
   }
   
   private IEnumerator FlashEffect()
   {
      float elapsed = 0f;
      while (elapsed < _flashTime)
      {
         elapsed += Time.deltaTime;
         _sprite.color = (Mathf.FloorToInt(elapsed * 20) % 2 == 0) ? Color.white : Color.red;
         yield return null;
      }
      _sprite.color = Color.white;
   }
}
