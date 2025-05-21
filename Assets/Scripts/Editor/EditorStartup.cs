using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
[InitializeOnLoad]
public static class EditorStartup
{
    static EditorStartup()
    {
        // Subscribe to the scene opened event
        EditorApplication.sceneOpened += OnSceneOpened;
        
        // Also run on editor startup
        if (EditorApplication.isPlayingOrWillChangePlaymode == false)
        {
            ValidateProjectSetup();
        }
    }
    
    private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
    {
        // Check if this is the main scene
        if (scene.name == "SampleScene" || scene.name == "Main")
        {
            ValidateProjectSetup();
        }
    }
    
    private static void ValidateProjectSetup()
    {
        // Check for needed layers
        CheckLayer("Ground", 8);
        
        // Check for needed tags
        CheckTag("Player");
        CheckTag("Enemy");
        CheckTag("Ground");
        CheckTag("Projectile");
        
        Debug.Log("Project layers and tags validated by editor startup");
    }
    
    private static void CheckLayer(string layerName, int suggestedIndex)
    {
        // Check if the layer exists
        if (LayerMask.NameToLayer(layerName) == -1)
        {
            Debug.LogWarning($"Layer '{layerName}' does not exist. Please add it manually in Edit > Project Settings > Tags and Layers. Suggested index: {suggestedIndex}");
        }
    }
    
    private static void CheckTag(string tagName)
    {
        // Try to find the tag
        try
        {
            if (!UnityEditorInternal.InternalEditorUtility.tags.Contains(tagName))
            {
                Debug.LogWarning($"Tag '{tagName}' does not exist. Please add it manually in Edit > Project Settings > Tags and Layers.");
            }
        }
        catch (System.Exception)
        {
            Debug.LogWarning($"Could not validate tag '{tagName}'.");
        }
    }
}
#endif
