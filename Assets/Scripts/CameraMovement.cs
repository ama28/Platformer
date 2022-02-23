using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// moves camera with a bit of smooth delay
/// </summary>

public class CameraMovement : MonoBehaviour
{

    private Transform transform_cam;

    public GameObject player;
    private Vector3 player_position;
    //public GameObject player2;

    //movement stuff
    //private Vector3 dir;
    //public float speed = 10f;

    public float smoothing = 0.1f;

    void Start()
    {
        transform_cam = GetComponent<Transform>();
    }

    //todo camera should follow the players in the future
    //void FixedUpdate()
    //{
    //    /*float horizontal = Input.GetAxisRaw("Horizontal");
    //    dir = new Vector3(horizontal, 0f, 0f).normalized;

    //    transform.position += dir * speed * Time.deltaTime;*/

    //    //try to have the camera follows the player
    //    //players locations
    //    float x1 = player1.transform.position.x;
    //    float x2 = player2.transform.position.x;

    //    transform.position = new Vector3((x1 + x2) / 2, transform.position.y, transform.position.z);
    //}


    // might need to bound camera to puzzle room later
    void LateUpdate()
    {
        if (transform.position.x != player.transform.position.x | transform.position.y != player.transform.position.y)
        {
            player_position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, player_position, smoothing);
        }
            
    }

}
