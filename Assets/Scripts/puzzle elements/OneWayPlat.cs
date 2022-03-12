using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlat : MonoBehaviour
{
    private GameObject current_oneway_plat;
    private BoxCollider2D player_collider;

    void Start()
    {
        player_collider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S) && current_oneway_plat != null)
        {
            StartCoroutine(DisableCollision());
        }
    }

    private void OnCollisionEnter2D(Collision2D obj)
    {
        if(obj.gameObject.CompareTag("OneWayPlatform"))
        {
            current_oneway_plat = obj.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D obj)
    {
        if (obj.gameObject.CompareTag("OneWayPlatform"))
        {
            current_oneway_plat = null;
        }

    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D current_oneway_plat_collider = current_oneway_plat.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(current_oneway_plat_collider, player_collider);
        yield return new WaitForSeconds(.2f);
        Physics2D.IgnoreCollision(current_oneway_plat_collider, player_collider, false);

    }
}
