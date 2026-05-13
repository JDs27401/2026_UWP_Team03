using UnityEngine;

namespace FactoryPattern
{
    public abstract class BaseTurretFactory : MonoBehaviour
    {
        public abstract AutoTurretStandalone CreateTurret(AutoTurretStandalone turretPrefab, Vector3 position);
    }
    
    public class TurretFactory : BaseTurretFactory
    {
        public override AutoTurretStandalone CreateTurret(AutoTurretStandalone turretPrefab, Vector3 position)
        {
            if (turretPrefab == null) return null;
            AutoTurretStandalone newTurret = turretPrefab.Clone(); 
            
            newTurret.transform.position = position;
            newTurret.transform.rotation = Quaternion.identity;

            return newTurret;
        }
    }
}