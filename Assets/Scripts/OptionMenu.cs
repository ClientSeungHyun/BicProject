using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class OptionMenu : MonoBehaviour
{
    private GameManagers gameManagerScript;
    public AudioMixer audioMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Toggle subToggle;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        gameManagerScript.subSpeed = 0.2f;
    }
    public void ClickNormal()
    {
        gameManagerScript.subSpeed = 0.5f;
    }
    public void ClickFast()
    {
        gameManagerScript.subSpeed = 0.8f;
    }
}
