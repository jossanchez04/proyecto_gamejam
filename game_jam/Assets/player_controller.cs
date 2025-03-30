using UnityEngine;

public class player_controller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 10.0f;
    public Rigidbody2D rb2d;
    private Vector2 moveInput;

    public bool moveRight = false;
    public bool moveLeft = false;
    public bool moveUp = false;
    public bool moveDown = false;

    public int playerDirection;

    public GameObject faceR;
    public GameObject faceL;
    public GameObject faceU;
    public GameObject faceD;

    public Animator playerAnimations;
    


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            moveInput.y = 0;
            rb2d.linearVelocity = moveInput * speed;
        } else
        {
            moveInput.x = 0;
            rb2d.linearVelocity = moveInput * speed;
        }

        //movin right
        if (moveInput.x > 0)
        {
            moveRight = true;
            faceR.SetActive(true);
            faceL.SetActive(false);
            faceU.SetActive(false);
            faceD.SetActive(false);
            playerAnimations.Play("WalkR");
        } 
        else
        {
            moveRight = false;
        }

        //movin left
        if (moveInput.x < 0)
        {
            moveLeft = true;
            faceR.SetActive(false);
            faceL.SetActive(true);
            faceU.SetActive(false);
            faceD.SetActive(false);
            playerAnimations.Play("WalkL");
        }
        else
        {
            moveLeft = false;
        }
        //movin up
        if (moveInput.y > 0)
        {
            moveUp = true;
            faceR.SetActive(false);
            faceL.SetActive(false);
            faceU.SetActive(true);
            faceD.SetActive(false);
            playerAnimations.Play("WalkU");
        }
        else
        {
            moveUp = false;
        }
        //movin down
        if (moveInput.y < 0)
        {
            moveDown = true;
            faceR.SetActive(false);
            faceL.SetActive(false);
            faceU.SetActive(false);
            faceD.SetActive(true);
            playerAnimations.Play("WalkD");
        }
        else
        {
            moveDown = false;
        }

        if (moveInput.x == 0 && moveInput.y == 0)
        {
            playerAnimations.Play("PlayerIdle");
        }
    }
}
