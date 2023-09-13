using System;
using System.Collections;
using System.Collections.Generic;
using IsoRush.State;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace IsoRush.Managers
{
    public class CheatsManager : MonoBehaviour
    {
        [Inject]
        private GameState _gameState;

        [Inject]
        private AudioManager _audioManager;

        [SerializeField]
        private float _gameSpeed = 1f;

        [SerializeField]
        private float _gameTime = 0f;

        [SerializeField]
        private bool _enabled = false;

        void Start()
        {
            if (_enabled)
            {
                Write();
            }
        }

        void Update()
        {
            if (!_enabled)
            {
                Read();
            }
            else
            {
                _gameState.GameSpeed.Value = 0;
                _audioManager.Stop();
            }
        }

        void Read()
        {
            _gameSpeed = _gameState.GameSpeed.Value;
            _gameTime = _gameState.GameTime.Value;
        }

        [ContextMenu("Write")]
        void Write()
        {
            _gameState.GameTime.Value = _gameTime;
            _gameState.GameSpeed.Value = _gameSpeed;

            _audioManager.SetMusicTime(_gameTime);
            _audioManager.Play();

            _enabled = false;
        }
    }
}
