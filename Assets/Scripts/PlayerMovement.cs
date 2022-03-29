
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D my_rigbod;

    private GameObject ghost;
    public GameObject ghostPrefab;
    private Rigidbody2D ghost_rigbod;

    public float baseSpeed = 6f;
    public float jump_force = 20.0f;
    public Vector2 movementVector;
    public float rewindTeleportSpeed = 5.0f; // time it takes for player to rewind
    private float delay;

    public float rewind_cooldown = 3.0f; // cooldown before rewinding again

    public bool canDoubleJump = true; // for rewind jump
    public float time_stationary;
    public bool is_moving;

    private Animator playerAnimator;
    public bool isGrounded;

    public bool rewindTransition = false;
    public GameObject levelChanger;

    private SpriteRenderer mySpriteRenderer;
    public Sprite defaultSprite;
    public Sprite frozenSprite;

    public GameObject postProcessVolume;
    private ParticleSystem particles;

    //dash stuff
    private float currentSpeed;
    public float dashPower;
    public float dashTime;
    public bool isDashing = false;
    float vertical; 
    float horizontal;
    BoxCollider2D boxCollider2d;
    [SerializeField] private LayerMask platformLayerMask;

    void Start()
    {
        my_rigbod = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();

        ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghost_rigbod = ghost.GetComponent<Rigidbody2D>();

        delay = ghost.GetComponent<Ghost>().delay_secs;
        time_stationary = 0f;

        playerAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        particles = GetComponentInChildren<ParticleSystem>();

        //dash stuff
        currentSpeed = baseSpeed;
        isDashing = false;
    }

    void OnCollisionStay2D(Collision2D collision) //kill enemy w dash & check grounded
    {
        if(collision.gameObject.tag == "Enemy" && isDashing)
        {
            Destroy(collision.gameObject);
        }
    }

    IEnumerator Dash(int direction) //0 = horizontal, 1 = vertical, 2 = diagonal
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Slash");
        playerAnimator.SetTrigger("DashSlash");
        float temp = my_rigbod.gravityScale;
        my_rigbod.gravityScale = 0;

        isDashing = true;
        currentSpeed *= dashPower; //dash

        switch (direction)
        {
            case 0:
                movementVector = new Vector2(horizontal * currentSpeed, 0);
                break;
            case 1:
                movementVector = new Vector2(0, vertical * currentSpeed);
                break;
            case 2:
                movementVector = new Vector2(horizontal * (currentSpeed), vertical * (currentSpeed));
                break;
        }

        yield return new WaitForSeconds(dashTime); //wait before finish dashing

        movementVector = new Vector2(0, 0);
        my_rigbod.gravityScale = temp;
        currentSpeed = baseSpeed; //stop dash
        StartCoroutine(EndDash());
    }

    IEnumerator EndDash()
    {
        yield return new WaitForSeconds(.01f);
        isDashing = false;
    }

    private bool IsGrounded() {
        float extraHeightText = 1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, new Vector3(boxCollider2d.bounds.size.x - 0.05f, boxCollider2d.bounds.size.y, boxCollider2d.bounds.size.z), 0, Vector2.down, extraHeightText, platformLayerMask);
        Color rayColor;
        if(raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCollider2d.bounds.center, Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        //Debug.Log(raycastHit.collider);
        return raycastHit.collider != null;
    }

    private void Update()
    {
        isGrounded = IsGrounded();

        if (isGrounded)
        {
            canDoubleJump = true;
        }
        else
        {
            canDoubleJump = false;
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    RestartScene();
        //}

        //get movement inputs
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        playerAnimator.SetFloat("Speed", Mathf.Abs(horizontal));

        //dash stuff
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.LeftShift)){
            if(!isDashing && canDoubleJump){
                if((vertical == 0 && horizontal != 0)){ //horizontal dash
                    StartCoroutine(Dash(0));
                }
                else if(vertical != 0 && horizontal == 0){ //vertical dash
                    StartCoroutine(Dash(1));
                }
                else if(vertical != 0  && horizontal != 0){ //diagonal dash
                    StartCoroutine(Dash(2));
                }
                playerAnimator.enabled = true;
                if (Time.timeScale == 0f)
                {
                    postProcessVolume.GetComponent<Animator>().SetTrigger("RESUME");

                    ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
                    ghost_rigbod = ghost.GetComponent<Rigidbody2D>();
                    ghost_rigbod.constraints = RigidbodyConstraints2D.FreezePosition;
                    ghost_rigbod.gravityScale = 0;
                }
                Time.timeScale = 1f;
                canDoubleJump = false;

                //re-enable trail
                GetComponent<TrailRenderer>().enabled = true;

                //unfreeze player
                my_rigbod.constraints = RigidbodyConstraints2D.None;
                my_rigbod.constraints = RigidbodyConstraints2D.FreezeRotation;

                //unfreeze ghost after delay
                StartCoroutine(TurnOnGravity());
            }
        }

        if (!isDashing){ //normal left right movement
            movementVector = new Vector2(horizontal * currentSpeed, my_rigbod.velocity.y);
        }

        //execute movement
        my_rigbod.velocity = movementVector;

        //decide whether adjustment is needed – set time statioinary
        is_moving = my_rigbod.velocity.magnitude > 0;

        if (!is_moving) { time_stationary += Time.deltaTime; } 
        else { time_stationary = 0f; }

        //rewind
        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.E)) 
        {
            if (rewindTransition)
            {
                levelChanger.GetComponent<LevelChange>().levelToLoad = "lab";
                levelChanger.GetComponent<LevelChange>().transition = true;
                return;
            }
            playerAnimator.enabled = false;
            ghost.GetComponent<Animator>().enabled = false;
            StartCoroutine(ParticlesWithDelay());
            postProcessVolume.GetComponent<Animator>().SetTrigger("REWIND");
            my_rigbod.constraints = RigidbodyConstraints2D.FreezePosition;
            ghost_rigbod.constraints = RigidbodyConstraints2D.FreezePosition;
            ghost_rigbod.gravityScale = 0;
            StartCoroutine(EBehavior());
        }

        SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
        //make sure player faces right direction
        if (Time.timeScale != 0f)
        {
            if (horizontal < 0)
            {
                //character faces left
                mySprite.flipX = true;
            }

            if (horizontal > 0)
            {
                //character faces right
                mySprite.flipX = false;
            }
        }
    }

    IEnumerator ParticlesWithDelay()
    {
        yield return new WaitForSeconds(.4f);
        particles.Play();
    }
    
    void FixedUpdate()
    {
        //StartCoroutine(FollowMe(movementVector, time_stationary));
        //jump button input
        if (Input.GetButton("Jump") && isGrounded) 
        {
            //play jump sound
            FMODUnity.RuntimeManager.PlayOneShot("event:/Jump");
            //jump
            my_rigbod.velocity = new Vector3(0, jump_force, 0);
            //isGrounded = false;
            playerAnimator.SetTrigger("Jump");
        }
    }

    void PlayFootStep()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Footstep");
    }

    IEnumerator EBehavior()
    {
        yield return new WaitForSeconds(.5f);
        //disable trail then rewind
        particles.Play();
        GetComponent<TrailRenderer>().enabled = false;
        mySpriteRenderer.sprite = frozenSprite;
        Rewind();
        //freeze player & ghost
    }

    void Rewind()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Rewind");
        transform.position = ghost.transform.position;
        Destroy(ghost);

        Time.timeScale = 0f;

        //respawn ghost
        //ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        //ghost_rigbod = ghost.GetComponent<Rigidbody2D>();

        canDoubleJump = true;
    }

    private IEnumerator TurnOnGravity()
    {
        yield return new WaitForSeconds(delay);
        ghost_rigbod.gravityScale = GetComponent<Rigidbody2D>().gravityScale;
        ghost_rigbod.constraints = RigidbodyConstraints2D.None;
        ghost_rigbod.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}