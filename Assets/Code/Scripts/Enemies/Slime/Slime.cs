using UnityEngine;
public class Slime : Enemy
{
    // Movement
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float patrolDistance = 4f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.2f;
    
    private Rigidbody2D rb;
    private float initialXPosition;
    private int patrolDirection = 1;
    
    public bool isMoving { get; private set; }
    public bool isGrounded { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        initialXPosition = transform.position.x;
        isMoving = true;
    }

    private void Update()
    {
        CheckIfGrounded();

        if (!isDead && !isHurt && isGrounded)
        {
            isMoving = true;
            if (patrolDirection == 1 && transform.position.x >= initialXPosition + patrolDistance)
            {
                patrolDirection = -1;
                Flip();
            }
            else if (patrolDirection == -1 && transform.position.x <= initialXPosition - patrolDistance)
            {
                patrolDirection = 1;
                Flip();
            }
        }
        else
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (isMoving && !isDead && !isHurt && isGrounded)
        {
            rb.velocity = new Vector2(patrolDirection * moveSpeed, rb.velocity.y);
        }
        else if (isDead || isHurt || !isGrounded)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    private void CheckIfGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckDistance, groundLayer);
    }

    private void Flip()
    {
        // Simple flip logic for the sprite
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);
        }
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector2(initialXPosition, transform.position.y), new Vector2(initialXPosition + patrolDistance, transform.position.y));
        Gizmos.DrawLine(new Vector2(initialXPosition, transform.position.y), new Vector2(initialXPosition - patrolDistance, transform.position.y));
    }
}
