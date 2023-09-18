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
        private AudioSource _musicAudioSource;

        [SerializeField]
        private AudioSource _warpAudioSource;

        private float _targetVolume = 1f;

        private float _fadeSpeed = 0.2f;

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
            _musicAudioSource.volume = Mathf.MoveTowards(
                _musicAudioSource.volume,
                _targetVolume,
                _fadeSpeed * Time.deltaTime
            );
        }

        public void FadeIn()
        {
            if (_musicAudioSource)
            {
                _musicAudioSource.volume = 0;
            }

            _targetVolume = 1;
            _fadeSpeed = 0.4f;
        }

        public void FadeOut(float fadeSpeed = 1f)
        {
            _targetVolume = 0;
            _fadeSpeed = fadeSpeed;
        }

        public void Play()
        {
            if (_musicAudioSource != null && _musicAudioSource.clip != null)
            {
                _musicAudioSource.Play();
            }

            _playing = true;
        }

        public void SetMusicTime(float gameTime)
        {
            _musicAudioSource.time = gameTime;
        }

        public void Stop()
        {
            _musicAudioSource?.Stop();
            _playing = false;
        }

        public void StartWarpSound()
        {
            _warpAudioSource.Play();
        }

        public void StopWarpSound()
        {
            _warpAudioSource.Stop();
        }
    }
}
