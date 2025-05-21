using UnityEngine;

/// <summary>
/// Common game events used across the game
/// </summary>
public static class GameEvents
{
    // Player events
    public class PlayerDiedEvent
    {
        public PlayerController Player;
    }
    
    // Enemy events
    public class EnemyDestroyedEvent
    {
        public EnemyController Enemy;
    }
    
    // Game state events
    public class GameStateChangedEvent
    {
        public GameManager.GameState PreviousState;
        public GameManager.GameState NewState;
    }
    
    // Projectile events
    public class ProjectileHitEvent
    {
        public Projectile Projectile;
        public GameObject HitObject;
    }
}
