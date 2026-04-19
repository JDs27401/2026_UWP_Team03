#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class StandaloneTurretPrefabCreator
{
    private const string TurretPrefabPath = "Assets/Prefabs/StandaloneAutoTurret.prefab";
    private const string ProjectilePrefabPath = "Assets/Prefabs/StandaloneAutoProjectile.prefab";

    [MenuItem("Tools/TowerDefense/Create Standalone Turret Prefabs")]
    public static void CreateOrUpdatePrefabs()
    {
        EnsureProjectilePrefab();
        EnsureTurretPrefab();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Standalone prefab gotowy: Assets/Prefabs/StandaloneAutoTurret.prefab");
    }

    [InitializeOnLoadMethod]
    private static void AutoCreateOnce()
    {
        if (AssetDatabase.LoadAssetAtPath<GameObject>(TurretPrefabPath) == null)
        {
            CreateOrUpdatePrefabs();
        }
    }

    private static void EnsureProjectilePrefab()
    {
        GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(ProjectilePrefabPath);
        if (existing != null)
        {
            return;
        }

        GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        projectile.name = "StandaloneAutoProjectile";
        projectile.transform.localScale = Vector3.one * 0.25f;

        Collider col = projectile.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }

        Rigidbody rb = projectile.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        projectile.AddComponent<AutoProjectileStandalone>();

        PrefabUtility.SaveAsPrefabAsset(projectile, ProjectilePrefabPath);
        Object.DestroyImmediate(projectile);
    }

    private static void EnsureTurretPrefab()
    {
        GameObject root = new GameObject("StandaloneAutoTurret");

        GameObject baseMesh = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        baseMesh.name = "Base";
        baseMesh.transform.SetParent(root.transform, false);
        baseMesh.transform.localScale = new Vector3(0.9f, 0.35f, 0.9f);

        GameObject headPivot = new GameObject("HeadPivot");
        headPivot.transform.SetParent(root.transform, false);
        headPivot.transform.localPosition = new Vector3(0f, 0.75f, 0f);

        GameObject barrel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        barrel.name = "Barrel";
        barrel.transform.SetParent(headPivot.transform, false);
        barrel.transform.localPosition = new Vector3(0f, 0f, 0.65f);
        barrel.transform.localScale = new Vector3(0.35f, 0.25f, 1.2f);

        GameObject firePoint = new GameObject("FirePoint");
        firePoint.transform.SetParent(headPivot.transform, false);
        firePoint.transform.localPosition = new Vector3(0f, 0f, 1.35f);

        SphereCollider trigger = root.AddComponent<SphereCollider>();
        trigger.isTrigger = true;
        trigger.radius = 12f;

        AutoTurretStandalone turret = root.AddComponent<AutoTurretStandalone>();
        SerializedObject so = new SerializedObject(turret);
        so.FindProperty("projectilePrefab").objectReferenceValue = AssetDatabase.LoadAssetAtPath<GameObject>(ProjectilePrefabPath);
        so.FindProperty("firePoint").objectReferenceValue = firePoint.transform;
        so.FindProperty("rotatingPart").objectReferenceValue = headPivot.transform;
        so.ApplyModifiedPropertiesWithoutUndo();

        PrefabUtility.SaveAsPrefabAsset(root, TurretPrefabPath);
        Object.DestroyImmediate(root);
    }
}
#endif

