using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] [CanBeNull] private Slider volumeMusic;

    [SerializeField] [CanBeNull] private Slider volumeEffect;
    public GameObject button;

    private AudioSource music;
    [CanBeNull] private AudioSource effect;

    // Start is called before the first frame update
    void Start()
    {
        
        if(SceneManager.GetSceneByName("SaraScene").IsValid())
            button.SetActive(true);
        
        if (volumeEffect && volumeMusic)
        {
            LoadMusic();
            LoadEffect();
        }

        music = GameObject.Find("Music").GetComponent<AudioSource>();
        if (effect)
            effect = GameObject.Find("Effect").GetComponent<AudioSource>();
        SetVolume();
    }

    public void SaveMusic()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeMusic!.value);
        SetVolume();
    }

    public void SaveEffect()
    {
        PlayerPrefs.SetFloat("effectVolume", volumeEffect!.value);
        SetVolume();
    }

    private void LoadMusic()
    {
        volumeMusic!.value = PlayerPrefs.GetFloat("musicVolume", 50);
    }

    private void LoadEffect()
    {
        volumeEffect!.value = PlayerPrefs.GetFloat("effectVolume", 50);
    }

    public void SetVolume()
    {
        if (music)
            music.volume = PlayerPrefs.GetFloat("musicVolume");

        if (effect)
            effect.volume = PlayerPrefs.GetFloat("effectVolume");
    }

    public void GoBack()
    {
        SceneManager.UnloadSceneAsync("Options");
    }
    
    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
        SceneManager.UnloadSceneAsync("Options");
        SceneManager.UnloadSceneAsync("SaraScene");
    }
}