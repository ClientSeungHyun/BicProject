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

    public Image[] subButtons;

    private GameManagers gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
        ClickNormal();
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
        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("SceneManager"));
        LoadingSceneManager.LoadScene("TitleScene");
    }

    public void ClickSlow()
    {
        gameManagerScript.subSpeed = 0.3f;
        subButtons[0].color = Color.red;
        subButtons[1].color = Color.white;
        subButtons[2].color = Color.white;
    }
    public void ClickNormal()
    {
        gameManagerScript.subSpeed = 0.5f;
        subButtons[0].color = Color.white;
        subButtons[1].color = Color.red;
        subButtons[2].color = Color.white;
    }
    public void ClickFast()
    {
        gameManagerScript.subSpeed = 0.8f;
        subButtons[0].color = Color.white;
        subButtons[1].color = Color.white;
        subButtons[2].color = Color.red;
    }

    public void ButtonColorChange(Button b, Color c)
    {
        ColorBlock col = b.colors;
        col.normalColor = c;
        b.colors = col;
    }
}
