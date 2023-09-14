using System;
using System.Collections;
using System.Collections.Generic;
using IsoRush.State;
using UnityEngine;
using VContainer;

namespace IsoRush.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [Inject]
        private GameState _gameState;

        [SerializeField]
        private AudioSource _audioSource;

        private float _targetVolume = 1f;

        private bool _playing = false;

        void Start()
        {
            if (_playing)
            {
                Play();
            }
        }

        void Update()
        {
            _audioSource.volume = Mathf.MoveTowards(
                _audioSource.volume,
                _targetVolume,
                0.08f * Time.deltaTime
            );
        }

        public void FadeIn()
        {
            if (_audioSource)
            {
                _audioSource.volume = 0;
            }

            _targetVolume = 1;
        }

        public void FadeOut()
        {
            _targetVolume = 0;
        }

        public void Play()
        {
            if (_audioSource != null && _audioSource.clip != null)
            {
                _audioSource.Play();
            }

            _playing = true;
        }

        public void SetMusicTime(float gameTime)
        {
            _audioSource.time = gameTime;
        }

        public void Stop()
        {
            _audioSource?.Stop();
            _playing = false;
        }
    }
}