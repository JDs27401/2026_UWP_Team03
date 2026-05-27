using UnityEngine;
using Singeltons;

namespace Managers
{
    public class AudioManager : SingletonPersistant<AudioManager>
    {
        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField, Range(0f, 1f)] private float musicVolume = 1f;

        private AudioSource _musicSource;

        protected override void Awake()
        {
            base.Awake();

            if (Instance != this)
            {
                return;
            }

            _musicSource = GetComponent<AudioSource>();
            if (_musicSource == null)
            {
                _musicSource = gameObject.AddComponent<AudioSource>();
            }

            _musicSource.playOnAwake = false;
            _musicSource.loop = true;
            _musicSource.volume = musicVolume;
            _musicSource.clip = backgroundMusic;

            if (backgroundMusic != null && !_musicSource.isPlaying)
            {
                _musicSource.Play();
            }
        }

        public void PlaySfx(string sfxName) {}

        public void PlayMusic(string musicName) {}
    }    
}

