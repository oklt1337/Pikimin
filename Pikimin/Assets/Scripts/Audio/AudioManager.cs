using System;
using System.Collections;
using SaveData;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager AudioManagerInstance;

        [SerializeField] private AudioSource backgroundSound;
        [SerializeField] private AudioSource sfx;
        [SerializeField] private AudioMixer mixer;

        [Header("Clips")]
        [SerializeField] private AudioClip[] rivalSounds;
        [SerializeField] private AudioClip[] trainerSounds;
        [SerializeField] private AudioClip[] dojoChefSounds;
        [SerializeField] private AudioClip[] areaSounds;
        [SerializeField] private AudioClip[] leftOverSounds;
        [SerializeField] private AudioClip[] buttonOverSounds;

        public float audioValue;
        
        public AudioSource BackgroundSound => backgroundSound;
        
        public AudioSource Sfx => sfx;

        public AudioClip[] RivalSounds => rivalSounds;

        public AudioClip[] TrainerSounds => trainerSounds;
        
        public AudioClip[] DojoChefSounds => dojoChefSounds;

        public AudioClip[] AreaSounds => areaSounds;
        
        public AudioClip[] LeftOverSounds => leftOverSounds;

        public AudioClip[] ButtonOverSounds => buttonOverSounds;

        private void Awake()
        {
            if (AudioManagerInstance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                AudioManagerInstance = this;
                DontDestroyOnLoad(this);
            }
        }

        public void PlayAudioClip(bool sfx, AudioClip clip)
        {
            if (sfx)
            {
                StartCoroutine(PlaySound(clip));
            }
            else
            {
                BackgroundSound.clip = clip;
                backgroundSound.Play();
            }
        }

        private IEnumerator PlaySound(AudioClip clip)
        {
            sfx.clip = clip;
            sfx.Play();
            
            yield return new WaitForSeconds(clip.length);
            
            sfx.clip = null;
        }
        
        public void SetLevel(float sliderValue)
        {
            mixer.SetFloat("MyMaster", Mathf.Log10(sliderValue) * 20);
        }
    }
}
