using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IDamageable
{
    // Health and energy values
    [Header("Player Stats")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxEnergy = 100;
    [SerializeField] private int currentEnergy;
    [SerializeField] private float energyRegenRate = 5f;
    
    // Component references
    private PlatformerController movementController;
    private MechController mechController;
    
    // Properties for UI and other systems
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public int CurrentEnergy => currentEnergy;
    public int MaxEnergy => maxEnergy;
    
    // IDamageable implementation
    public bool IsDead => currentHealth <= 0;
    
    private void Awake()
    {
        // Get component references
        movementController = GetComponent<PlatformerController>();
        mechController = GetComponent<MechController>();
    }
    
    private void Start()
    {
        // Initialize player stats
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        
        // Notify UI of initial values
        UpdateUI();
    }
    
    private void Update()
    {
        // Handle regenerating energy over time
        RegenerateEnergy();
        
        // Process input
        HandleInput();
    }
    
    private void HandleInput()
    {
        // Movement and jumping inputs are handled by PlatformerController
        
        // Attack input is handled by MechController
        if (Input.GetButtonDown("Fire1") && mechController != null)
        {
            mechController.Fire();
        }
    }
    
    private void RegenerateEnergy()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy = Mathf.Min(maxEnergy, currentEnergy + Mathf.CeilToInt(energyRegenRate * Time.deltaTime));
            UpdateUI();
        }
    }
    
    public bool UseEnergy(int amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        
        if (currentHealth <= 0)
        {
            Die();
        }
        
        UpdateUI();
    }
    
    public void Reset()
    {
        // Reset health and energy to max
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        
        // Update UI
        UpdateUI();
        
        // Make sure the player is active
        gameObject.SetActive(true);
    }
    
    private void Die()
    {
        Debug.Log("Player died");
        
        // Notify the event system
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Publish(new PlayerDiedEvent { Player = this });
        }
        
        // Hide the player for now
        gameObject.SetActive(false);
    }
    
    private void UpdateUI()
    {
        // Find and update the HUD
        HUDController hudController = FindAnyObjectByType<HUDController>();
        if (hudController != null)
        {
            hudController.UpdateHealthBar(currentHealth, maxHealth);
            hudController.UpdateEnergyBar(currentEnergy, maxEnergy);
        }
    }
}
