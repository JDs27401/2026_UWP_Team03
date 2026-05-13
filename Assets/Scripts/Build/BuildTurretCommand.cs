using CommandPattern;
using Economy;
using FactoryPattern;
using UnityEngine;


namespace Build
{
    public class BuildTurretCommand : ICommand
    {
        private AutoTurretStandalone _turretPrefab;
        private Vector3 _position;
        private int _cost;
        private GameObject _placedTurret;
        private BaseTurretFactory _turretFactory;

        public BuildTurretCommand(AutoTurretStandalone prefab, Vector3 position, int cost, BaseTurretFactory turretFactory) 
        {
            _turretPrefab = prefab;
            _position = position;
            _cost = cost;
            _turretFactory = turretFactory;
        }

        public bool Execute() 
        {
            if (!SimpleEconomyService.Instance.TrySpendCredits(_cost, "Build Tower")) return false;
            
            if (_placedTurret != null) 
            {
                _placedTurret.SetActive(true);
            }
            else
            {
                AutoTurretStandalone createdTurret = _turretFactory.CreateTurret(_turretPrefab, _position);
                _placedTurret = createdTurret.gameObject;
            }
            return true;
        }

        public void Undo() 
        {
            if (_placedTurret != null) 
            {
                _placedTurret.SetActive(false);
                SimpleEconomyService.Instance.AddCredits(_cost, "Undo Build Tower");
            }
        }
    }
}