using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Editor tool to create prefabs from GameObjects in the scene
/// </summary>
public class CreatePrefabs : EditorWindow
{
    [MenuItem("MechPlatformer/Create Prefabs")]
    static void CreateGamePrefabs()
    {
        string prefabsFolder = "Assets/Prefabs";
        
        // Create directory if it doesn't exist
        if (!Directory.Exists(prefabsFolder))
        {
            Directory.CreateDirectory(prefabsFolder);
        }
        
        // Create Player prefab
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            string playerPrefabPath = prefabsFolder + "/PlayerPrefab.prefab";
            SavePrefab(player, playerPrefabPath);
        }
        
        // Create Enemy prefab
        GameObject enemy = GameObject.Find("Enemy");
        if (enemy != null)
        {
            string enemyPrefabPath = prefabsFolder + "/EnemyPrefab.prefab";
            SavePrefab(enemy, enemyPrefabPath);
        }
        
        // Create Projectile prefab
        GameObject projectile = GameObject.Find("Projectile");
        if (projectile != null)
        {
            string projectilePrefabPath = prefabsFolder + "/ProjectilePrefab.prefab";
            SavePrefab(projectile, projectilePrefabPath);
        }
        
        // Create GameManager prefab
        GameObject gameManager = GameObject.Find("GameManager");
        if (gameManager != null)
        {
            string gameManagerPrefabPath = prefabsFolder + "/GameManagerPrefab.prefab";
            SavePrefab(gameManager, gameManagerPrefabPath);
        }
        
        // Create EventBus prefab
        GameObject eventBus = GameObject.Find("EventBus");
        if (eventBus != null)
        {
            string eventBusPrefabPath = prefabsFolder + "/EventBusPrefab.prefab";
            SavePrefab(eventBus, eventBusPrefabPath);
        }
        
        // Create UI prefab
        GameObject ui = GameObject.Find("UI");
        if (ui != null)
        {
            string uiPrefabPath = prefabsFolder + "/UIPrefab.prefab";
            SavePrefab(ui, uiPrefabPath);
        }
        
        // Refresh the AssetDatabase
        AssetDatabase.Refresh();
        Debug.Log("Prefabs created successfully!");
    }
    
    static void SavePrefab(GameObject gameObject, string path)
    {
        // Check if the prefab already exists
        bool prefabExists = AssetDatabase.LoadAssetAtPath<GameObject>(path) != null;
        
        if (prefabExists)
        {
            // Update existing prefab
            PrefabUtility.SaveAsPrefabAsset(gameObject, path);
            Debug.Log($"Updated prefab: {path}");
        }
        else
        {
            // Create new prefab
            PrefabUtility.SaveAsPrefabAsset(gameObject, path);
            Debug.Log($"Created prefab: {path}");
        }
    }
    
    [MenuItem("MechPlatformer/Open Prefab Creator")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow<CreatePrefabs>("Prefab Creator");
    }
    
    void OnGUI()
    {
        GUILayout.Label("Create Prefabs from Scene GameObjects", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Create All Prefabs"))
        {
            CreateGamePrefabs();
        }
    }
}
