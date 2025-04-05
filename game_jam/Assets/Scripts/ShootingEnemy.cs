using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.5f;
    public float wanderRadius = 3f;
    public float minWanderTime = 1f;
    public float maxWanderTime = 3f;
    
    [Header("Shooting")]
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public float shootRange = 6f;
    public float projectileSpeed = 8f;
    public Transform firePoint;
    
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 wanderPoint;
    private float wanderTimer;
    private float shootTimer;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        SetNewWanderPoint();
        shootTimer = shootInterval; // Shoot immediately
    }
    
    void Update()
    {
        // Handle wandering
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0)
        {
            SetNewWanderPoint();
        }
        
        // Handle shooting
        shootTimer -= Time.deltaTime;
        bool playerInRange = Vector2.Distance(transform.position, player.position) <= shootRange;
        
        if (playerInRange && shootTimer <= 0)
        {
            ShootAtPlayer();
            shootTimer = shootInterval;
        }
        
        // Animation
        animator.SetBool("IsMoving", rb.linearVelocity.magnitude > 0.1f);
    }
    
    void FixedUpdate()
    {
        // Move toward wander point
        Vector2 direction = (wanderPoint - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        
        // Face player if in range, otherwise face movement direction
        if (Vector2.Distance(transform.position, player.position) <= shootRange)
        {
            FacePlayer();
        }
        else if (rb.linearVelocity.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(rb.linearVelocity.x), 1, 1);
        }
    }
    
    void SetNewWanderPoint()
    {
        wanderPoint = (Vector2)transform.position + Random.insideUnitCircle * wanderRadius;
        wanderTimer = Random.Range(minWanderTime, maxWanderTime);
    }
    
    void FacePlayer()
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }
    
    void ShootAtPlayer()
    {
        if (projectilePrefab && firePoint)
        {
            // Calculate direction to player
            Vector2 direction = (player.position - firePoint.position).normalized;
            
            // Instantiate and rotate projectile to face player
            GameObject projectile = Instantiate(
                projectilePrefab, 
                firePoint.position, 
                Quaternion.identity
            );
            
            // Set projectile rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            // Set projectile velocity
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            if (projectileRb) projectileRb.linearVelocity = direction * projectileSpeed;
            
            // Trigger shoot animation
            animator.SetTrigger("Shoot");
        }
    }
    
    // Visualize wander radius and shoot range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
