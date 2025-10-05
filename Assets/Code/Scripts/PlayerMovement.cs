using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    
    // Player movement - related
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.2f;
    //private PlayerStateMachine playerStateMachine;

    // Directional variables
    private float horizontalInput;
    public bool isFacingRight { private set; get; } = true;

    // True/false checks
    public bool isGrounded;
    public bool isMoving;
    public bool isDead;
    public bool isHurt;
    public bool isAttacking;
    public bool isShooting;
    public bool isFalling;
    private bool isFastFalling;
    

    private float originalGravityScale;
    [SerializeField] private float fastFallMultiplier = 2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }
    
    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        
        if (!isGrounded && rb.velocity.y < 0)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }

        if (Input.GetKeyDown(KeyCode.J)) isAttacking = true;
        if (Input.GetKeyDown(KeyCode.K)) isShooting = true;
        IsFastFalling();
        
        CheckIfGrounded();
    }

    private void FixedUpdate() // check
    { 
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        isMoving = (horizontalInput != 0);
        
        if (isFalling) rb.gravityScale = originalGravityScale;

        Flip();
    }

    private void Flip() // check
    {
        if (isDead || isHurt) return;

        if ((isFacingRight && horizontalInput < 0) || (!isFacingRight && horizontalInput > 0))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    private void IsFastFalling()
    {
        if (Input.GetKey(KeyCode.LeftShift)) rb.gravityScale = originalGravityScale * fastFallMultiplier;
        else rb.gravityScale = originalGravityScale;
    }
    
    private void Jump() // check
    {
        if (!isGrounded) return;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
    }

    private void CheckIfGrounded()
    {
        bool currentlyGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckDistance, groundLayer);
        isGrounded = currentlyGrounded;
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);
        }
    }
}
