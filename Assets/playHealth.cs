using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int currentHealth;
    public int prevHealth;
    public bool canDamage;
    public int cooldown;
    void Start()
    {
        currentHealth = 3;
        prevHealth = 3;
        cooldown = 3;
        canDamage = true;
        //damaged = false;
    }

    // Update is called once per frame
    void Update()
    {
        //restart scene if player dies
        if(currentHealth <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (prevHealth != currentHealth)//damaged by some enemy
        {
            canDamage = false;
            prevHealth = currentHealth;
            StartCoroutine(Reset_canDamage());
        }

        print(currentHealth);
    }
    private IEnumerator Reset_canDamage() {
        yield return new WaitForSeconds(cooldown);
        canDamage = true;
    }
}
