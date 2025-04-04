using UnityEngine;
using UnityEngine.UI;

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

    public int health = 9;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite twoThirdHeart;
    public Sprite oneThirdHeart;
    public Sprite emptyHeart;
    public GameObject gameOverScreen;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            playerMovement();
        }
        else
        {
            gameOverScreen.SetActive(true);
        }
        showHealth();
    }

    void playerMovement()
    {
        gameOverScreen.SetActive(false);
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            moveInput.y = 0;
            rb2d.linearVelocity = moveInput * speed;
        }
        else
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

    void showHealth()
    {
        int fullHearts = health / 3;
        int fragments = health % 3;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < fullHearts)
            {
                hearts[i].sprite = fullHeart;
                hearts[i].enabled = true;
            }
            else if (i == fullHearts)
            {
                // Show partial heart
                if (fragments == 2)
                    hearts[i].sprite = twoThirdHeart;
                else if (fragments == 1)
                    hearts[i].sprite = oneThirdHeart;
                else
                    hearts[i].sprite = emptyHeart;

                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
                hearts[i].enabled = true;
            }
        }
    }
}
