
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Summary
 
 Really easy to get out of sync if ghost can respawn while player is moving
 Still sometimes gets out of sync if ghost respawns only when player is stationary
 - Could be a render issue or some rigidbody setting thing?
 
 Bugs:
 - sometimes ghost and player gets slightly out of sync
 - sometimes player/ghost gets stuck in platform
 - can upper wall hang since y velocity is 0...
 */


public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D my_rigbod;

    private GameObject ghost;
    public GameObject ghostPrefab;
    private Rigidbody2D ghost_rigbod;

    float speed;
    public float jump_force = 20.0f;
    public Vector2 movementVector;
    public float rewindTeleportSpeed = 5.0f; // time it takes for player to rewind
    private float delay;

    public float rewind_cooldown = 3.0f; // cooldown before rewinding again

    private bool canFakeDoubleJump; // for rewind jump
    public float time_stationary;
    public bool is_moving;

    //dash stuff
    public float dashPower;
    public float dashTime;
    public float baseSpeed;
    bool isDashing;
    float vertical; 
    float horizontal;
    bool isGrounded; 

    void Start()
    {
        my_rigbod = GetComponent<Rigidbody2D>();

        ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghost_rigbod = ghost.GetComponent<Rigidbody2D>();

        delay = ghost.GetComponent<Ghost>().delay_secs;
        time_stationary = 0f;

        //dash stuff
        speed = baseSpeed;
        isDashing = false;
        isGrounded = false;
    }

    IEnumerator Dash()
    {
        isDashing = true; 
        speed *= dashPower; //dash
        yield return new WaitForSeconds(dashTime); //wait before finish dashing
        speed = baseSpeed; //stop dash
        isDashing = false; 
    }

    void OnCollisionEnter2D(Collision2D collision) //kill enemy w dash & check grounded
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

    private void Update()
    {
        //get movement inputs
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        //dash stuff
        if(Input.GetKey(KeyCode.K)){
            if(!isDashing && isGrounded){
                if(vertical == 0){ //horizontal dash
                    StartCoroutine(Dash());
                    movementVector = new Vector2(horizontal * speed, my_rigbod.velocity.y);
                }
                if(horizontal == 0){ //vertical dash
                    StartCoroutine(Dash());
                    movementVector = new Vector2(my_rigbod.velocity.x, vertical * speed);
                    isGrounded = false;
                }
                if(vertical != 0  && horizontal != 0){ //diagonal dash
                    StartCoroutine(Dash());
                    movementVector = new Vector2(horizontal * speed, vertical * speed); 
                    isGrounded = false;
                }
            }
        }
        else{ //normal left right movement
            movementVector = new Vector2(horizontal * speed, my_rigbod.velocity.y);
        }

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
    }
    
    void FixedUpdate()
    {
        //execute movement
        my_rigbod.velocity = movementVector;

        //StartCoroutine(FollowMe(movementVector, time_stationary));
        //jump button input
        if (Input.GetButton("Jump") &&
            ((Mathf.Abs(my_rigbod.velocity.y) < 0.001f) ||
            canFakeDoubleJump)) 
        {
            //REMOVE FREEZE

            //re-enable trail
            GetComponent<TrailRenderer>().enabled = true;

            //unfreeze player
            my_rigbod.constraints = RigidbodyConstraints2D.None;
            my_rigbod.constraints = RigidbodyConstraints2D.FreezeRotation;

            //unfreeze ghost after delay
            StartCoroutine(TurnOnGravity());

            //jump
            my_rigbod.velocity = new Vector3(0, jump_force, 0);
            canFakeDoubleJump = false;
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
}