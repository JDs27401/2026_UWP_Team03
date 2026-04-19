using System;
using Economy;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Build
{
    public class MouseTurretBuilder : MonoBehaviour
    {
        [Header("Build Setup")]
        [SerializeField] private GameObject turretPrefab;
        [SerializeField] private LayerMask placementMask = ~0;
        [SerializeField] private float placementOffsetY;
        [SerializeField] private bool keepBuildModeAfterPlacement = true;
        [SerializeField] private int turretBuildCost = 10;

        [Header("Sell Setup")]
        [SerializeField] private int turretSellRefund = 5;

        [Header("Input")]
        [SerializeField] private Key toggleBuildModeKey = Key.B;

        public bool IsBuildModeEnabled { get; private set; }
        public bool IsSellModeEnabled { get; private set; }

        public event Action<bool> OnBuildModeChanged;
        public event Action<bool> OnSellModeChanged;
        public event Action<GameObject> OnTurretPlaced;
        public event Action<GameObject> OnTurretSold;

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current[toggleBuildModeKey].wasPressedThisFrame)
            {
                SetBuildMode(!IsBuildModeEnabled);
            }

            if (!IsBuildModeEnabled && !IsSellModeEnabled)
            {
                return;
            }

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (IsBuildModeEnabled)
                {
                    TryPlaceTurretAtMousePosition();
                }
                else if (IsSellModeEnabled)
                {
                    TrySellTurretAtMousePosition();
                }
            }
        }

        public void SetBuildMode(bool isEnabled)
        {
            if (IsBuildModeEnabled == isEnabled)
            {
                return;
            }

            if (isEnabled && IsSellModeEnabled)
            {
                IsSellModeEnabled = false;
                OnSellModeChanged?.Invoke(false);
            }

            IsBuildModeEnabled = isEnabled;
            OnBuildModeChanged?.Invoke(IsBuildModeEnabled);
        }

        public void ToggleBuildMode()
        {
            SetBuildMode(!IsBuildModeEnabled);
        }

        public void SetSellMode(bool isEnabled)
        {
            if (IsSellModeEnabled == isEnabled)
            {
                return;
            }

            if (isEnabled && IsBuildModeEnabled)
            {
                IsBuildModeEnabled = false;
                OnBuildModeChanged?.Invoke(false);
            }

            IsSellModeEnabled = isEnabled;
            OnSellModeChanged?.Invoke(IsSellModeEnabled);
        }

        public void ToggleSellMode()
        {
            SetSellMode(!IsSellModeEnabled);
        }

        private void TryPlaceTurretAtMousePosition()
        {
            if (turretPrefab == null)
            {
                Debug.LogWarning("MouseTurretBuilder: Brak przypisanego prefabu wiezy.");
                return;
            }

            Camera cam = Camera.main;
            if (cam == null)
            {
                Debug.LogWarning("MouseTurretBuilder: Brak Camera.main na scenie.");
                return;
            }

            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, 2000f, placementMask, QueryTriggerInteraction.Ignore))
            {
                return;
            }

            BuildPlatform buildPlatform = hit.collider.GetComponentInParent<BuildPlatform>();
            if (buildPlatform == null)
            {
                Debug.Log("[Build] Wiezyczke mozna stawiac tylko na wyznaczonych platformach.");
                return;
            }

            if (SimpleEconomyService.Instance == null)
            {
                Debug.LogWarning("[Economy] Brak SimpleEconomyService - budowa przerwana.");
                return;
            }

            if (!SimpleEconomyService.Instance.TrySpendCredits(turretBuildCost, "postawienie wiezy"))
            {
                Debug.Log("[Build] Nie stac cie na wiezyczke - budowa przerwana.");
                return;
            }

            Vector3 spawnPosition = hit.point + Vector3.up * placementOffsetY;
            GameObject turret = Instantiate(turretPrefab, spawnPosition, Quaternion.identity);
            OnTurretPlaced?.Invoke(turret);

            Debug.Log($"[Build] Wiezyczka postawiona pomyslnie. Koszt: {turretBuildCost}. Nowe saldo: {SimpleEconomyService.Instance.Credits}.");

            if (!keepBuildModeAfterPlacement)
            {
                SetBuildMode(false);
            }
        }

        private void TrySellTurretAtMousePosition()
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                Debug.LogWarning("MouseTurretBuilder: Brak Camera.main na scenie.");
                return;
            }

            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, 2000f, ~0, QueryTriggerInteraction.Ignore))
            {
                return;
            }

            AutoTurretStandalone turret = hit.collider.GetComponentInParent<AutoTurretStandalone>();
            if (turret == null)
            {
                Debug.Log("[Sell] Kliknij w collider wiezyczki, aby ja sprzedac.");
                return;
            }

            if (SimpleEconomyService.Instance == null)
            {
                Debug.LogWarning("[Economy] Brak SimpleEconomyService - sprzedaz przerwana.");
                return;
            }

            GameObject turretObject = turret.gameObject;
            Destroy(turretObject);
            SimpleEconomyService.Instance.AddCredits(turretSellRefund, "sprzedaz wiezy");
            OnTurretSold?.Invoke(turretObject);

            Debug.Log($"[Sell] Sprzedano wiezyczke. Zwrot: {turretSellRefund}. Nowe saldo: {SimpleEconomyService.Instance.Credits}.");
        }
    }
}
