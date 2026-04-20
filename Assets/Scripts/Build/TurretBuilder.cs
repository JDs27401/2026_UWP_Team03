using System;
using CommandPattern;
using Economy;
using UnityEngine;
using UnityEngine.InputSystem;
using CommandPattern;

namespace Build
{
    public class TurretBuilder : MonoBehaviour
    {
        [Header("Build Setup")]
        [SerializeField] private GameObject turretPrefab;
        [SerializeField] private LayerMask placementMask = ~0;
        [SerializeField] private float placementOffsetY;
        [SerializeField] private bool keepBuildModeAfterPlacement = true;
        [SerializeField] private int turretBuildCost = 10;

        [Header("Sell Setup")]
        [SerializeField] private int turretSellRefund = 5;

        [Header("New Input System")]
        [SerializeField] private InputActionReference toggleBuildModeAction;
        [SerializeField] private InputActionReference toggleSellModeAction;
        [SerializeField] private InputActionReference undoAction;
        [SerializeField] private InputActionReference redoAction;
        [SerializeField] private InputActionReference clickAction;
        [SerializeField] private InputActionReference pointerPositionAction;

        public enum Mode { None, Build, Sell }
        public Mode CurrentMode { get; private set; } = Mode.None;

        public event Action<Mode> OnModeChanged;

        private void OnEnable()
        {
            if (toggleBuildModeAction != null)
            {
                toggleBuildModeAction.action.performed += OnToggleBuild;
                toggleBuildModeAction.action.Enable();
            }
            if (toggleSellModeAction != null)
            {
                toggleSellModeAction.action.performed += OnToggleSell;
                toggleSellModeAction.action.Enable();
            }
            if (undoAction != null)
            {
                undoAction.action.performed += OnUndo;
                undoAction.action.Enable();
            }
            if (redoAction != null)
            {
                redoAction.action.performed += OnRedo;
                redoAction.action.Enable();
            }
            if (clickAction != null)
            {
                clickAction.action.performed += OnClick;
                clickAction.action.Enable();
            }
            if (pointerPositionAction != null)
            {
                pointerPositionAction.action.Enable();
            }
        }

        private void OnDisable()
        {
            if (toggleBuildModeAction != null)
            {
                toggleBuildModeAction.action.performed -= OnToggleBuild;
                toggleBuildModeAction.action.Disable();
            }
            if (toggleSellModeAction != null)
            {
                toggleSellModeAction.action.performed -= OnToggleSell;
                toggleSellModeAction.action.Disable();
            }
            if (undoAction != null)
            {
                undoAction.action.performed -= OnUndo;
                undoAction.action.Disable();
            }
            if (redoAction != null)
            {
                redoAction.action.performed -= OnRedo;
                redoAction.action.Disable();
            }
            if (clickAction != null)
            {
                clickAction.action.performed -= OnClick;
                clickAction.action.Disable();
            }
            if (pointerPositionAction != null)
            {
                pointerPositionAction.action.Disable();
            }
        }
        
        private void OnToggleBuild(InputAction.CallbackContext context) => SetMode(CurrentMode == Mode.Build ? Mode.None : Mode.Build);
        private void OnToggleSell(InputAction.CallbackContext context) => SetMode(CurrentMode == Mode.Sell ? Mode.None : Mode.Sell);
        
        private void OnUndo(InputAction.CallbackContext context) 
        {
            CommandInvoker.UndoCommand();
            Debug.Log("[Command] Undo");
        }
        
        private void OnRedo(InputAction.CallbackContext context) 
        {
            CommandInvoker.RedoCommand();
            Debug.Log("[Command] Redo");
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            HandleMouseClick();
        }
        
        public void SetMode(Mode newMode)
        {
            CurrentMode = newMode;
            OnModeChanged?.Invoke(CurrentMode);
            Debug.Log($"[Builder] Tryb ustawiony na: {CurrentMode}");
        }

        private void HandleMouseClick()
        {
            if (CurrentMode == Mode.None) return;

            Camera cam = Camera.main;
            if (cam == null || pointerPositionAction == null) return;
            
            Vector2 pointerPos = pointerPositionAction.action.ReadValue<Vector2>();
            Ray ray = cam.ScreenPointToRay(pointerPos);

            if (CurrentMode == Mode.Build)
            {
                if (Physics.Raycast(ray, out RaycastHit hit, 2000f, placementMask, QueryTriggerInteraction.Ignore))
                {
                    BuildPlatform buildPlatform = hit.collider.GetComponentInParent<BuildPlatform>();
                    if (buildPlatform != null)
                    {
                        Vector3 spawnPosition = hit.point + Vector3.up * placementOffsetY;
                        ICommand buildCmd = new BuildTurretCommand(turretPrefab, spawnPosition, turretBuildCost);
                        CommandInvoker.ExecuteCommand(buildCmd);

                        if (!keepBuildModeAfterPlacement) SetMode(Mode.None);
                    }
                }
            }
            else if (CurrentMode == Mode.Sell)
            {
                if (Physics.Raycast(ray, out RaycastHit hit, 2000f, ~0, QueryTriggerInteraction.Ignore))
                {
                    AutoTurretStandalone turret = hit.collider.GetComponentInParent<AutoTurretStandalone>();
                    if (turret != null)
                    {
                        ICommand sellCommand = new SellTurretCommand(turret.gameObject, turretSellRefund);
                        CommandInvoker.ExecuteCommand(sellCommand);
                    }
                }
            }
        }
    }
}