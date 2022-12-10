using System.Collections.Generic;
using UnityEngine;

namespace VVVVVV
{
    public enum BGM
    {
        PUSHINGONWARDS = 1,
    }

    public class SoundManager : Utils.SingletonMonoBehaviour<SoundManager>
    // public class SoundManager : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> bgmList;

        private AudioSource bgmPlayer => transform.GetChild(0).GetComponent<AudioSource>();
        private AudioSource sfxPlayer => transform.GetChild(1).GetComponent<AudioSource>();

        public void Play(BGM track)
        {
            bgmPlayer.clip = bgmList[(int)track];
            bgmPlayer.Play();
        }
        public void PlayEffect(AudioClip sfx)
        {
            sfxPlayer.PlayOneShot(sfx);
        }
    }
}