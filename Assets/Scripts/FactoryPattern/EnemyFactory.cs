using UnityEngine;

namespace FactoryPattern
{
    public abstract class BaseEnemyFactory : MonoBehaviour
    {
        public abstract Enemy CreateEnemy(Transform spawnPoint);
    }
    
    public class EnemyFactory : BaseEnemyFactory
    {
        [Header("Enemy Prototypes")] [SerializeField]
        private Enemy[] enemyPrototypes;

        public override Enemy CreateEnemy(Transform spawnPoint)
        {
            if (enemyPrototypes == null || enemyPrototypes.Length == 0) return null;
            
            int randomIndex = UnityEngine.Random.Range(0, enemyPrototypes.Length);
            Enemy prototype = enemyPrototypes[randomIndex];
            
            Enemy newEnemy = prototype.Clone(); 
            
            newEnemy.transform.position = spawnPoint.position;
            newEnemy.transform.rotation = spawnPoint.rotation;

            return newEnemy;
        }
    }
}