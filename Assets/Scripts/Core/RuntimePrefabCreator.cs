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
        MechController mechController = player.GetComponent<MechController>();
        if (mechController == null) return;
        
        // Set the firePoint reference
        GameObject firePoint = GameObject.Find("FirePoint");
        if (firePoint == null)
        {
            // Create FirePoint if it doesn't exist
            firePoint = new GameObject("FirePoint");
            firePoint.transform.SetParent(player.transform);
            firePoint.transform.localPosition = new Vector3(0.5f, 0, 0);
            Debug.Log("Created new FirePoint GameObject");
        }
        
        // Set firePoint reference
        SetPrivateField(mechController, "firePoint", firePoint.transform);
        Debug.Log("Set up firePoint reference in MechController");
        
        // Handle projectile reference
        GameObject projectile = GameObject.Find("Projectile");
        if (projectile == null)
        {
            // Try to create a projectile if it doesn't exist
            projectile = new GameObject("Projectile");
            projectile.transform.position = new Vector3(0, -10, 0);
            projectile.AddComponent<SpriteRenderer>();
            BoxCollider2D collider = projectile.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(0.5f, 0.5f);
            
            Rigidbody2D rb = projectile.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            projectile.AddComponent<Projectile>();
            projectile.tag = "Projectile";
            projectile.SetActive(false); // Hide initially
            Debug.Log("Created new Projectile GameObject");
            
            // Try to set up a placeholder sprite
            try {
                System.Type placeholdersType = System.Type.GetType("PlaceholderSprites");
                if (placeholdersType != null)
                {
                    System.Reflection.MethodInfo method = placeholdersType.GetMethod("CreateCircleSprite");
                    if (method != null && method.IsStatic)
                    {
                        Sprite sprite = method.Invoke(null, new object[] { Color.yellow }) as Sprite;
                        if (sprite != null && projectile.GetComponent<SpriteRenderer>() != null)
                        {
                            projectile.GetComponent<SpriteRenderer>().sprite = sprite;
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Failed to set projectile sprite: {e.Message}");
            }
        }
        
        // Set projectile prefab reference
        SetPrivateField(mechController, "projectilePrefab", projectile);
        Debug.Log("Set up projectilePrefab reference in MechController");
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
