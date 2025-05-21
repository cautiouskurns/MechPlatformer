using UnityEngine;

/// <summary>
/// Handles input for the game
/// </summary>
public class InputManager : MonoBehaviour
{
    // Singleton pattern
    public static InputManager Instance { get; private set; }
    
    // Input state properties
    public float HorizontalInput { get; private set; }
    public float VerticalInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool FirePressed { get; private set; }
    public bool DashPressed { get; private set; }
    public bool PausePressed { get; private set; }
    
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
    }
    
    private void Update()
    {
        // Get movement input
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");
        
        // Get button inputs
        JumpPressed = Input.GetButtonDown("Jump");
        JumpHeld = Input.GetButton("Jump");
        FirePressed = Input.GetButtonDown("Fire1");
        DashPressed = Input.GetKeyDown(KeyCode.LeftShift);
        PausePressed = Input.GetKeyDown(KeyCode.Escape);
        
        // Handle pause input
        if (PausePressed)
        {
            TogglePause();
        }
    }
    
    private void TogglePause()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
            {
                GameManager.Instance.PauseGame();
            }
            else if (GameManager.Instance.CurrentState == GameManager.GameState.Paused)
            {
                GameManager.Instance.ResumeGame();
            }
        }
    }
}
