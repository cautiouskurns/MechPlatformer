using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private HUDView hudView;
    
    private void Awake()
    {
        // Find HUDView if not assigned in inspector
        if (hudView == null)
        {
            hudView = GetComponentInChildren<HUDView>();
        }
        
        if (hudView == null)
        {
            Debug.LogError("HUDController: No HUDView found!");
        }
    }
    
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (hudView != null)
        {
            hudView.SetHealthDisplay(currentHealth, maxHealth);
        }
    }
    
    public void UpdateEnergyBar(int currentEnergy, int maxEnergy)
    {
        if (hudView != null)
        {
            hudView.SetEnergyDisplay(currentEnergy, maxEnergy);
        }
    }
}
