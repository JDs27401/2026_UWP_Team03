using UnityEngine;

namespace Build
{
    public class BuildUIPlaceholder : MonoBehaviour
    {
        [SerializeField] private MouseTurretBuilder mouseTurretBuilder;
        [SerializeField] private Vector2 panelPosition = new Vector2(12f, 120f);
        [SerializeField] private Vector2 panelSize = new Vector2(360f, 180f);

        private void Awake()
        {
            if (mouseTurretBuilder == null)
            {
                mouseTurretBuilder = FindObjectOfType<MouseTurretBuilder>();
            }
        }

        private void OnGUI()
        {
            if (mouseTurretBuilder == null)
            {
                GUI.Box(new Rect(panelPosition.x, panelPosition.y, panelSize.x, panelSize.y), "Build UI Placeholder");
                GUI.Label(new Rect(panelPosition.x + 12f, panelPosition.y + 30f, panelSize.x - 24f, 40f), "Brak komponentu MouseTurretBuilder na scenie.");
                return;
            }

            Rect panelRect = new Rect(panelPosition.x, panelPosition.y, panelSize.x, panelSize.y);
            GUI.Box(panelRect, "Build/Sell UI Placeholder");

            string buildModeText = mouseTurretBuilder.IsBuildModeEnabled ? "TRYB BUDOWY: WLACZONY" : "TRYB BUDOWY: WYLACZONY";
            string sellModeText = mouseTurretBuilder.IsSellModeEnabled ? "TRYB SPRZEDAZY: WLACZONY" : "TRYB SPRZEDAZY: WYLACZONY";

            GUI.Label(new Rect(panelRect.x + 12f, panelRect.y + 26f, panelRect.width - 24f, 20f), buildModeText);
            GUI.Label(new Rect(panelRect.x + 12f, panelRect.y + 46f, panelRect.width - 24f, 20f), sellModeText);

            if (GUI.Button(new Rect(panelRect.x + 12f, panelRect.y + 72f, 200f, 28f), "Przelacz budowanie (B)"))
            {
                mouseTurretBuilder.ToggleBuildMode();
            }

            if (GUI.Button(new Rect(panelRect.x + 12f, panelRect.y + 106f, 200f, 28f), "Przelacz sprzedaz"))
            {
                mouseTurretBuilder.ToggleSellMode();
            }

            GUI.Label(new Rect(panelRect.x + 12f, panelRect.y + 140f, panelRect.width - 24f, 18f), "Build: LPM na platformie. Sell: LPM na wiezyczce.");
        }
    }
}
