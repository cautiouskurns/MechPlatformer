using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    // Reference to key systems
    [SerializeField] private GameManager gameManagerPrefab;
    [SerializeField] private EventBus eventBusPrefab;
    [SerializeField] private InputManager inputManagerPrefab;
    
    private void Awake()
    {
        // Initialize core systems if not already present
        InitializeCoreSystem<GameManager>(gameManagerPrefab);
        InitializeCoreSystem<EventBus>(eventBusPrefab);
        InitializeCoreSystem<InputManager>(inputManagerPrefab);
        
        // Initialize setup components
        SetupScene();
        
        // Log successful initialization
        Debug.Log("Game bootstrapper initialization complete");
    }
    
    private void SetupScene()
    {
        // Set up scene connections
        AddSetupComponent<SetupSceneConnections>();
        
        // Set up layers and tags
        AddSetupComponent<LayerAndTagSetup>();
        
        // Set up UI elements
        AddSetupComponent<UISetup>();
        
        // Set up placeholder graphics
        AddSetupComponent<SetupPlaceholders>();
    }
    
    private T AddSetupComponent<T>() where T : Component
    {
        // Check if component already exists
        T existingComponent = GetComponent<T>();
        if (existingComponent != null)
        {
            return existingComponent;
        }
        
        // Add component
        T component = gameObject.AddComponent<T>();
        Debug.Log($"Added setup component: {typeof(T).Name}");
        return component;
    }
    
    private T InitializeCoreSystem<T>(T prefab) where T : MonoBehaviour
    {
        // Check if system already exists
        T existingSystem = FindAnyObjectByType<T>();
        if (existingSystem != null)
        {
            return existingSystem;
        }
        
        // Create new instance if prefab is assigned
        if (prefab != null)
        {
            T instance = Instantiate(prefab);
            return instance;
        }
        
        // Create a new GameObject with the component if no prefab is assigned
        GameObject systemObject = new GameObject(typeof(T).Name);
        T newSystem = systemObject.AddComponent<T>();
        Debug.Log($"Created new {typeof(T).Name} instance");
        return newSystem;
    }
}
