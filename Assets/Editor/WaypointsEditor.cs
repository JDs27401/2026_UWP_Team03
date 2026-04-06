using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Waypoints))]
public class WaypointsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Waypoints waypoints = (Waypoints)target;

        GUILayout.Space(15);
        GUILayout.Label("Narzędzie Malowania Ścieżki", EditorStyles.boldLabel);
        GUILayout.Label("1. Zaznacz po lewej obiekt Waypoints.");
        GUILayout.Label("2. Upewnij się, że przypisano Path Prefab.");
        GUILayout.Label("3. PRZYTRZYMAJ SHIFT i KLIKAJ (lub przeciągaj) \nmyszą po Terenie w oknie Scene!");
        
        GUILayout.Space(10);
        if (GUILayout.Button("Wyczyść i zacznij od nowa", GUILayout.Height(30)))
        {
            waypoints.ClearPath();
        }
    }

    void OnSceneGUI()
    {
        Waypoints waypoints = (Waypoints)target;
        Event e = Event.current;

        // Zapobieganie odznaczaniu obiektu Waypoints podczas klikania z Shiftem
        if (e.shift)
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(controlID);
        }

        // Malowanie przytrzymując Lewy Przycisk Myszy + Shift
        if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0 && e.shift)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            
            // Rzucamy raycast żeby trafić w terrain
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Unikamy nakładania kratek jedna na drugą / klikania po kafelkach
                if (hit.collider.gameObject == waypoints.gameObject || hit.collider.transform.IsChildOf(waypoints.transform))
                {
                    return; 
                }

                waypoints.AddPathNode(hit.point);
                EditorUtility.SetDirty(waypoints);
            }
            e.Use();
        }
    }
}
