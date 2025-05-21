using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float direction = 1f; // 1 = right, -1 = left
    
    private Rigidbody2D rb;
    private int damage = 10;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Configure rigidbody for a projectile
        if (rb != null)
        {
            rb.gravityScale = 0; // No gravity on projectile
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Don't rotate
            rb.interpolation = RigidbodyInterpolation2D.Interpolate; // Smoother movement
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Better collision detection
        }
        
        // Ensure collider is set as trigger
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }
    
    private void Start()
    {
        // Set projectile velocity
        rb.linearVelocity = new Vector2(direction * speed, 0);
        
        // Destroy after lifetime
        Destroy(gameObject, lifetime);
    }
    
    public void Initialize(float characterScaleX, int damageAmount)
    {
        // Set direction based on character's facing direction
        direction = Mathf.Sign(characterScaleX);
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(direction * speed, 0);
        }
        
        // Set damage amount from weapon
        damage = damageAmount;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if collided with damageable object
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // Apply damage
            damageable.TakeDamage(damage);
            
            // Destroy projectile
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            // Hit terrain/environment
            Destroy(gameObject);
        }
    }
}
