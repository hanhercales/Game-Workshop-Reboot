using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected int currentHealth;
    [SerializeField] protected int damageToPlayer;
    
    public bool isDead { get; protected set; }
    public bool isHurt { get; protected set; }
    
    // Flashing effect
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected float hurtFlashDuration = 0.1f;
    [SerializeField] protected Color hurtFlashColor = Color.white;

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

        isHurt = true;
        StartCoroutine(FlashEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (isDead) return;
        
        isDead = true;
        Debug.Log(gameObject.name  + " died!");
        Destroy(gameObject, 0.5f);
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(gameObject.name + " hit " + damageToPlayer + " damage to player.");
            // PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            // if (playerHealth != null) playerHealth.TakeDamage(damageToPlayer);
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
        yield return new WaitForSeconds(0.1f);
        isHurt = false;
    }
}
