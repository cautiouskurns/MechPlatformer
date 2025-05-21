using UnityEngine;

/// <summary>
/// Main entry point for the game - attach this to a GameObject in the scene
/// </summary>
public class MechPlatformerMain : MonoBehaviour
{
    // References to required components
    [Header("Core Components")]
    [SerializeField] private GameBootstrapper gameBootstrapper;
    [SerializeField] private SetupPlaceholders placeholderSetup;
    [SerializeField] private RuntimePrefabCreator prefabCreator;
    [SerializeField] private UISetup uiSetup;
    [SerializeField] private LayerAndTagSetup layerAndTagSetup;
    
    private void Awake()
    {
        Debug.Log("MechPlatformer: Game starting");
        
        // Check for required components and add them if missing
        EnsureComponentExists<GameBootstrapper>(ref gameBootstrapper);
        EnsureComponentExists<SetupPlaceholders>(ref placeholderSetup);
        EnsureComponentExists<RuntimePrefabCreator>(ref prefabCreator);
        EnsureComponentExists<UISetup>(ref uiSetup);
        EnsureComponentExists<LayerAndTagSetup>(ref layerAndTagSetup);
    }
    
    private void Start()
    {
        // Additional initialization can go here
    }
    
    /// <summary>
    /// Ensures that the specified component exists on this GameObject
    /// </summary>
    private void EnsureComponentExists<T>(ref T componentRef) where T : Component
    {
        if (componentRef == null)
        {
            componentRef = GetComponent<T>();
            if (componentRef == null)
            {
                componentRef = gameObject.AddComponent<T>();
                Debug.Log($"Added missing component: {typeof(T).Name}");
            }
        }
    }
}
