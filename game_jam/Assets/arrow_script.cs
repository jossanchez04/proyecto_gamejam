using UnityEngine;

public class arrow_script : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    public float lifetime = 3f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Make sure gravity doesn't affect the arrow
        rb.gravityScale = 0;

        // Move the arrow based on its facing direction (local "up" is forward)
        rb.linearVelocity = transform.up * speed;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyProjectile"))
        {
            enemy_health enemy = collision.GetComponent<enemy_health>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        if (collision.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }
}
