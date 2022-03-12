using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWeak : MonoBehaviour
{
    public float speed = 3f;
    public float distance = 5f;
    private float mid_x;
    private Rigidbody2D rb2d;

    void Start()
    {
        mid_x = transform.position.x;
        rb2d = GetComponent<Rigidbody2D>();

        rb2d.velocity = new Vector3(speed, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.x < mid_x - distance/2)
        {
            rb2d.velocity = new Vector3(speed, 0, 0);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (transform.position.x > mid_x + distance / 2)
        {
            rb2d.velocity = new Vector3(-speed, 0, 0);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
