using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;
    
    //hpPanel to display
    [SerializeField] private GameObject healthPanel;
    
    public bool isDead { get; private set; }
    public bool isHurt { get; private set; }
    
    // Invincibility check
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private float flashInterval = 0.1f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;
        isHurt = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void UpdateHealthIcons()
    {
        if (healthPanel.transform.childCount < currentHealth)
        {
            Debug.Log("HP > Max HP");
            return;
        }
        
        for (int i = 0; i < currentHealth; i++)
        {
            healthPanel.transform.GetChild(i).gameObject.SetActive(true);
        }

        for (int i = (int)currentHealth; i < maxHealth; i++)
        {
            healthPanel.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead || isHurt) return;
        currentHealth -= damageAmount;
        // Debug.Log("Player hit! Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            UpdateHealthIcons();
            StartCoroutine(InvincibilityRoutine());
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Player is dead!");

        StartCoroutine(ReloadSceneAfterDelay(1.5f));
    }

    private IEnumerator InvincibilityRoutine()
    {
        isHurt = true;
        Debug.Log(isHurt);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);

        float timer = 0f;
        while (timer < invincibilityDuration)
        {
            if (spriteRenderer != null)
            {
                // spriteRenderer.enabled = !spriteRenderer.enabled;
            }
            yield return new WaitForSeconds(flashInterval);
            timer += flashInterval;
        }

        if (spriteRenderer != null)
        {
            // spriteRenderer.enabled = true;
        }
        
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
        isHurt = false;
    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
