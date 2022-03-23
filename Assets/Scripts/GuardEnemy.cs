using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEnemy : MonoBehaviour
{
    public float speed;
    Transform target;
    private Animator myAnim;
    public float turnTime = 2f; 
    Vector2 currentTarget;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        myAnim = GetComponent<Animator>();
        currentTarget = new Vector2(target.position.x, transform.position.y);
    }

    void TurnTowardPlayer()
    {
        if(turnTime <= 0){
            if (target.position.x > transform.position.x)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                currentTarget = new Vector2(target.position.x, transform.position.y);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                currentTarget = new Vector2(target.position.x, transform.position.y);
            }
            turnTime = 2f;
        }
    }

    void Update()
    {
        turnTime -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
        myAnim.SetFloat("GuardSpeed", Mathf.Abs(target.position.x - transform.position.x));
        TurnTowardPlayer();
    }
}
