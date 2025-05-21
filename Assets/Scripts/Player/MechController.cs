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
        
        // Create projectile
        if (firePoint != null && projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            
            // Set up projectile with damage value
            Projectile projectileComponent = projectile.GetComponent<Projectile>();
            if (projectileComponent != null)
            {
                projectileComponent.Initialize(transform.localScale.x, weaponDamage);
            }
        }
        else
        {
            Debug.LogWarning("MechController: firePoint or projectilePrefab is missing!");
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
