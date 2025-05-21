using UnityEngine;

/// <summary>
/// Creates prefabs from scene GameObjects at runtime (for development only)
/// This works around the Editor-only limitations of PrefabUtility
/// </summary>
public class RuntimePrefabCreator : MonoBehaviour
{
    [SerializeField] private string prefabsPath = "Assets/Prefabs";
    
    private void Start()
    {
        // Wait a frame to make sure everything is initialized
        Invoke("CreatePrefabs", 0.5f);
    }
    
    public void CreatePrefabs()
    {
        try
        {
            Debug.Log("Creating prefabs from scene objects...");
            
            // Set up player
            SetupPlayer();
            
            // Set up enemy
            SetupEnemy();
            
            // Set up projectile
            SetupProjectile();
            
            // Set up UI
            SetupUI();
            
            // Set up CombatSystem
            SetupCombatSystem();
            
            Debug.Log("All runtime prefabs and references set up successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error creating prefabs: " + e.Message);
        }
    }
    
    private void SetupPlayer()
    {
        // Get player GameObject
        GameObject player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogWarning("Player GameObject not found");
            return;
        }
        
        Debug.Log("Setting up Player GameObject");
        
        // Connect platformer controller references
        SetupPlayerPlatformerController(player);
        
        // Connect mech controller references
        SetupPlayerMechController(player);
        
        // Make sure player has required components
        EnsurePlayerComponents(player);
    }
    
    private void SetupPlayerPlatformerController(GameObject player)
    {
        // Set GroundCheck reference
        GameObject groundCheck = GameObject.Find("GroundCheck");
        PlatformerController platformerController = player.GetComponent<PlatformerController>();
        
        if (groundCheck != null && platformerController != null)
        {
            SetPrivateField(platformerController, "groundCheck", groundCheck.transform);
            SetPrivateField(platformerController, "groundLayers", LayerMask.GetMask("Default"));
            
            Debug.Log("Set up groundCheck reference in PlatformerController");
        }
        else
        {
            Debug.LogWarning("Could not set up PlatformerController references - missing components");
        }
    }
    
    private void SetupPlayerMechController(GameObject player)
    {
        // Set the projectile as a reference in MechController
        GameObject firePoint = GameObject.Find("FirePoint");
        GameObject projectile = GameObject.Find("Projectile");
        MechController mechController = player.GetComponent<MechController>();
        
        if (firePoint != null && projectile != null && mechController != null)
        {
            // Set firePoint reference
            SetPrivateField(mechController, "firePoint", firePoint.transform);
            
            // Set projectile prefab reference
            SetPrivateField(mechController, "projectilePrefab", projectile);
            
            Debug.Log("Set up firePoint and projectilePrefab references in MechController");
        }
        else
        {
            Debug.LogWarning("Could not set up MechController references - missing components");
        }
    }
    
    private void EnsurePlayerComponents(GameObject player)
    {
        // Ensure player has necessary components
        if (!player.GetComponent<BoxCollider2D>())
        {
            player.AddComponent<BoxCollider2D>();
            Debug.Log("Added BoxCollider2D to Player");
        }
        
        // Ensure rigidbody settings are correct
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            Debug.Log("Configured Rigidbody2D settings on Player");
        }
    }
    
    private void SetupEnemy()
    {
        // Get enemy GameObject
        GameObject enemy = GameObject.Find("Enemy");
        if (enemy == null)
        {
            Debug.LogWarning("Enemy GameObject not found");
            return;
        }
        
        Debug.Log("Setting up Enemy GameObject");
        
        // Ensure enemy has necessary components
        if (!enemy.GetComponent<BoxCollider2D>())
        {
            enemy.AddComponent<BoxCollider2D>();
            Debug.Log("Added BoxCollider2D to Enemy");
        }
    }
    
    private void SetupProjectile()
    {
        // Get projectile GameObject
        GameObject projectile = GameObject.Find("Projectile");
        if (projectile == null)
        {
            Debug.LogWarning("Projectile GameObject not found");
            return;
        }
        
        Debug.Log("Setting up Projectile GameObject");
        
        // Ensure proper projectile configuration
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        if (projectileComponent == null)
        {
            projectileComponent = projectile.AddComponent<Projectile>();
            Debug.Log("Added Projectile component to Projectile GameObject");
        }
    }
    
    private void SetupUI()
    {
        // Connect HUDController to HUDView
        GameObject hud = GameObject.Find("HUD");
        if (hud != null)
        {
            HUDController hudController = hud.GetComponent<HUDController>();
            HUDView hudView = hud.GetComponent<HUDView>();
            
            if (hudController != null && hudView != null)
            {
                SetPrivateField(hudController, "hudView", hudView);
                Debug.Log("Connected HUDController to HUDView");
            }
        }
    }
    
    private void SetupCombatSystem()
    {
        // Setup CombatSystem references
        GameObject combatSystem = GameObject.Find("CombatSystem");
        if (combatSystem != null)
        {
            Debug.Log("Found CombatSystem GameObject");
        }
    }
    
    private void SetPrivateField(object target, string fieldName, object value)
    {
        if (target == null)
        {
            Debug.LogError($"Target is null when trying to set {fieldName}");
            return;
        }
        
        System.Reflection.FieldInfo field = target.GetType().GetField(fieldName, 
            System.Reflection.BindingFlags.NonPublic | 
            System.Reflection.BindingFlags.Public | 
            System.Reflection.BindingFlags.Instance);
            
        if (field != null)
        {
            field.SetValue(target, value);
        }
        else
        {
            Debug.LogError($"Field {fieldName} not found on {target.GetType().Name}");
        }
    }
}
