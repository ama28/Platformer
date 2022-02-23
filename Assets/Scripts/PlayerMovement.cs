using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rigbod;
    //private Transform transform_player;
    //public Animator animator;

    //movement stuff
    private Vector3 dir;
    public float speed = 7f;
    public float JumpForce = 20f;
    //float horizontalMove = 0f;

    //Fire bullet stuff
    //public Fireball fireball;
    //public Transform LauchOffSet;

    public GameObject ghost;
    public int offset_frames = 30; // amount of delay between ghost and player
    public float rewind_cooldown = 2f;
    public float gravity_multiplier = 5;

    private List<Vector3> storedPositions; // for storing player positions for ghost to follow later
    private bool canFakeDoubleJump; // for rewind jump

    void Start()
    {
        rigbod = GetComponent<Rigidbody2D>();
        rigbod.gravityScale = gravity_multiplier;
        storedPositions = new List<Vector3>();
        //transform_player = GetComponent<Transform>();
    }

    void Update()
    {
        //Fire bullet
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    Instantiate(fireball, LauchOffSet.position, transform.rotation);
        //    FMODUnity.RuntimeManager.PlayOneShot("event:/Laser");
        //}

        //comment out position restriction in case 
        //if (Input.GetKeyDown(KeyCode.R)) //| transform.position.y < -5)
        //{
        //    RestartScene();
        //}

        //allow animator to access character's speed
        //horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
        //animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }


    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        dir = new Vector3(horizontal, 0f, 0f).normalized;

        storedPositions.Add(transform.position); // store the position of player every frame

        if (storedPositions.Count > offset_frames) // after a number of frames, ghost follows the player's movement
        {
            ghost.transform.position = storedPositions[0];
            storedPositions.RemoveAt(0);
        }

        if (Input.GetButton("Jump") && ((Mathf.Abs(rigbod.velocity.y) < 0.001f) || canFakeDoubleJump))
        {
            rigbod.velocity = new Vector3(0, JumpForce, 0);
            canFakeDoubleJump = false;
            //animator.SetTrigger("Jump");
            //FMODUnity.RuntimeManager.PlayOneShot("event:/jumpD");
        }
        else if (Input.GetKey(KeyCode.J)) // press J to rewind
        {
            StartCoroutine(Rewind());
        }
        else
        {
            transform.position += dir * speed * Time.deltaTime;
            //if (dir.x < 0)
            //{
            //    //character faces left
            //    transform.localRotation = Quaternion.Euler(0, 180, 0);
            //}

            //if (dir.x > 0)
            //{
            //    //character faces right
            //    transform.localRotation = Quaternion.Euler(0, 0, 0);
            //}
        }
    }

    //public void RestartScene()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    private IEnumerator Rewind()
    {
        transform.position = ghost.transform.position; // rewind to ghost's position
        ghost.SetActive(false); // despawn ghost
        canFakeDoubleJump = true; // can jump after rewind

        yield return null;
        yield return new WaitForSeconds(rewind_cooldown); // ghost respawn cooldown

        ghost.SetActive(true); // spawns ghost
        ghost.transform.position = transform.position; // ghost respawn wherever the player is
        storedPositions = new List<Vector3>();
    }
}
