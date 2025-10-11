using UnityEngine;

public class PlayerProjectiles : MonoBehaviour
{
    // Bullet spawning
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawn;
    
    // Bullet variables
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float bulletSpeed = 10f;
    
    // Bullet container
    [SerializeField] private Transform bulletContainer;
    
    // Player Movement
    private PlayerMovement playerMovement;

    private float nextFireTime;
    
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }



    private void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("No projectile prefab assigned");
            return;
        }
        
        // Determine the direction of the bullet
        Vector2 shootDirection = playerMovement.isFacingRight ? Vector2.right : Vector2.left;
        
        // Create the bullet 
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        
        // Putting all in a container
        if (bulletContainer != null)
        {
            bullet.transform.SetParent(bulletContainer);
        }
        else
        {
            Debug.LogWarning("No projectile container assigned");
        }
        
        // Provide it speed
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null) rb.velocity = shootDirection * bulletSpeed;
        else Debug.LogWarning("No rigidbody assigned");
    }
}
