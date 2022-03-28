using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Spike : MonoBehaviour
{
    public float speed = 1f;
    private float pos_y;

    void Start()
    {
        pos_y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, pos_y - Mathf.PingPong(Time.time * speed, 0.6f));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Destroy(other.gameObject);
            //RestartScene();
            //minus 1 health
            if (other.gameObject.GetComponent<playHealth>().canDamage)
                FMODUnity.RuntimeManager.PlayOneShot("event:/Die");
                other.gameObject.GetComponent<playHealth>().currentHealth -= 1;
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
