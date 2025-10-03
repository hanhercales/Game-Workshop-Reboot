using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private LayerMask groundLayer; // Ground checking
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    
    private float horizontalInput;
    public bool isFacingRight { private set; get; } = true;

    public bool isGrounded;
    public bool isMoving;
    public bool isDead;
    public bool isHurt;
    public bool isAttacking;
    public bool isShooting;
    public bool isFalling;

    private float originalGravityScale;
    [SerializeField] private float fastFallMultiplier = 2.0f;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (rb.velocity != Vector2.zero) isMoving = true;
        else  isMoving = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        
        isAttacking = Input.GetKey(KeyCode.J);
        isShooting = Input.GetKey(KeyCode.K);
        
        if (!isGrounded && rb.velocity.y < 0)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }
        
        CheckIfGrounded();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        isMoving = (horizontalInput != 0);

        if (isFalling) rb.gravityScale = originalGravityScale * fastFallMultiplier;
        else  rb.gravityScale = originalGravityScale;

        Flip();
    }
    
    public void SetHorizontalMovement(float input)
    {
        horizontalInput = input;
    }
    
    public void Jump()
    {
        if (!isGrounded) return;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
    }

    private void CheckIfGrounded()
    {
        bool currentlyGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isGrounded = currentlyGrounded;
    }
    
    private void Flip()
    {
        if (isDead || isHurt) return;
        
        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f; // Flip the X scale
            transform.localScale = localScale;
        }
    }

    private void Die()
    {
        if(isDead)
            gameObject.SetActive(false);
    }
    
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
