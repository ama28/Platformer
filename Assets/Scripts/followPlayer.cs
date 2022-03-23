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
    }

    // Update is called once per frame
    void Update()
    {
        //lerp to player position
        transform.position = Vector3.Lerp(transform.position, player.transform.position, followSpeed * Time.deltaTime);
        //reset z so it's visible
        transform.position = new Vector3(transform.position.x, transform.position.y, -4f);
    }
}
