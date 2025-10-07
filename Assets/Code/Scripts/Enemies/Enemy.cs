using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected int currentHealth;
    [SerializeField] protected int damageToPlayer;
    
    public bool isDead {get; protected set;}
    public bool isHurt {get; protected set;}

    // For flashing effect
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected float hurtFlashDuration = 0.1f; // How long to flash
    [SerializeField] protected Color hurtFlashColor = Color.white; // Color when hurt

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;
        isHurt = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    // Take damage
    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;
        
        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " has been hit " + damageAmount + ". Current health: " + currentHealth);

        isHurt = true; // Set isHurt to true when hit!
        StartCoroutine(FlashEffect()); // Start the flashing effect

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (isDead) return;
        
        isDead = true;
        Debug.Log(gameObject.name + " died!");
        Destroy(gameObject, 0.5f); // Consider pooling enemies in a real game
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(gameObject.name + " hit " + damageToPlayer + " damage to player.");
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            
            if (playerHealth != null) playerHealth.TakeDamage(damageToPlayer);
        }
            
    }
    
    protected IEnumerator FlashEffect()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = hurtFlashColor;
            yield return new WaitForSeconds(hurtFlashDuration);
            spriteRenderer.color = originalColor;
        }
        yield return new WaitForSeconds(0.1f); // Small delay
        isHurt = false; // Reset isHurt
    }
}