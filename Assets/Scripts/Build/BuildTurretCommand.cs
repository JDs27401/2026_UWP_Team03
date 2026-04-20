using CommandPattern;
using Economy;
using UnityEngine;


namespace Build
{
    public class BuildTurretCommand : ICommand
    {
        private GameObject _turretPrefab;
        private Vector3 _position;
        private int _cost;
        private GameObject _placedTurret;

        public BuildTurretCommand(GameObject prefab, Vector3 position, int cost) 
        {
            _turretPrefab = prefab;
            _position = position;
            _cost = cost;
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
                _placedTurret = Object.Instantiate(_turretPrefab, _position, Quaternion.identity);
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