using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public float followSpeed;
    public GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        followSpeed = 1;
        //transform.forward = Vector3.right;
    }

    // Update is called once per frame
    void Update()
    {
        //lerp to player position
        transform.position = Vector3.Lerp(transform.position, player.transform.position, followSpeed * Time.deltaTime);
        //reset z so it's visible
        transform.position = new Vector3(transform.position.x, transform.position.y, -4f);
        //transform.LookAt(player.transform.position, Vector3.up);

        Vector3 playPos = player.transform.position;
        Vector3 spiritPos = transform.position;

        // float cosAngle = (spiritPos.y - playPos.y) / (Mathf.Sqrt(spiritPos.y * spiritPos.y + spiritPos.x * spiritPos.x));
        Vector3 d = playPos - spiritPos;
        float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
        //float tanAngle = (spiritPos.y - playPos.y) / (spiritPos.x - playPos.x);
        //float angle = Mathf.Atan(tanAngle) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10 * Time.deltaTime);

       // angle -= Mathf.PI / 2; // point forward

        //print(angle / Mathf.PI * 180 + " " + Mathf.Rad2Deg * angle);
        //transform.localEulerAngles = new Vector3(0, 0, angle / Mathf.PI * 180);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //do damage
            FMODUnity.RuntimeManager.PlayOneShot("event:/Die");
            collision.gameObject.GetComponent<playHealth>().currentHealth -= 1;
        }
    }
}
