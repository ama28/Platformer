using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    private Rigidbody2D rigbod;
    private bool is_open;
    public float door_open_time = 3f;
    private float walking_speed;

    void Start()
    {
        walking_speed = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().baseSpeed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        rigbod = collision.gameObject.GetComponent<Rigidbody2D>();

        if (!is_open && (collision.CompareTag("Player") || collision.CompareTag("Ghost")))
        {
            if (rigbod.velocity.x > walking_speed)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(3).gameObject.SetActive(false);
                is_open = true;
                StartCoroutine(ReactivateDoor(1));
            }
            else if (rigbod.velocity.x < -walking_speed)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(3).gameObject.SetActive(false);
                is_open = true;
                StartCoroutine(ReactivateDoor(2));
            }
        }
    }

    private IEnumerator ReactivateDoor(int child)
    {
        yield return new WaitForSeconds(door_open_time);

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(child).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(true);
        is_open = false;
    }


}
