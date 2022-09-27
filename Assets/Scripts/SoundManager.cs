using System.Collections.Generic;
using UnityEngine;

namespace VVVVVV
{
    public enum BGM
    {
        PUSHINGONWARDS = 1,
    }

    public class SoundManager : MonoBehaviour
    {
        private AudioSource bgmPlayer;
        private AudioSource sfxPlayer;

        [SerializeField] private List<AudioClip> bgmList;

        void Awake()
        {
            bgmPlayer = transform.GetChild(0).GetComponent<AudioSource>();
            sfxPlayer = transform.GetChild(1).GetComponent<AudioSource>();
        }

        public void PlayTrack(BGM track)
        {
            bgmPlayer.clip = bgmList[(int)track];
            bgmPlayer.Play();
        }
    }
}