using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyProjectile : MonoBehaviour
{
    public float fire_speed = 20f; 
    Collider2D myCollider;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        GameObject ghost = GameObject.FindGameObjectWithTag("Ghost");
        Physics2D.IgnoreCollision(myCollider, ghost.GetComponent<Collider2D>());
    }
    void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * fire_speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Destroy(other.gameObject);
            Destroy(gameObject);
            if (other.gameObject.GetComponent<playHealth>().canDamage)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Die");
                other.gameObject.GetComponent<playHealth>().currentHealth -= 1;
            }
                
            //RestartScene();
        }
        else if (other.gameObject.name != "CameraConfiner")
        {
            Destroy(gameObject);
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
