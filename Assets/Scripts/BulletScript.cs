using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BulletScript : MonoBehaviour
{
    [SerializeField] Rigidbody2D bullet_body;

    private Collider2D bullet_collider;
    public Vector3 dir;
    public float fire_speed; 

    void Awake()
    {
        bullet_collider = GetComponent<Collider2D>();
        GameObject ghost = GameObject.FindGameObjectWithTag("Ghost");
        Physics2D.IgnoreCollision(bullet_collider, ghost.GetComponent<Collider2D>());
    }

    void Update()
    {
        transform.position += dir * Time.deltaTime * fire_speed; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Destroy(other.gameObject);
            Destroy(gameObject);
            if (other.gameObject.GetComponent<playHealth>().canDamage)
                FMODUnity.RuntimeManager.PlayOneShot("event:/Die");
                other.gameObject.GetComponent<playHealth>().currentHealth -= 1;
            //RestartScene();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
