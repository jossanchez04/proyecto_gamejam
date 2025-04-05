using UnityEngine;
using System.Collections;

public class RandomEnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;
    public float moveDuration = 2f;
    public float wanderRadius = 5f;
    
    private Vector2 moveDirection;
    private float timeUntilChange;
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 startPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        timeUntilChange = 0f;
    }

    void Update()
    {
        if (timeUntilChange <= 0)
        {
            StartCoroutine(ChangeDirection());
            timeUntilChange = Random.Range(minWaitTime, maxWaitTime);
        }
        else
        {
            timeUntilChange -= Time.deltaTime;
        }
        
        // Update animation
        animator.SetBool("IsMoving", moveDirection.magnitude > 0.1f);
        
        // Flip sprite based on direction
        if (moveDirection.x != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(moveDirection.x), 
                1, 
                1);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    IEnumerator ChangeDirection()
    {
        // Calculate random position within wander radius
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 targetPosition = startPosition + randomDirection * wanderRadius;
        
        moveDirection = (targetPosition - (Vector2)transform.position).normalized;
        
        yield return new WaitForSeconds(moveDuration);
        
        // Stop moving
        moveDirection = Vector2.zero;
    }
}
