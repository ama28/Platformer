using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChange : MonoBehaviour
{
    public Vector2 new_bottom_left;
    public Vector2 new_upper_right;
    public Vector3 new_player_position;
    private CameraMovement cam_movement;


    // Start is called before the first frame update
    void Start()
    {
        cam_movement = Camera.main.GetComponent<CameraMovement>();
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.CompareTag("Player") || obj.CompareTag("Ghost"))
        {
            cam_movement.bottom_left_coord = new_bottom_left;
            cam_movement.upper_right_coord = new_upper_right;
            obj.transform.position = new_player_position;
        }
    }
}
