using UnityEngine;

public class MechController : MonoBehaviour, IAttacker
{
    [Header("Weapon Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private int weaponDamage = 10;
    [SerializeField] private int energyCostPerShot = 5;
    
    // Component references
    private PlayerController playerController;
    
    // Firing state
    private float nextFireTime = 0f;
    
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        
        // Ensure firePoint is assigned
        if (firePoint == null)
        {
            // First, check if we have a "FirePoint" child
            Transform firePointTransform = transform.Find("FirePoint");
            
            if (firePointTransform != null)
            {
                firePoint = firePointTransform;
                Debug.Log("Found and assigned existing FirePoint child");
            }
            else
            {
                // Try to find FirePoint in the scene
                GameObject firePointObject = GameObject.Find("FirePoint");
                if (firePointObject != null)
                {
                    firePoint = firePointObject.transform;
                    Debug.Log("Found and assigned FirePoint from scene");
                }
                else
                {
                    // Create a new FirePoint as a child
                    GameObject newFirePoint = new GameObject("FirePoint");
                    newFirePoint.transform.SetParent(transform);
                    newFirePoint.transform.localPosition = new Vector3(0.5f, 0, 0);
                    firePoint = newFirePoint.transform;
                    Debug.Log("Created and assigned new FirePoint");
                }
            }
        }
        
        // Ensure projectilePrefab is assigned
        if (projectilePrefab == null)
        {
            // Try to find a Projectile object in the scene
            GameObject projectileObject = GameObject.Find("Projectile");
            if (projectileObject != null)
            {
                projectilePrefab = projectileObject;
                Debug.Log("Found and assigned Projectile from scene");
            }
            else
            {
                // Create a simple projectile prefab
                CreateDefaultProjectilePrefab();
                Debug.Log("Created default projectile prefab");
            }
        }
    }
    
    private void CreateDefaultProjectilePrefab()
    {
        // Create a simple projectile GameObject
        GameObject newProjectile = new GameObject("Projectile");
        newProjectile.transform.position = new Vector3(0, -10, 0); // Start off-screen
        
        // Add required components
        SpriteRenderer spriteRenderer = newProjectile.AddComponent<SpriteRenderer>();
        newProjectile.AddComponent<BoxCollider2D>().isTrigger = true;
        Rigidbody2D rb = newProjectile.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        // Add projectile component
        newProjectile.AddComponent<Projectile>();
        
        // Create a simple circular sprite if possible
        if (spriteRenderer != null)
        {
            // Check if we can access the PlaceholderSprites utility
            System.Type placeholdersType = System.Type.GetType("PlaceholderSprites");
            if (placeholdersType != null)
            {
                // Try to use PlaceholderSprites to create a sprite
                System.Reflection.MethodInfo method = placeholdersType.GetMethod("CreateCircleSprite");
                if (method != null)
                {
                    Sprite sprite = method.Invoke(null, new object[] { Color.yellow }) as Sprite;
                    if (sprite != null)
                    {
                        spriteRenderer.sprite = sprite;
                    }
                }
            }
        }
        
        // Set as prefab
        projectilePrefab = newProjectile;
        
        // Hide it initially
        newProjectile.SetActive(false);
    }
    
    private void Update()
    {
        // Check if game is paused
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        {
            return;
        }
        
        // Handle firing input
        if (InputManager.Instance != null)
        {
            if (InputManager.Instance.FirePressed)
            {
                Fire();
            }
        }
        else
        {
            // Fallback to direct input
            if (Input.GetButtonDown("Fire1"))
            {
                Fire();
            }
        }
    }
    
    public void Fire()
    {
        // Check fire cooldown
        if (Time.time < nextFireTime) return;
        
        // Check energy cost
        if (playerController != null && !playerController.UseEnergy(energyCostPerShot))
        {
            // Not enough energy
            return;
        }
        
        // Set cooldown for next fire
        nextFireTime = Time.time + fireRate;
        
        // Check for missing firePoint and try to fix it
        if (firePoint == null)
        {
            Debug.LogWarning("MechController: firePoint is missing! Attempting to create one...");
            GameObject newFirePoint = new GameObject("FirePoint");
            newFirePoint.transform.SetParent(transform);
            newFirePoint.transform.localPosition = new Vector3(0.5f, 0, 0);
            firePoint = newFirePoint.transform;
        }
        
        // Check for missing projectilePrefab and try to fix it
        if (projectilePrefab == null)
        {
            Debug.LogWarning("MechController: projectilePrefab is missing! Attempting to find or create one...");
            GameObject existingProjectile = GameObject.Find("Projectile");
            if (existingProjectile != null)
            {
                projectilePrefab = existingProjectile;
            }
            else
            {
                CreateDefaultProjectilePrefab();
            }
        }
        
        // Create projectile - after attempting to fix any missing references
        if (firePoint != null && projectilePrefab != null)
        {
            // Make sure the projectile is active before instantiation
            bool wasActive = projectilePrefab.activeSelf;
            projectilePrefab.SetActive(true);
            
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            
            // Reset original projectile's active state
            projectilePrefab.SetActive(wasActive);
            
            // Make sure the new projectile is active
            projectile.SetActive(true);
            
            // Set up projectile with damage value
            Projectile projectileComponent = projectile.GetComponent<Projectile>();
            if (projectileComponent != null)
            {
                projectileComponent.Initialize(transform.localScale.x, weaponDamage);
            }
            else
            {
                Debug.LogWarning("MechController: Instantiated projectile is missing Projectile component!");
                
                // Add the component if missing
                projectileComponent = projectile.AddComponent<Projectile>();
                projectileComponent.Initialize(transform.localScale.x, weaponDamage);
            }
        }
        else
        {
            Debug.LogError("MechController: Failed to create firePoint or projectilePrefab!");
        }
    }
    
    public void Attack()
    {
        Fire();
    }
    
    public int GetAttackDamage()
    {
        return weaponDamage;
    }
}
