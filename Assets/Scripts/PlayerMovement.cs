
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// Really easy to get out of sync if ghost can respawn while player is moving
/// Still sometimes gets out of sync if ghost respawns only when player is stationary
/// - Could be a render issue or some rigidbody setting thing?
/// 
/// Bugs:
/// - sometimes ghost and player gets slightly out of sync
/// - sometimes player/ghost gets stuck in platform
/// - can upper wall hang since y velocity is 0...
/// 
/// </summary>


public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D my_rigbod;

    private GameObject ghost;
    public GameObject ghostPrefab;
    private Rigidbody2D ghost_rigbod;

    public float speed = 7f;
    public float jump_force = 20f;
    public Vector2 movementVector;
    public float rewindAnimationLength = 0f; // time it takes for player to rewind
    private float delay;

    public float rewind_cooldown = 3.0f; // cooldown before rewinding again

    private bool canFakeDoubleJump; // for rewind jump
    public float time_stationary;
    public bool is_moving;

    void Start()
    {
        my_rigbod = GetComponent<Rigidbody2D>();

        ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghost_rigbod = ghost.GetComponent<Rigidbody2D>();

        delay = ghost.GetComponent<Ghost>().delay_secs;
        time_stationary = 0f;
    }

    private void Update()
    {
        //get movement inputs
        float horizontal = Input.GetAxisRaw("Horizontal");
        movementVector = new Vector2(horizontal * speed, my_rigbod.velocity.y);

        //decide whether adjustment is needed – set time statioinary
        is_moving = my_rigbod.velocity.magnitude > 0;

        if (!is_moving) { time_stationary += Time.deltaTime; } //set time standing still
        else { time_stationary = 0f; }

        //rewind
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            Rewind();
            my_rigbod.constraints = RigidbodyConstraints2D.FreezePosition;
            ghost_rigbod.constraints = RigidbodyConstraints2D.FreezeRotation;
            ghost_rigbod.gravityScale = 0;
        }
    }

    void FixedUpdate()
    {
        //execute movement
        my_rigbod.velocity = movementVector;
        //StartCoroutine(FollowMe(movementVector, time_stationary));

        //player jump
        if (Input.GetButton("Jump") &&
            ((Mathf.Abs(my_rigbod.velocity.y) < 0.001f) ||
            canFakeDoubleJump)) // jumping
        {
            //REMOVE FREEZE
            //unfreeze player
            my_rigbod.constraints = RigidbodyConstraints2D.None;
            my_rigbod.constraints = RigidbodyConstraints2D.FreezeRotation;

            //unfreeze ghost
            ghost_rigbod.constraints = RigidbodyConstraints2D.None;
            ghost_rigbod.constraints = RigidbodyConstraints2D.FreezeRotation;

            //start ghost delay (prevent it from falling for delay time if its in the air)
            StartCoroutine(TurnOnGravity());

            my_rigbod.velocity = new Vector3(0, jump_force, 0);
            canFakeDoubleJump = false;
        }
    }

    void Rewind()
    {
        transform.position = ghost.transform.position;
        Destroy(ghost);
        Debug.Log("destroyed");

        //respawn ghost
        ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghost_rigbod = ghost.GetComponent<Rigidbody2D>();

        canFakeDoubleJump = true;
    }

    private IEnumerator TurnOnGravity()
    {
        yield return new WaitForSeconds(delay);
        ghost_rigbod.gravityScale = GetComponent<Rigidbody2D>().gravityScale;
    }
}