using UnityEngine;
using Economy;
using CommandPattern;

namespace Build
{
    
    public class SellTurretCommand : ICommand
    {
        private GameObject _turret;
        private int _refund;

        public SellTurretCommand(GameObject turret, int refund) 
        {
            _turret = turret;
            _refund = refund;
        }

        public bool Execute() 
        {
            if (_turret == null) return false;
        
            _turret.SetActive(false);
            SimpleEconomyService.Instance.AddCredits(_refund, "Sell Tower");
            return true;
        }

        public void Undo() 
        {
            
            if (SimpleEconomyService.Instance.TrySpendCredits(_refund, "Undo Sell Tower")) 
            {
                _turret.SetActive(true);
            }
        }
    }
}