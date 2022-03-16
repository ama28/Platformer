using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if(target.position.x - transform.position.x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else{
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
