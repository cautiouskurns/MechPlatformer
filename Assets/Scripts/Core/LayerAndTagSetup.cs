using UnityEngine;

/// <summary>
/// Configures the project's layers and tags
/// </summary>
public class LayerAndTagSetup : MonoBehaviour
{
    private void Awake()
    {
        ConfigureGameObjects();
    }
    
    private void ConfigureGameObjects()
    {
        // Set Ground layer for ground object
        GameObject ground = GameObject.Find("Ground");
        if (ground != null)
        {
            ground.layer = LayerMask.NameToLayer("Default"); // Using Default as ground layer for now
            ground.tag = "Ground";
            
            // Add a BoxCollider2D if not present
            if (ground.GetComponent<BoxCollider2D>() == null)
            {
                BoxCollider2D collider = ground.AddComponent<BoxCollider2D>();
                // Set collider size
                SpriteRenderer renderer = ground.GetComponent<SpriteRenderer>();
                if (renderer != null && renderer.sprite != null)
                {
                    collider.size = new Vector2(renderer.sprite.bounds.size.x, renderer.sprite.bounds.size.y);
                }
                else
                {
                    collider.size = new Vector2(20, 1); // Default size
                }
            }
        }
        
        // Set Player tag
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            player.tag = "Player";
        }
        
        // Set Enemy tag
        GameObject enemy = GameObject.Find("Enemy");
        if (enemy != null)
        {
            enemy.tag = "Enemy";
            
            // Implement IDamageable if not already done
            if (enemy.GetComponent<EnemyController>() == null)
            {
                enemy.AddComponent<EnemyController>();
            }
        }
        
        // Set Projectile tag
        GameObject projectile = GameObject.Find("Projectile");
        if (projectile != null)
        {
            projectile.tag = "Projectile";
        }
        
        // Set GroundCheck layer
        GameObject groundCheck = GameObject.Find("GroundCheck");
        if (groundCheck != null)
        {
            groundCheck.layer = LayerMask.NameToLayer("Ignore Raycast"); // Use Ignore Raycast layer for ground check
        }
    }
}
