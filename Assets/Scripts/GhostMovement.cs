using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IGNORE THIS FOR NOW
/// </summary>

public class GhostMovement : MonoBehaviour
{
    public GameObject player;
    public float speed = 1f;

    private Vector3 player_position;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        player_position = player.transform.position;

        if (transform.position.x != player_position.x | transform.position.y != player_position.y)
        {
            transform.position = Vector3.Lerp(transform.position, player_position, speed / 1000);
        }
    }
}
