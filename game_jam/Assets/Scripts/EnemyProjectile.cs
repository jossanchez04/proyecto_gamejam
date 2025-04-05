using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 7f;
    public int damage = 1;
    public float lifetime = 3f;
    public GameObject hitEffect; // Optional visual effect
    
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime); // Auto-destroy after lifetime
    }
    
    void FixedUpdate()
    {
        rb.linearVelocity = transform.right * speed;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore collisions with other projectiles and enemies
        if (collision.CompareTag("EnemyProjectile") || collision.CompareTag("Enemy")) 
            return;
        
        // Damage player if hit
        if (collision.CompareTag("Player"))
        {
        }

        // Spawn hit effect if assigned
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        // Always destroy projectile on any collision
        Destroy(gameObject);
    }
}