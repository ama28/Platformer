
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

    private bool canFakeDoubleJump; // for rewind jump
    public float time_stationary;
    public bool is_moving;

    public Animator playerAnimator; 

    //dash stuff
    private float currentSpeed;
    public float dashPower;
    public float dashTime;
    bool isDashing;
    float vertical; 
    float horizontal;
    bool isGrounded; 
    float initY;

    void Start()
    {
        my_rigbod = GetComponent<Rigidbody2D>();

        ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghost_rigbod = ghost.GetComponent<Rigidbody2D>();

        delay = ghost.GetComponent<Ghost>().delay_secs;
        time_stationary = 0f;

        playerAnimator = GetComponent<Animator>();

        //dash stuff
        currentSpeed = baseSpeed;
        isDashing = false;
        isGrounded = true;
    }

    void OnCollisionStay2D(Collision2D collision) //kill enemy w dash & check grounded
    {
        if(collision.gameObject.tag == "Enemy" && isDashing)
        {
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    IEnumerator Dash()
    {
        float temp = my_rigbod.gravityScale;
        my_rigbod.gravityScale = 0;

        isDashing = true;
        currentSpeed *= dashPower; //dash
        yield return new WaitForSeconds(dashTime); //wait before finish dashing

        my_rigbod.gravityScale = temp;
        currentSpeed = baseSpeed; //stop dash
        isDashing = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }

        //get movement inputs
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        playerAnimator.SetFloat("Speed", Mathf.Abs(horizontal));

        //dash stuff
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            if(!isDashing && !isGrounded && horizontal == 0 && vertical < 0){ //mid-air down dash
                StartCoroutine(Dash());
                movementVector = new Vector2(0, vertical * currentSpeed);
            }
            if(!isDashing && isGrounded){
                if(vertical == 0 && horizontal != 0){ //horizontal dash
                    StartCoroutine(Dash());
                    movementVector = new Vector2(horizontal * currentSpeed, 0);
                    if(!canFakeDoubleJump)
                        isGrounded = false;
                }
                if(horizontal == 0 && vertical != 0){ //vertical dash
                    StartCoroutine(Dash());
                    movementVector = new Vector2(0, vertical * currentSpeed);
                    if(vertical > 0){
                        isGrounded = false;
                    }
                }
                if(vertical != 0  && horizontal != 0){ //diagonal dash
                    StartCoroutine(Dash());
                    movementVector = new Vector2(horizontal * currentSpeed, vertical * currentSpeed); 
                    isGrounded = false;
                }

                //REMOVE FREEZE

                //re-enable trail
                GetComponent<TrailRenderer>().enabled = true;

                //unfreeze player
                my_rigbod.constraints = RigidbodyConstraints2D.None;
                my_rigbod.constraints = RigidbodyConstraints2D.FreezeRotation;

                //unfreeze ghost after delay
                StartCoroutine(TurnOnGravity());
            }
        }
        else{ //normal left right movement
            movementVector = new Vector2(horizontal * currentSpeed, my_rigbod.velocity.y);
        }

        //execute movement
        my_rigbod.velocity = movementVector;

        //decide whether adjustment is needed – set time statioinary
        is_moving = my_rigbod.velocity.magnitude > 0;

        if (!is_moving) { time_stationary += Time.deltaTime; } 
        else { time_stationary = 0f; }

        //rewind
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            //disable trail then rewind
            GetComponent<TrailRenderer>().enabled = false;
            Rewind();

            //freeze player & ghost
            my_rigbod.constraints = RigidbodyConstraints2D.FreezePosition;
            ghost_rigbod.constraints = RigidbodyConstraints2D.FreezePosition;
            ghost_rigbod.gravityScale = 0;
        }

        //make sure player faces right direction
        if (horizontal < 0)
        {
            //character faces left
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        if (horizontal > 0)
        {
            //character faces right
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
    
    void FixedUpdate()
    {
        //StartCoroutine(FollowMe(movementVector, time_stationary));
        //jump button input
        if (Input.GetButton("Jump") &&
            ((Mathf.Abs(my_rigbod.velocity.y) < 0.001f) ||
            canFakeDoubleJump)) 
        {
            //jump
            my_rigbod.velocity = new Vector3(0, jump_force, 0);
            canFakeDoubleJump = false;
            //isGrounded = false;
        }
    }

    void Rewind()
    {
        transform.position = ghost.transform.position;
        Destroy(ghost);

        //respawn ghost
        ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghost_rigbod = ghost.GetComponent<Rigidbody2D>();

        canFakeDoubleJump = true;

        //GetComponent<TrailRenderer>().enabled = true;
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