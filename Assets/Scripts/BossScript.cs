using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;

    Vector3 direction;
    int count = 0;


    public float fireRate; //the higher the slower boss fires
    float nextFire;
    void Start()
    {
        nextFire = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTimeOver();
    }

    void CheckTimeOver()
    {
        if(Time.time > nextFire)
        {
            if (count % 5 == 0)
            {
                direction = new Vector3(-1, 0, 0);
                count += 1;
            }
            else if (count % 5 == 1)
            {
                direction = new Vector3(-1, 1, 0);
                count += 1;
            }
            else if (count % 5 == 2)
            {
                direction = new Vector3(0, 1, 0);
                count += 1;
            }
            else if (count % 5 == 3)
            {
                direction = new Vector3(1, 1, 0);
                count += 1;
            }
            else if (count % 5 == 4)
            {
                direction = new Vector3(1, 0, 0);
                count += 1;
            }

            if (count == 30)
            {
                count = 0;
            }
            FMODUnity.RuntimeManager.PlayOneShot("event:/Shoot", transform.position);



            if (count <= 10)
            {
                //fire up
                GameObject tempBullet = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation);
                tempBullet.GetComponent<BulletScript>().dir = Vector3.up;

                //fire left
                GameObject tempBullet1 = Instantiate(bullet, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), transform.rotation);
                tempBullet1.GetComponent<BulletScript>().dir = Vector3.left;

                //fire right
                GameObject tempBullet2 = Instantiate(bullet, new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), transform.rotation);
                tempBullet2.GetComponent<BulletScript>().dir = Vector3.right;

            }
            else if (count <= 20){
                GameObject tempBullet = Instantiate(bullet, new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z), transform.rotation);
                tempBullet.GetComponent<BulletScript>().dir = direction;
            }
            else
            {
                GameObject tempBullet = Instantiate(bullet, new Vector3(transform.position.x + direction.x * -1f, transform.position.y + direction.y, transform.position.z), transform.rotation);
                tempBullet.GetComponent<BulletScript>().dir = new Vector3(direction.x * -1f, direction.y, direction.z);
            }
            //time b4 fire again
            nextFire = Time.time + fireRate;
        }
    }
}
