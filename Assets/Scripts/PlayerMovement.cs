
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
    private Rigidbody2D ghost_rigbod;

    private Collider2D my_collider;
    private Collider2D ghost_collider;

    public float speed = 7f;
    public float jump_force = 20f;

    public GameObject ghost;
    public float delay_secs = 1.2f; // amount of delay between ghost and player
    public float rewind_cooldown = .8f; // time to stay stationary for ghost to respawn
    public float gravity_multiplier = 5;

    private Vector3 dir;
    private Vector3 apply_velocity;

    private bool canFakeDoubleJump; // for rewind jump
    private bool is_moving;
    private float time_counter;

    void Start()
    {
        my_rigbod = GetComponent<Rigidbody2D>();
        my_collider = GetComponent<Collider2D>();
        my_rigbod.gravityScale = gravity_multiplier;

        ghost_rigbod = ghost.GetComponent<Rigidbody2D>();
        ghost_collider = ghost.GetComponent<Collider2D>();
        ghost_rigbod.gravityScale = gravity_multiplier;

        Physics2D.IgnoreCollision(my_collider, ghost_collider); // allow overlap of player and ghost

        StartCoroutine(Rewind());
        time_counter = 0f;
    }


    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        dir = new Vector3(horizontal, 0f, 0f).normalized;

        is_moving = my_rigbod.velocity.magnitude > 0;
        apply_velocity = my_rigbod.velocity;

        if (!is_moving) { time_counter += Time.deltaTime; }
        else { time_counter = 0f; }

        if (Input.GetButton("Jump") &&
            ((Mathf.Abs(my_rigbod.velocity.y) < 0.001f) ||
            canFakeDoubleJump)) // jumping
        {
            apply_velocity = new Vector3(0, jump_force, 0);
            my_rigbod.velocity = apply_velocity;
            canFakeDoubleJump = false;
        }
        else if (Input.GetKey(KeyCode.J) &&
            ghost.activeSelf &&
            Vector3.Distance(ghost.transform.position, transform.position) < 30) // rewinding (press j)
        {
            StartCoroutine(Rewind());
        }
        else // moving horizontally
        {
            apply_velocity = dir * speed;
            apply_velocity.y = my_rigbod.velocity.y;
            my_rigbod.velocity = apply_velocity;
        }

        if (ghost.activeSelf) // if ghost is active
        {
            StartCoroutine(FollowMe(apply_velocity, time_counter)); // shadowing player's movement
        }
        else if (time_counter > rewind_cooldown) // if player remains stationary long enough
        {
            StartCoroutine(RespawnGhost()); // reactivate the ghost
        }
    }


    private IEnumerator Rewind()
    {
        transform.position = ghost.transform.position; // rewind to ghost's position
        ghost.SetActive(false); // despawn ghost
        canFakeDoubleJump = true; // can jump after rewind

        yield return null;
    }

    private IEnumerator RespawnGhost()
    {
        ghost.SetActive(true); // respawns ghost
        ghost.transform.position = transform.position; // ghost respawn wherever the player is
        Physics2D.IgnoreCollision(my_collider, ghost_collider); // re-allow overlap between player and ghost

        yield return null;
    }

    private IEnumerator FollowMe(Vector2 velo, float time_stationary)
    {
        // adjusting for when player and ghost gets slight out of sync 
        if (time_stationary > delay_secs) 
        {
            ghost.transform.position = Vector3.Lerp(ghost.transform.position, transform.position, .2f);
        }

        yield return null;
        yield return new WaitForSeconds(delay_secs);
        ghost_rigbod.velocity = velo;
    }
}