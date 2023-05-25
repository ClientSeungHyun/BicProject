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

    public Image storyDialog;
    private StoryScript storyScript;

    private PlayerControl player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        storyScript = GameObject.Find("Story").GetComponent<StoryScript>();
        boostActive = false;
        boostImage.gameObject.SetActive(false);
        storyDialog.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (storyScript.IsStoryComplete())
            storyDialog.gameObject.SetActive(false);
        
        boostGage.fillAmount = player.PlayerEg() / 100f;
    }

    public void BoostOnOff(bool b)
    {
        boostActive = b;
        boostImage.gameObject.SetActive(boostActive);
    }

   
}
