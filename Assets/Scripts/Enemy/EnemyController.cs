using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private int currentHealth;
    
    // Properties
    public bool IsDead => currentHealth <= 0;
    
    private void Start()
    {
        // Initialize health
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage. Health: {currentHealth}/{maxHealth}");
        
        if (IsDead)
        {
            Die();
        }
    }
    
    private void Die()
    {
        Debug.Log("Enemy destroyed");
        
        // Notify event system if available
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Publish(new EnemyDestroyedEvent { Enemy = this });
        }
        
        // Destroy this enemy
        Destroy(gameObject);
    }
}

// Event class for enemy destruction
public class EnemyDestroyedEvent
{
    public EnemyController Enemy;
}
