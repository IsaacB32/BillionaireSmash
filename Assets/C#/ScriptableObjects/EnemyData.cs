using UnityEngine;

namespace C_.ScriptableObjects
{
    [CreateAssetMenu]
    public class EnemyData : ScriptableObject {
        public string enemyName;
        public float health;
        public float speed;
        public Color spriteColor;
        public float scale = 1f;
        public GameObject dropPrefab;
    }
}