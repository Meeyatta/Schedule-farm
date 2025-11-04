using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundName
{
    Test
}

public class AudioManager : MonoBehaviour
{
    public List<Sound> Sounds = new List<Sound>();
    [System.Serializable]
    public class Sound
    {
        public string sName;
        public SoundName eName;
        public AudioClip Clip;
        public float Volume;
    }

    public AudioSource SourcePrefab;

    #region Singleton
    public static AudioManager Instance;
    void Singleton()
    {
        if (Instance != null) { Destroy(Instance); }
        else { Instance = this; }
    }

    void Awake()
    {
        Singleton();
    }
    #endregion

    public void PlaySound(SoundName name)
    {
        AudioClip clip = null;
        float volume = 0f;
        foreach (var v in Sounds)
        {
            if (name == v.eName) { clip = v.Clip; volume = v.Volume; break; }
        }

        if (clip == null || volume == 0f) return;

        AudioSource sound = Instantiate(SourcePrefab.gameObject, transform).GetComponent<AudioSource>();
        
        sound.volume = volume;
        sound.clip = clip;
        sound.Play();

        float duration = clip.length;
        Destroy(sound.gameObject, duration);
    }
}
