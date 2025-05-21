using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Creates and sets up UI elements for the game
/// </summary>
public class UISetup : MonoBehaviour
{
    private void Start()
    {
        SetupHUD();
    }
    
    private void SetupHUD()
    {
        // Find the main Canvas and HUD
        GameObject canvas = GameObject.Find("UI");
        if (canvas == null)
        {
            canvas = new GameObject("UI");
            Canvas canvasComponent = canvas.AddComponent<Canvas>();
            canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();
        }
        
        // Create HUD if it doesn't exist
        GameObject hud = GameObject.Find("HUD");
        if (hud == null)
        {
            hud = new GameObject("HUD");
            hud.transform.SetParent(canvas.transform, false);
            
            // Add components
            hud.AddComponent<HUDController>();
            hud.AddComponent<HUDView>();
        }
        
        // Get HUDView component
        HUDView hudView = hud.GetComponent<HUDView>();
        
        // Create health bar UI
        CreateHealthBar(hudView);
        
        // Create energy bar UI
        CreateEnergyBar(hudView);
    }
    
    private void CreateHealthBar(HUDView hudView)
    {
        // Create container panel for health bar
        GameObject healthPanel = new GameObject("HealthPanel");
        healthPanel.transform.SetParent(hudView.transform, false);
        
        RectTransform healthPanelRect = healthPanel.AddComponent<RectTransform>();
        healthPanelRect.anchorMin = new Vector2(0, 1);
        healthPanelRect.anchorMax = new Vector2(0.3f, 1);
        healthPanelRect.pivot = new Vector2(0, 1);
        healthPanelRect.anchoredPosition = new Vector2(10, -10);
        healthPanelRect.sizeDelta = new Vector2(0, 30);
        
        // Create health bar slider
        GameObject healthBar = new GameObject("HealthBar");
        healthBar.transform.SetParent(healthPanel.transform, false);
        
        Slider healthSlider = healthBar.AddComponent<Slider>();
        healthSlider.minValue = 0;
        healthSlider.maxValue = 100;
        healthSlider.value = 100;
        
        RectTransform healthBarRect = healthBar.GetComponent<RectTransform>();
        healthBarRect.anchorMin = new Vector2(0, 0);
        healthBarRect.anchorMax = new Vector2(1, 1);
        healthBarRect.sizeDelta = Vector2.zero;
        
        // Create slider background
        GameObject background = new GameObject("Background");
        background.transform.SetParent(healthBar.transform, false);
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f);
        
        RectTransform bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        
        // Create fill area
        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(healthBar.transform, false);
        
        RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
        fillAreaRect.anchorMin = Vector2.zero;
        fillAreaRect.anchorMax = Vector2.one;
        fillAreaRect.offsetMin = new Vector2(5, 0);
        fillAreaRect.offsetMax = new Vector2(-5, 0);
        
        // Create fill
        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(fillArea.transform, false);
        Image fillImage = fill.AddComponent<Image>();
        fillImage.color = Color.red;
        
        RectTransform fillRect = fill.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.sizeDelta = Vector2.zero;
        
        // Create health text
        GameObject healthText = new GameObject("HealthText");
        healthText.transform.SetParent(healthPanel.transform, false);
        
        TextMeshProUGUI healthTMP = healthText.AddComponent<TextMeshProUGUI>();
        healthTMP.text = "100/100";
        healthTMP.alignment = TextAlignmentOptions.Center;
        healthTMP.fontSize = 14;
        healthTMP.color = Color.white;
        
        RectTransform healthTextRect = healthText.GetComponent<RectTransform>();
        healthTextRect.anchorMin = new Vector2(0, 0);
        healthTextRect.anchorMax = new Vector2(1, 1);
        healthTextRect.sizeDelta = Vector2.zero;
        
        // Set up the slider components
        healthSlider.fillRect = fillRect;
        healthSlider.targetGraphic = bgImage;
        
        // Connect to HUDView
        SerializedObjectHelper.SetFieldValue(hudView, "healthSlider", healthSlider);
        SerializedObjectHelper.SetFieldValue(hudView, "healthText", healthTMP);
    }
    
    private void CreateEnergyBar(HUDView hudView)
    {
        // Create container panel for energy bar
        GameObject energyPanel = new GameObject("EnergyPanel");
        energyPanel.transform.SetParent(hudView.transform, false);
        
        RectTransform energyPanelRect = energyPanel.AddComponent<RectTransform>();
        energyPanelRect.anchorMin = new Vector2(0, 1);
        energyPanelRect.anchorMax = new Vector2(0.3f, 1);
        energyPanelRect.pivot = new Vector2(0, 1);
        energyPanelRect.anchoredPosition = new Vector2(10, -50);
        energyPanelRect.sizeDelta = new Vector2(0, 30);
        
        // Create energy bar slider
        GameObject energyBar = new GameObject("EnergyBar");
        energyBar.transform.SetParent(energyPanel.transform, false);
        
        Slider energySlider = energyBar.AddComponent<Slider>();
        energySlider.minValue = 0;
        energySlider.maxValue = 100;
        energySlider.value = 100;
        
        RectTransform energyBarRect = energyBar.GetComponent<RectTransform>();
        energyBarRect.anchorMin = new Vector2(0, 0);
        energyBarRect.anchorMax = new Vector2(1, 1);
        energyBarRect.sizeDelta = Vector2.zero;
        
        // Create slider background
        GameObject background = new GameObject("Background");
        background.transform.SetParent(energyBar.transform, false);
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f);
        
        RectTransform bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        
        // Create fill area
        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(energyBar.transform, false);
        
        RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
        fillAreaRect.anchorMin = Vector2.zero;
        fillAreaRect.anchorMax = Vector2.one;
        fillAreaRect.offsetMin = new Vector2(5, 0);
        fillAreaRect.offsetMax = new Vector2(-5, 0);
        
        // Create fill
        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(fillArea.transform, false);
        Image fillImage = fill.AddComponent<Image>();
        fillImage.color = Color.blue;
        
        RectTransform fillRect = fill.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.sizeDelta = Vector2.zero;
        
        // Create energy text
        GameObject energyText = new GameObject("EnergyText");
        energyText.transform.SetParent(energyPanel.transform, false);
        
        TextMeshProUGUI energyTMP = energyText.AddComponent<TextMeshProUGUI>();
        energyTMP.text = "100/100";
        energyTMP.alignment = TextAlignmentOptions.Center;
        energyTMP.fontSize = 14;
        energyTMP.color = Color.white;
        
        RectTransform energyTextRect = energyText.GetComponent<RectTransform>();
        energyTextRect.anchorMin = new Vector2(0, 0);
        energyTextRect.anchorMax = new Vector2(1, 1);
        energyTextRect.sizeDelta = Vector2.zero;
        
        // Set up the slider components
        energySlider.fillRect = fillRect;
        energySlider.targetGraphic = bgImage;
        
        // Connect to HUDView
        SerializedObjectHelper.SetFieldValue(hudView, "energySlider", energySlider);
        SerializedObjectHelper.SetFieldValue(hudView, "energyText", energyTMP);
    }
}
