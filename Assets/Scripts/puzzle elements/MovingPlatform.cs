using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public float speed = 2f;
    public float distance = 5f;
    private float mid;
    public bool isHorizontal = true;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (isHorizontal)
        {
            mid = transform.position.x + distance / 2;
        }
        else
        {
            mid = transform.position.y + distance / 2;
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isHorizontal)
        {
            transform.position = new Vector3(mid - Mathf.PingPong(Time.time * speed, distance), transform.position.y, transform.position.z);
        }
        else if (!isHorizontal)
        {
            transform.position = new Vector3(transform.position.x, mid - Mathf.PingPong(Time.time * speed, distance), transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.transform.parent = null;
        }
    }
}
