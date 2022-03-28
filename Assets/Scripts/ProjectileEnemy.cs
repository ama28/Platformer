using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;

    public float fireRate;
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
            FMODUnity.RuntimeManager.PlayOneShot("event:/Shoot", transform.position);
            Instantiate(bullet, new Vector2(transform.position.x - 1f, transform.position.y) , transform.rotation);
            nextFire = Time.time + fireRate;
        }
    }
}
