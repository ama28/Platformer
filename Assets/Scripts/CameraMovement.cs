using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// moves camera with a bit of smooth delay
/// also bounded by puzzle room (rectangular)
/// </summary>

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public float smoothing = 0.1f;
    public Vector2 bottom_left_coord; // drag camera box to bottom left & upper right of room
    public Vector2 upper_right_coord; // such that the edges align and grab the positions

    private Vector3 target_position;

    void LateUpdate()
    {
        target_position = player.transform.position;
        target_position.x = Mathf.Clamp(target_position.x, bottom_left_coord.x, upper_right_coord.x);
        target_position.y = Mathf.Clamp(target_position.y, bottom_left_coord.y, upper_right_coord.y);
        target_position.z = -10;

        transform.position = Vector3.Lerp(transform.position, target_position, smoothing);

    }
}
