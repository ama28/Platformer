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
        currentHealth = 6;
        prevHealth = 6;
        cooldown = 3;
        canDamage = true;
        //damaged = false;

        StartCoroutine(Flashing());
        horLocation = GameObject.Find("Laser1 Pos");
        verLocation = GameObject.Find("Laser2 Pos");
    }

    void MovePosition()
    {
        if (currentHealth == 5)
        {
            GetComponent<BossScript>().enabled = false;
            transform.position = new Vector3(-5.5f, -1.8548f, 0);
            tempLaser1 = Instantiate(horMoveLaser, horLocation.transform.position, transform.rotation);
        }
        if (currentHealth == 4)
        {
            Destroy(tempLaser1);
            transform.position = new Vector3(2.2f, 1.6f, 0);
            tempLaser2 = Instantiate(verMoveLaser, verLocation.transform.position, transform.rotation);
        }
        if (currentHealth == 3)
        {
            Destroy(tempLaser2);
            GetComponent<BossScript>().enabled = true;
            transform.position = new Vector3(0.35f, -1.8548f, 0);
        }
        if (currentHealth == 2)
        {
            transform.position = new Vector3(6.38f, -0.01f, 0);
        }
        if (currentHealth == 1)
        {
            transform.position = new Vector3(-5.8f, -1.8548f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //restart scene if player dies
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("lab4");
        }

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
