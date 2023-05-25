using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private GameObject player;
    public GameObject effect;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void OnTriggerEnter(Collider col)
    {
        if(col.tag == ("Player"))
        {
            player.GetComponent<PlayerControl>().playerHP -= 2;
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
