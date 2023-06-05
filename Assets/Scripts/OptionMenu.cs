using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class OptionMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    public Button[] subButtons;

    private GameManagers gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
    }

    public void setBGMSounds()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(bgmSlider.value) * 20);
    }
    public void setSFXSounds()
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(sfxSlider.value) * 20);
    }
    public void GoTitle()
    {
        LoadingSceneManager.LoadScene("TitleScene");
    }

    public void ClickSlow()
    {
        gameManagerScript.subSpeed = 0.3f;
        ButtonColorChange(subButtons[0], Color.red);
        ButtonColorChange(subButtons[1], Color.white);
        ButtonColorChange(subButtons[2], Color.white);
    }
    public void ClickNormal()
    {
        gameManagerScript.subSpeed = 0.5f;
        ButtonColorChange(subButtons[0], Color.white);
        ButtonColorChange(subButtons[1], Color.red);
        ButtonColorChange(subButtons[2], Color.white);
    }
    public void ClickFast()
    {
        gameManagerScript.subSpeed = 0.8f;
        ButtonColorChange(subButtons[0], Color.white);
        ButtonColorChange(subButtons[1], Color.white);
        ButtonColorChange(subButtons[2], Color.red);
    }

    public void ButtonColorChange(Button b, Color c)
    {
        ColorBlock col = b.colors;
        col.normalColor = c;
        b.colors = col;
    }
}
