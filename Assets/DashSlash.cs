using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSlash : MonoBehaviour
{
    Rigidbody2D rigbod;
    public float dashSpeed;
    float dashTime;
    public float startDashTime;
    int dir;
    bool dashed;
    Vector3 direction;
    //int dashCount;
    bool isGrounded; 

    void Start()
    {
        rigbod = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        dashed = false;
        //dashCount = 1;
        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(dir == 0 && Input.GetKey(KeyCode.LeftShift)){
            if(Input.GetKey(KeyCode.A))
                dir = 1;
            if(Input.GetKey(KeyCode.D))
                dir = 2;
            if(Input.GetKey(KeyCode.W))
                dir = 3;
            if(Input.GetKey(KeyCode.S))
                dir = 4;
        }
        else{
            if(dashTime <= 0){
                dir = 0;
                dashTime = startDashTime;
                rigbod.velocity = Vector2.zero;
                dashed = false;
            }
            else{
                dashTime -= Time.deltaTime;
                dashed = true;
                if(isGrounded)
                {
                    if(dir == 1)
                        //rigbod.AddForce(Vector2.left * dashSpeed, ForceMode2D.Impulse);
                        rigbod.velocity = Vector2.left * dashSpeed;
                    if(dir == 2)
                        rigbod.velocity = Vector2.right * dashSpeed;
                    if(dir == 3){
                        rigbod.velocity = Vector2.up * dashSpeed;
                        isGrounded = false;
                    }
                    if(dir == 4)
                        rigbod.velocity = Vector2.down * dashSpeed;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && dashed)
        {
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}
