using UnityEngine;
using System.Reflection;

/// <summary>
/// Sets up placeholder graphics for development testing
/// </summary>
public class SetupPlaceholders : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreateMissingObjects();
        SetupPlayerSprite();
        SetupEnemySprite();
        SetupProjectileSprite();
        SetupGroundSprite();
        SetupFirePointSprite();
        SetupGroundCheckSprite();
    }
    
    /// <summary>
    /// Create any missing objects needed for testing
    /// </summary>
    void CreateMissingObjects()
    {
        // Create Ground if it doesn't exist
        if (GameObject.Find("Ground") == null)
        {
            GameObject ground = new GameObject("Ground");
            ground.transform.position = new Vector3(0, -4, 0);
            ground.transform.localScale = new Vector3(20, 1, 1);
            ground.AddComponent<SpriteRenderer>();
            ground.AddComponent<BoxCollider2D>();
            ground.tag = "Ground";
        }
        
        // Create Player if it doesn't exist
        if (GameObject.Find("Player") == null)
        {
            GameObject player = new GameObject("Player");
            player.transform.position = new Vector3(0, 2, 0);
            player.AddComponent<SpriteRenderer>();
            player.AddComponent<BoxCollider2D>();
            player.AddComponent<Rigidbody2D>();
            player.AddComponent<PlayerController>();
            player.AddComponent<PlatformerController>();
            player.AddComponent<MechController>();
            player.tag = "Player";
            
            // Add GroundCheck
            Transform existingGroundCheck = player.transform.Find("GroundCheck");
            if (existingGroundCheck == null)
            {
                GameObject groundCheck = new GameObject("GroundCheck");
                groundCheck.transform.SetParent(player.transform);
                groundCheck.transform.localPosition = new Vector3(0, -0.5f, 0);
                Debug.Log("Created GroundCheck as child of Player");
                
                // Directly assign the reference to any PlatformerController
                PlatformerController platformerController = player.GetComponent<PlatformerController>();
                if (platformerController != null)
                {
                    // Use reflection directly to set the private field
                    FieldInfo field = platformerController.GetType().GetField("groundCheck", 
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    
                    if (field != null)
                    {
                        field.SetValue(platformerController, groundCheck.transform);
                        Debug.Log("Directly assigned groundCheck to PlatformerController");
                    }
                }
            }
            
            // Add FirePoint
            Transform existingFirePoint = player.transform.Find("FirePoint");
            if (existingFirePoint == null)
            {
                GameObject firePoint = new GameObject("FirePoint");
                firePoint.transform.SetParent(player.transform);
                firePoint.transform.localPosition = new Vector3(0.5f, 0, 0);
                Debug.Log("Created FirePoint as child of Player");
                
                // Directly assign the reference to any MechController
                MechController mechController = player.GetComponent<MechController>();
                if (mechController != null)
                {
                    // Use reflection directly to set the private field
                    FieldInfo field = mechController.GetType().GetField("firePoint", 
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    
                    if (field != null)
                    {
                        field.SetValue(mechController, firePoint.transform);
                        Debug.Log("Directly assigned firePoint to MechController");
                    }
                }
            }
        }
        
        // Create Enemy if it doesn't exist
        if (GameObject.Find("Enemy") == null)
        {
            GameObject enemy = new GameObject("Enemy");
            enemy.transform.position = new Vector3(5, 0, 0);
            enemy.AddComponent<SpriteRenderer>();
            enemy.AddComponent<BoxCollider2D>();
            enemy.AddComponent<EnemyController>();
            enemy.tag = "Enemy";
        }
        
        // Create Projectile if it doesn't exist
        if (GameObject.Find("Projectile") == null)
        {
            GameObject projectile = new GameObject("Projectile");
            projectile.transform.position = new Vector3(0, -10, 0); // Out of view initially
            projectile.AddComponent<SpriteRenderer>();
            projectile.AddComponent<BoxCollider2D>();
            projectile.AddComponent<Rigidbody2D>();
            projectile.AddComponent<Projectile>();
            projectile.tag = "Projectile";
            projectile.SetActive(false); // Hidden initially
        }
    }
    
    void SetupPlayerSprite()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            Debug.Log("Setting up Player placeholder sprite");
            Sprite playerSprite = PlaceholderSprites.CreateSquareSprite(Color.blue);
            PlaceholderSprites.ApplySprite(player, playerSprite);
            
            // Set size to match sprite
            BoxCollider2D collider = player.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.size = new Vector2(1, 1);
            }
            
            // Make sure rigidbody is configured correctly
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.gravityScale = 3.0f;
                rb.mass = 1.0f;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            }
        }
    }
    
    void SetupEnemySprite()
    {
        GameObject enemy = GameObject.Find("Enemy");
        if (enemy != null)
        {
            Debug.Log("Setting up Enemy placeholder sprite");
            Sprite enemySprite = PlaceholderSprites.CreateSquareSprite(Color.red);
            PlaceholderSprites.ApplySprite(enemy, enemySprite);
            
            // Set size to match sprite
            BoxCollider2D collider = enemy.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.size = new Vector2(1, 1);
            }
        }
    }
    
    void SetupProjectileSprite()
    {
        GameObject projectile = GameObject.Find("Projectile");
        if (projectile == null)
        {
            // Create a projectile if it doesn't exist
            projectile = new GameObject("Projectile");
            projectile.transform.position = new Vector3(0, -10, 0); // Out of view initially
            projectile.AddComponent<SpriteRenderer>();
            projectile.AddComponent<BoxCollider2D>();
            projectile.AddComponent<Rigidbody2D>();
            projectile.AddComponent<Projectile>();
            projectile.tag = "Projectile";
            Debug.Log("Created new Projectile GameObject");
        }
        
        Debug.Log("Setting up Projectile placeholder sprite");
        Sprite projectileSprite = PlaceholderSprites.CreateCircleSprite(Color.yellow);
        PlaceholderSprites.ApplySprite(projectile, projectileSprite);
            
        // Configure components
        BoxCollider2D collider = projectile.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.size = new Vector2(0.5f, 0.5f);
            collider.isTrigger = true;
        }
        else
        {
            collider = projectile.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(0.5f, 0.5f);
            collider.isTrigger = true;
        }
        
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        else
        {
            rb = projectile.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        
        // Make sure it has a Projectile component
        if (projectile.GetComponent<Projectile>() == null)
        {
            projectile.AddComponent<Projectile>();
        }
        
        // Hide it initially - it's just for creating the prefab
        projectile.SetActive(false);
    }
    
    void SetupGroundSprite()
    {
        GameObject ground = GameObject.Find("Ground");
        if (ground != null)
        {
            Debug.Log("Setting up Ground placeholder sprite");
            Sprite groundSprite = PlaceholderSprites.CreateSquareSprite(Color.green);
            PlaceholderSprites.ApplySprite(ground, groundSprite);
            
            // Set up the collider based on sprite size
            BoxCollider2D collider = ground.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.size = new Vector2(1, 1);
            }
            
            // Set up the tag
            ground.tag = "Ground";
            ground.layer = LayerMask.NameToLayer("Default");
        }
    }
    
    void SetupFirePointSprite()
    {
        GameObject firePoint = GameObject.Find("FirePoint");
        if (firePoint != null)
        {
            // Add a small sprite renderer to visualize the fire point in editor
            SpriteRenderer renderer = firePoint.GetComponent<SpriteRenderer>();
            if (renderer == null)
            {
                renderer = firePoint.AddComponent<SpriteRenderer>();
            }
            
            Sprite firePointSprite = PlaceholderSprites.CreateCircleSprite(new Color(1f, 0.5f, 0f, 0.5f));
            renderer.sprite = firePointSprite;
            renderer.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        }
    }
    
    void SetupGroundCheckSprite()
    {
        GameObject groundCheck = GameObject.Find("GroundCheck");
        if (groundCheck != null)
        {
            // Add a small sprite renderer to visualize the ground check in editor
            SpriteRenderer renderer = groundCheck.GetComponent<SpriteRenderer>();
            if (renderer == null)
            {
                renderer = groundCheck.AddComponent<SpriteRenderer>();
            }
            
            Sprite groundCheckSprite = PlaceholderSprites.CreateCircleSprite(new Color(0f, 1f, 0f, 0.5f));
            renderer.sprite = groundCheckSprite;
            renderer.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        }
    }
}


