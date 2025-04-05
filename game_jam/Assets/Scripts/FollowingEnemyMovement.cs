using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stoppingDistance = 1f; // How close to get to player
    public float detectionRange = 8f; // How far the enemy can "see" the player
    
    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Calculate distance to player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer < detectionRange && distanceToPlayer > stoppingDistance)
        {
            // Calculate direction to player
            movement = (player.position - transform.position).normalized;
            
            // Animation control
            animator.SetBool("IsMoving", true);
            
            // Flip sprite based on direction
            if (movement.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (movement.x < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            movement = Vector2.zero;
            animator.SetBool("IsMoving", false);
        }
    }

    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}