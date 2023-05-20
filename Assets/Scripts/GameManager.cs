using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    public int chooseCard;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (GameObject.Find("Player"))
        {
            player = GameObject.Find("Player");
        }
    }

    private void Update()
    {
        if (chooseCard == 1 && player)
        {
            player.GetComponent<PlayerControl>().HPLevelUp();
            chooseCard = 0;
        }   
        else if (chooseCard == 2 && player)
        {
            player.GetComponent<PlayerControl>().GunLevelUp();
            chooseCard = 0;
        }
        else if (chooseCard == 3 && player)
        {
            player.GetComponent<PlayerControl>().EnergyLevelUp();
            chooseCard = 0;
        }
    }
}
