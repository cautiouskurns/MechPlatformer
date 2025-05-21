using UnityEngine;

/// <summary>
/// Creates basic placeholder sprites for development purposes
/// </summary>
public static class PlaceholderSprites
{
    /// <summary>
    /// Create a square sprite with the given color
    /// </summary>
    public static Sprite CreateSquareSprite(Color color)
    {
        // Create a 32x32 texture
        Texture2D texture = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        
        // Fill texture with color
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }
        texture.SetPixels(pixels);
        texture.Apply();
        
        // Create sprite from texture
        return Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
    }
    
    /// <summary>
    /// Create a circle sprite with the given color
    /// </summary>
    public static Sprite CreateCircleSprite(Color color)
    {
        // Create a 32x32 texture
        Texture2D texture = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        
        // Center and radius
        Vector2 center = new Vector2(16, 16);
        float radius = 15;
        
        // Fill texture with transparent color first
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.clear;
        }
        
        // Fill circle with color
        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                if (distance <= radius)
                {
                    pixels[y * 32 + x] = color;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        // Create sprite from texture
        return Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
    }
    
    /// <summary>
    /// Helper to apply a sprite to a gameobject
    /// </summary>
    public static void ApplySprite(GameObject gameObject, Sprite sprite)
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sprite = sprite;
        }
    }
}
