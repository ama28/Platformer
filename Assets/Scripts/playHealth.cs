using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int currentHealth;
    public int prevHealth;
    public bool canDamage;
    public int cooldown;
    public Image healthImage;
    void Start()
    {
        currentHealth = 3;
        prevHealth = 3;
        cooldown = 3;
        canDamage = true;
        healthImage = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
        //damaged = false;

        StartCoroutine(Flashing());
    }

    // Update is called once per frame
    void Update()
    {
        //restart scene if player dies
        if (currentHealth <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else
        {
            //change ui appearance
            //print(healthImage.mainTexture);
            var img = Resources.Load<Sprite>("heart " + currentHealth);
            //print(texture);
            //change image texture
            healthImage.sprite = img;
            
        }

        if (prevHealth != currentHealth)//damaged by some enemy
        {
            canDamage = false;
            prevHealth = currentHealth;
            StartCoroutine(Reset_canDamage());
        }

        if(canDamage)
            GetComponent<SpriteRenderer>().color = Color.white;
        //print(currentHealth);
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
