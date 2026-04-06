using UnityEngine;
using Singeltons;

namespace Managers
{
    public class AudioManager : SingletonPersistant<AudioManager>
    {
        public void PlaySFX(string SFXName) {}
        public void PlayMusic(string MusicName) {}
    }    
}

