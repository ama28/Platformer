using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ghost : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private Collider2D myCollider;
    private GameObject player;
    private PlayerMovement moveScript;
    private Animator ghostAnimator;
    public bool isDashing = false;
    public float dashXVelocity;
    public float delay_secs = 1.2f; // amount of delay between ghost and player

    // Start is called before the first frame update
    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        moveScript = player.GetComponent<PlayerMovement>();
        ghostAnimator = GetComponent<Animator>();

        GameObject[] obstructions = GameObject.FindGameObjectsWithTag("Obstruction");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject[] collisionsToIgnore = obstructions.Concat(enemies).ToArray();

        //prevent collision between ghost and player
        myCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(myCollider, player.GetComponent<Collider2D>());
        for (int i = 0; i < collisionsToIgnore.Length; i++)
        {
            Physics2D.IgnoreCollision(myCollider, collisionsToIgnore[i].GetComponent<Collider2D>());
        }

        //make ghost gravity scale match player
    }

    void OnCollisionStay2D(Collision2D collision) //kill enemy w dash & check grounded
    {
        if (collision.gameObject.tag == "Enemy" && isDashing)
        {
            Destroy(collision.gameObject);
        }
    }

    private void Update()
    {
        //ghost to dash through enemy code
        //if (Mathf.Abs(myRigidbody.velocity.x) >= dashXVelocity - 1)
        //    isDashing = true;
        //else isDashing = false;

        //if (isDashing && enemy != null)
        //{
        //    Physics2D.IgnoreCollision(myCollider, enemy.GetComponent<Collider2D>(), false);
        //    Physics2D.IgnoreCollision(myCollider, obstructions[0].GetComponent<Collider2D>(), false);
        //}
        //else if (enemy != null)
        //{
        //    Physics2D.IgnoreCollision(myCollider, enemy.GetComponent<Collider2D>());
        //    Physics2D.IgnoreCollision(myCollider, obstructions[0].GetComponent<Collider2D>());
        //}
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
        ghostAnimator.SetFloat("GhostSpeed", Mathf.Abs(move.x));
        myRigidbody.velocity = move;

        //make sure player faces right direction
        if (move.x < 0)
        {
            //character faces left
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        if (move.x > 0)
        {
            //character faces right
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
