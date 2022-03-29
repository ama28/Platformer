using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BossHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int currentHealth;
    int prevHealth;
    public bool canDamage;
    int cooldown;
    GameObject tempLaser1;
    GameObject tempLaser2;

    [SerializeField]
    GameObject horMoveLaser;

    [SerializeField]
    GameObject verMoveLaser;

    public GameObject horLocation;
    public GameObject verLocation;

    void Start()
    {
        currentHealth = 4;
        prevHealth = 4;
        cooldown = 3;
        canDamage = true;
        //damaged = false;

        StartCoroutine(Flashing());
        horLocation = GameObject.Find("Laser1 Pos");
        verLocation = GameObject.Find("Laser2 Pos");
    }

    void MovePosition()
    {
        if (currentHealth == 3)
        {
            GetComponent<BossScript>().enabled = false;
            transform.position = new Vector3(-3.29f, 1.615174f, 0);
            tempLaser1 = Instantiate(horMoveLaser, horLocation.transform.position, transform.rotation);
        }
        if (currentHealth == 2)
        {
            Destroy(tempLaser1);
            transform.position = new Vector3(0.53f, 1.505192f, 0);
            tempLaser2 = Instantiate(verMoveLaser, verLocation.transform.position, transform.rotation);
        }
        if (currentHealth == 1)
        {
            Destroy(tempLaser2);
            GetComponent<BossScript>().enabled = true;
            transform.position = new Vector3(0.25f, -2.044801f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //restart scene if player dies
        if (currentHealth <= 0)
            Destroy(gameObject);

        if (prevHealth != currentHealth)//damaged by player
        {
            canDamage = false;
            prevHealth = currentHealth;
            MovePosition();
            StartCoroutine(Reset_canDamage());
        }

        if(canDamage)
            GetComponent<SpriteRenderer>().color = Color.white;
    }
    private IEnumerator Reset_canDamage() {
        yield return new WaitForSeconds(cooldown);
        canDamage = true;
    }

    private IEnumerator Flashing() {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            if (!canDamage)
            {
                if (GetComponent<SpriteRenderer>().color == Color.white)
                    GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.5f);
                else
                    GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
