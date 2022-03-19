using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEnemy : MonoBehaviour
{
    public float speed;
    Transform target;
    private Animator myAnim;
    public float delay = 2f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        myAnim = GetComponent<Animator>();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, transform.position.y), speed * Time.deltaTime);
        myAnim.SetFloat("GuardSpeed", Mathf.Abs(target.position.x - transform.position.x));

        if (target.position.x > transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
