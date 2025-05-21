using UnityEngine;
using UnityEngine.SceneManagement;

// Event definitions
public class PlayerDiedEvent
{
    public PlayerController Player;
}

public class GameManager : MonoBehaviour
{
    // Singleton pattern
    public static GameManager Instance { get; private set; }

    // Game state enum
    public enum GameState 
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public GameState CurrentState { get; private set; } = GameState.Playing;
    
    [Header("Game Settings")]
    [SerializeField] private int playerStartLives = 3;
    [SerializeField] private float respawnDelay = 2f;
    
    // Game state tracking
    private int currentScore = 0;
    private int currentLives;
    private int enemiesDefeated = 0;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Initialize game state
        currentLives = playerStartLives;
    }
    
    private void Start()
    {
        // Subscribe to events
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Subscribe<PlayerDiedEvent>(OnPlayerDied);
            EventBus.Instance.Subscribe<EnemyDestroyedEvent>(OnEnemyDestroyed);
        }
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<PlayerDiedEvent>(OnPlayerDied);
            EventBus.Instance.Unsubscribe<EnemyDestroyedEvent>(OnEnemyDestroyed);
        }
    }

    public void SetGameState(GameState newState)
    {
        CurrentState = newState;
        
        // Handle state changes
        switch (CurrentState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                ShowGameOverUI();
                break;
            case GameState.MainMenu:
                Time.timeScale = 1f;
                break;
        }

        // Log state change for debugging
        Debug.Log($"Game state changed to: {CurrentState}");
    }
    
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateUI();
    }
    
    public void AddLife()
    {
        currentLives++;
        UpdateUI();
    }
    
    public void PauseGame()
    {
        if (CurrentState == GameState.Playing)
        {
            SetGameState(GameState.Paused);
        }
    }
    
    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            SetGameState(GameState.Playing);
        }
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SetGameState(GameState.Playing);
    }
    
    private void OnPlayerDied(PlayerDiedEvent eventData)
    {
        currentLives--;
        
        if (currentLives <= 0)
        {
            SetGameState(GameState.GameOver);
        }
        else
        {
            // Respawn player after delay
            Invoke("RespawnPlayer", respawnDelay);
        }
        
        UpdateUI();
    }
    
    private void OnEnemyDestroyed(EnemyDestroyedEvent eventData)
    {
        enemiesDefeated++;
        AddScore(10); // Award points for defeating enemy
    }
    
    private void RespawnPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(0, 2, 0); // Default respawn position
            
            // Reset player state
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Reset();
            }
        }
        else
        {
            Debug.LogWarning("Cannot respawn player - player prefab not found");
        }
    }
    
    private void ShowGameOverUI()
    {
        // Find and activate game over UI
        // This would be implemented once UI is set up
        Debug.Log("Game Over! Score: " + currentScore);
    }
    
    private void UpdateUI()
    {
        // Update score and lives display
        // This would be implemented once UI is set up
    }
}
