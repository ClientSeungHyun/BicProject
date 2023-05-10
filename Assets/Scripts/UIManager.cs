using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<Image> healths;
    public Image boostGage;

    public bool boostActive;
    public Image boostImage;

    public PlayerControl player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        boostActive = false;
        boostImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        boostGage.fillAmount = player.PlayerEg() / 100f;
    }

    public void BoostOnOff(bool b)
    {
        boostActive = b;
        boostImage.gameObject.SetActive(boostActive);
    }

   
}
