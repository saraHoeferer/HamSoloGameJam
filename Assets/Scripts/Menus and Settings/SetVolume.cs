using UnityEngine;

namespace Menus_and_Settings
{
    public class SetVolume : MonoBehaviour
    {
        public AudioSource music;
        public AudioSource effect1;
        public AudioSource effect2;
        public AudioSource effect3;

        // Start is called before the first frame update
        void Start()
        {
            music.volume = PlayerPrefs.GetFloat("musicVolume");
            effect1.volume = PlayerPrefs.GetFloat("effectVolume");
            effect2.volume = PlayerPrefs.GetFloat("effectVolume");
            effect3.volume = PlayerPrefs.GetFloat("effectVolume");
        }
    }
}