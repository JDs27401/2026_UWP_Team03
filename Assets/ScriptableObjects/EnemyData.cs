using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public float health = 100f;
        public float damage = 10f;
    }
}