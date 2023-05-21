using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class OptionMenu : MonoBehaviour
{
    private GameManagers gameManagerScript;

    public Slider bgmSlider;
    public Slider sfxSlider;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickSubtitle(bool active)
    {
        gameManagerScript.activeSubtitile = active;
    }
    
    public void setBGMSounds()
    {
        gameManagerScript.audioMixer.SetFloat("BGM", Mathf.Log10(bgmSlider.value) * 20);
    }
    public void setSFXSounds()
    {
        gameManagerScript.audioMixer.SetFloat("SFX", Mathf.Log10(bgmSlider.value) * 20);
    }
    public void GoTitle()
    {
        LoadingSceneManager.LoadScene("TitleScene");
    }
}
