using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDView : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    
    [Header("Energy UI")]
    [SerializeField] private Slider energySlider;
    [SerializeField] private TextMeshProUGUI energyText;
    
    public void SetHealthDisplay(int current, int max)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }
        
        if (healthText != null)
        {
            healthText.text = $"{current}/{max}";
        }
    }
    
    public void SetEnergyDisplay(int current, int max)
    {
        if (energySlider != null)
        {
            energySlider.maxValue = max;
            energySlider.value = current;
        }
        
        if (energyText != null)
        {
            energyText.text = $"{current}/{max}";
        }
    }
}
