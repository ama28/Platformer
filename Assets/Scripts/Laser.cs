using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 1f;
    float pos_x;
    float pos_y;
    public bool horMovement;
    private float initializationTime;

    void Awake()
    {
        pos_x = transform.position.x;
        pos_y = transform.position.y;
        initializationTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        float timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;
        if (horMovement)
            transform.position = new Vector2(pos_x + Mathf.PingPong(timeSinceInitialization * speed, 15f), transform.position.y);
        else
            transform.position = new Vector2(transform.position.x, pos_y - Mathf.PingPong(timeSinceInitialization * speed, 5f));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Destroy(other.gameObject);
            //RestartScene();
            //minus 1 health
            if (other.gameObject.GetComponent<playHealth>().canDamage)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Die");
                other.gameObject.GetComponent<playHealth>().currentHealth -= 1;
            }
                
        }
    }
}
