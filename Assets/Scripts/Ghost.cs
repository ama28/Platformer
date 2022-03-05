using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private Collider2D myCollider;
    private GameObject player;
    private PlayerMovement moveScript;
    public float delay_secs = 1.2f; // amount of delay between ghost and player

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        moveScript = player.GetComponent<PlayerMovement>();

        //prevent collision between ghost and player
        myCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(myCollider, player.GetComponent<Collider2D>());

        //make ghost gravity scale match player
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StartCoroutine(FollowMe(moveScript.movementVector, moveScript.time_stationary));
    }

    private IEnumerator FollowMe(Vector2 move, float time_standing_still)
    {
        // adjusting for when player and ghost gets slight out of sync 
        if (time_standing_still > delay_secs)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, .2f);
        }
        yield return new WaitForSeconds(delay_secs);
        myRigidbody.velocity = move;
    }
}
