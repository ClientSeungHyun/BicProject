using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image boostGage;
    public Image boostImage;
    public Image storyDialog;
    public List<Image> healths;

    public Image clearImage;
    public Image gameOverImage;

    private bool boostActive;
    private float time;

    private StoryScript storyScript;
    private PlayerControl player;
    private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        storyScript = GameObject.Find("Story").GetComponent<StoryScript>();
        boostActive = false;
        boostImage.gameObject.SetActive(false);
        storyDialog.gameObject.SetActive(true);
        clearImage.gameObject.SetActive(false);
        gameOverImage.gameObject.SetActive(false);

        playerInput = FindObjectOfType<PlayerInput>();
        playerInput.retryButton.gameObject.SetActive(false);
        playerInput.returnToTitleButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (storyScript.IsStoryComplete())
            storyDialog.gameObject.SetActive(false);

        //플레이어가 죽으면
        if (player.IsDeath())
        {
            FadeIn(gameOverImage);
            playerInput.retryButton.gameObject.SetActive(true);
            playerInput.returnToTitleButton.gameObject.SetActive(true);
        }
        //스테이지 클리어하면
        if (player.IsStageClear())
        {
            FadeIn(clearImage);
            playerInput.retryButton.gameObject.SetActive(true);
            playerInput.returnToTitleButton.gameObject.SetActive(true);
        }
        boostGage.fillAmount = player.PlayerEg() / 100f;
    }

    public void BoostOnOff(bool b)
    {
        boostActive = b;
        boostImage.gameObject.SetActive(boostActive);
    }

    private void FadeIn(Image fImage)
    {
        if (time < 2f)
        {
            fImage.gameObject.SetActive(true);
            fImage.color = new Color(1, 1, 1, time / 2f);
        }
        else
        {
            time = 2f;
        }
        time += Time.deltaTime;
    }
}