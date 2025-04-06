using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

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
    public bool justAttacked = false;

    public float attackCooldown = 1f;

    public int playerDirection;

    public GameObject faceR;
    public GameObject faceL;
    public GameObject faceU;
    public GameObject faceD;

    public GameObject sandalR;
    public GameObject sandalL;
    public GameObject sandalU;
    public GameObject sandalD;

    public Animator playerAnimations;

    public int health = 9;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite twoThirdHeart;
    public Sprite oneThirdHeart;
    public Sprite emptyHeart;
    public GameObject gameOverScreen;

    public float fear = 0f;
    public Image[] fearImages;
    public float fearIncrementRate = 1f;

    private float healthDecrementInterval = 1f;
    private float timeToNextHealthDecrement = 1f;

    public GameObject panelCloseEyes;
    private Image panelImage;
    public float closeEyesDuration = 1f; 
    public float cooldownDuration = 5f; 
    private bool canCloseEyes = true;  
    private float currentCooldown = 0f; 
    private float targetAlpha = 0f; 
    private float currentAlpha = 0f;
    public float timeWithEyesClosed = 0f;

    public GameObject sandalPrefab;

    public GameObject restartButton;


    void Start()
    {
        panelImage = panelCloseEyes.GetComponent<Image>(); 
        panelCloseEyes.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            PlayerMovement();
            UpdateFearBar();
            DecrementHealthOverTime();
            if (Input.GetKeyDown(KeyCode.E) && canCloseEyes)
            {
                StartCoroutine(CloseEyesRoutine());
            }
        }
        else
        {
            gameOverScreen.SetActive(true);
            restartButton.SetActive(true);
        }
        ShowHealth();
        if (!canCloseEyes)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
            {
                canCloseEyes = true;
            }
        }
    }

    void PlayerMovement()
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

        if (justAttacked == true && attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && attackCooldown <= 0f)
        {
            if (moveUp)
            {
                Instantiate(sandalPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            }
            else if (moveDown)
            {
                Instantiate(sandalPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
            }
            else if (moveLeft)
            {
                Instantiate(sandalPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            }
            else if (moveRight)
            {
                Instantiate(sandalPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 270)));
            }

            attackCooldown = 1f; // Reset the cooldown timer
        }
        else
        {
            attackCooldown -= Time.deltaTime; // Decrease cooldown over time
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyProjectile"))
        {
            health -= 1;
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
        //SceneManager.LoadScene(2);
    }

    void ShowHealth()
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

    void UpdateFearBar()
    {
        fear += fearIncrementRate * Time.deltaTime;
        fear = Mathf.Clamp(fear, 0, 100); 

        UpdateFearImages();
    }

    void UpdateFearImages()
    {
        int index = Mathf.FloorToInt(fear / (100f / fearImages.Length)); 

        for (int i = 0; i < fearImages.Length; i++)
        {
            if (i <= index)
            {
                fearImages[i].enabled = true; 
            }
            else
            {
                fearImages[i].enabled = false;  
            }
        }
    }

    void DecrementHealthOverTime()
    {
        timeToNextHealthDecrement = Mathf.Lerp(1f, 0.1f, fear / 100f);  

        if (timeToNextHealthDecrement <= 0f && health > 0)
        {
            health--;  
            timeToNextHealthDecrement = 1f;  
        }
        else
        {
            timeToNextHealthDecrement -= Time.deltaTime;  
        }
    }

    IEnumerator CloseEyesRoutine()
    {
        canCloseEyes = false; 
        currentCooldown = cooldownDuration; 

        panelCloseEyes.SetActive(true); 
        targetAlpha = 1f;  

        float timeElapsed = 0f;
        while (timeElapsed < closeEyesDuration)
        {
            currentAlpha = Mathf.Lerp(0f, 1f, timeElapsed / closeEyesDuration); 
            panelImage.color = new Color(0f, 0f, 0f, currentAlpha);  
            timeElapsed += Time.deltaTime;
            yield return null;  
        }
        panelImage.color = new Color(0f, 0f, 0f, 1f);  

        fear -= 10f;  
        fear = Mathf.Clamp(fear, 0f, 100f);  

        float timeWithEyesClosedElapsed = 0f;  
        while (timeWithEyesClosedElapsed < closeEyesDuration)
        {
            timeWithEyesClosedElapsed += Time.deltaTime;  
            yield return null;
        }

        timeElapsed = 0f;
        while (timeElapsed < closeEyesDuration)
        {
            currentAlpha = Mathf.Lerp(1f, 0f, timeElapsed / closeEyesDuration); 
            panelImage.color = new Color(0f, 0f, 0f, currentAlpha);  
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        panelImage.color = new Color(0f, 0f, 0f, 0f);  
        panelCloseEyes.SetActive(false);  
    }

}
