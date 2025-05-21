using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private int dashEnergyCost = 20;
    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayers;
    
    // Component references
    private Rigidbody2D rb;
    private PlayerController playerController;
    private Animator animator;
    
    // Movement state tracking
    private bool isGrounded;
    private bool isDashing;
    private float dashTimeLeft;
    private float horizontalInput;
    private bool facingRight = true;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        // Check if game is paused
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        {
            return;
        }
        
        // Get player input from InputManager if available
        if (InputManager.Instance != null)
        {
            horizontalInput = InputManager.Instance.HorizontalInput;
            
            // Handle jump input
            if (InputManager.Instance.JumpPressed && isGrounded)
            {
                Jump();
            }
            
            // Handle dash input
            if (InputManager.Instance.DashPressed && !isDashing)
            {
                TryDash();
            }
        }
        else
        {
            // Fallback to direct input if InputManager not available
            horizontalInput = Input.GetAxis("Horizontal");
            
            // Handle jump input
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Jump();
            }
            
            // Handle dash input
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
            {
                TryDash();
            }
        }
        
        // Check if player is on ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);
        
        // Update dash state
        if (isDashing)
        {
            UpdateDash();
        }
        
        // Update animations if animator exists
        UpdateAnimationState();
    }
    
    private void FixedUpdate()
    {
        // Handle horizontal movement
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }
        
        // Flip player based on movement direction
        if (horizontalInput > 0 && !facingRight || horizontalInput < 0 && facingRight)
        {
            FlipCharacter();
        }
    }
    
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        
        // Trigger jump animation if animator exists
        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }
    
    private void TryDash()
    {
        if (playerController != null && playerController.UseEnergy(dashEnergyCost))
        {
            isDashing = true;
            dashTimeLeft = dashDuration;
            
            // Dash forward in facing direction
            float dashDirection = facingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);
            
            // Optional: Make character temporarily invulnerable during dash
            
            // Trigger dash animation if animator exists
            if (animator != null)
            {
                animator.SetTrigger("Dash");
            }
        }
    }
    
    private void UpdateDash()
    {
        dashTimeLeft -= Time.deltaTime;
        
        if (dashTimeLeft <= 0)
        {
            isDashing = false;
        }
    }
    
    private void FlipCharacter()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    
    private void UpdateAnimationState()
    {
        if (animator == null) return;
        
        // Set animation parameters
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsDashing", isDashing);
    }
    
    private void OnDrawGizmosSelected()
    {
        // Visualize ground check radius in editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
