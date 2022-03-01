using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhostTest : MonoBehaviour
{
    private Rigidbody2D my_rigbod;

    public float speed = 0.7f;
    public float jump_force = 20f;

    public float delay_secs = 1.2f; // amount of delay between ghost and player
    public float rewind_cooldown = .8f; // time to stay stationary for ghost to respawn
    public float gravity_multiplier = 5;

    private bool canFakeDoubleJump; // for rewind jump
    private float time_counter;
    private GameObject ghost;
    private bool rewinded;

    void Start()
    {
        //init rigbod and ghost
        ghost = transform.Find("Ghost").gameObject;
        my_rigbod = GetComponent<Rigidbody2D>();

        //adjust gravity
        my_rigbod.gravityScale = gravity_multiplier;
        ghost.GetComponent<Rigidbody2D>().gravityScale = gravity_multiplier;

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ghost.GetComponent<Collider2D>()); // allow overlap of player and ghost

        ghost.SetActive(false); // despawn ghost
        canFakeDoubleJump = true; // can jump after rewind

        time_counter = 0f;
        rewinded = true;
    }


    // Update is called once per frame
    void Update()
    {
        //only count if rewinded
        if(rewinded)
            time_counter += Time.deltaTime;

        if (Input.GetButton("Jump") &&
            ((Mathf.Abs(my_rigbod.velocity.y) < 0.001f) ||
            canFakeDoubleJump)) // jumping
        {
            my_rigbod.velocity += new Vector2(0, jump_force);
            canFakeDoubleJump = false;
        }
        else if (Input.GetKey(KeyCode.J) && ghost.activeSelf) // rewinding (press j)
        {
            rewind();
        }
        else 
        {
            my_rigbod.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, my_rigbod.velocity.y);
        }

        //update ghost position
        if (!rewinded) // if ghost rewind is active
        {
            StartCoroutine(FollowMe(my_rigbod.velocity, time_counter)); // shadowing player's movement
            //time_counter = 0;//makesure cooldown is zero
        }
        else if (time_counter > rewind_cooldown) // waitfor cool down
        {
            respawnGhost(); // reactivate the ghost
            rewinded = false; //turn off rewinded
        }

    }

    private void rewind()
    {
        transform.position = ghost.transform.position; // rewind to ghost's position
        ghost.SetActive(false); // despawn ghost
        canFakeDoubleJump = true; // can jump after rewind
        rewinded = true;
    }

    private void respawnGhost()
    {
        // respawns ghost
        ghost.transform.localPosition = new Vector3(0,0,0);
        //ghost.transform.position = transform.position;
        ghost.SetActive(true);// ghost respawn wherever the player is
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ghost.GetComponent<Collider2D>());
    }

    private IEnumerator FollowMe(Vector2 velo, float time_stationary)
    {
        // adjusting for when player and ghost gets slight out of sync 
        if (time_stationary > delay_secs)
        {
            ghost.transform.position = Vector3.Lerp(ghost.transform.position, transform.position, .2f);
            Debug.Log(ghost.transform.localPosition.magnitude);
            if (ghost.transform.localPosition.magnitude == 0)
            {
               
                //time_counter = 0;
            }
                
        }

        yield return null;
        yield return new WaitForSeconds(delay_secs);
        ghost.GetComponent<Rigidbody2D>().velocity = velo;
    }
}


