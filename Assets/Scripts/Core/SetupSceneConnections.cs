using UnityEngine;

/// <summary>
/// Sets up all scene connections and references
/// </summary>
public class SetupSceneConnections : MonoBehaviour
{
    void Awake()
    {
        // Connect PlatformerController to GroundCheck
        GameObject player = GameObject.Find("Player");
        GameObject groundCheck = GameObject.Find("GroundCheck");
        if (player != null && groundCheck != null)
        {
            PlatformerController platformerController = player.GetComponent<PlatformerController>();
            if (platformerController != null)
            {
                SerializedObjectHelper.SetFieldValue(platformerController, "groundCheck", groundCheck.transform);
                
                // Set ground layers
                SerializedObjectHelper.SetFieldValue(platformerController, "groundLayers", LayerMask.GetMask("Default"));
            }
        }
        
        // Connect MechController to FirePoint and Projectile prefab
        GameObject firePoint = GameObject.Find("FirePoint");
        GameObject projectile = GameObject.Find("Projectile");
        if (player != null && firePoint != null && projectile != null)
        {
            MechController mechController = player.GetComponent<MechController>();
            if (mechController != null)
            {
                SerializedObjectHelper.SetFieldValue(mechController, "firePoint", firePoint.transform);
                SerializedObjectHelper.SetFieldValue(mechController, "projectilePrefab", projectile);
            }
        }
        
        // Connect HUDController to HUDView
        GameObject hud = GameObject.Find("HUD");
        if (hud != null)
        {
            HUDController hudController = hud.GetComponent<HUDController>();
            HUDView hudView = hud.GetComponent<HUDView>();
            if (hudController != null && hudView != null)
            {
                SerializedObjectHelper.SetFieldValue(hudController, "hudView", hudView);
            }
        }
    }
}

/// <summary>
/// Helper class for setting serialized fields
/// </summary>
public static class SerializedObjectHelper
{
    /// <summary>
    /// Set a serialized field on a MonoBehaviour
    /// </summary>
    public static void SetFieldValue(object target, string fieldName, object value)
    {
        System.Reflection.FieldInfo field = target.GetType().GetField(fieldName, 
            System.Reflection.BindingFlags.Public | 
            System.Reflection.BindingFlags.NonPublic | 
            System.Reflection.BindingFlags.Instance);
            
        if (field != null)
        {
            field.SetValue(target, value);
            Debug.Log($"Set {fieldName} on {target.GetType().Name}");
        }
        else
        {
            Debug.LogError($"Field {fieldName} not found on {target.GetType().Name}");
        }
    }
}
