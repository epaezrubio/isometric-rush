using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace IsoRush.Player
{
    public class PlayerFX : MonoBehaviour
    {
        [SerializeField]
        List<AudioClip> _destroyAudioClips = new List<AudioClip>();

        [SerializeField]
        List<AudioClip> _jumpAudioClips = new List<AudioClip>();


        private int _currentDestroyClip = 0;

        private int _currentJumpClip = 0;

        [SerializeField]
        AudioSource _audioSource;

        public void PlayDestroySound()
        {
            _audioSource.PlayOneShot(_destroyAudioClips[_currentDestroyClip]);

            _currentDestroyClip = (_currentDestroyClip + 1) % _destroyAudioClips.Count;
        }

        internal void PlayJumpSound()
        {
            
            _audioSource.PlayOneShot(_jumpAudioClips[_currentJumpClip]);

            _currentJumpClip = (_currentJumpClip + 1) % _jumpAudioClips.Count;
        }
    }
}
