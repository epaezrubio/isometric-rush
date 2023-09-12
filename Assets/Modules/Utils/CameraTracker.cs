using System.Collections;
using System.Collections.Generic;
using IsoRush.State;
using UniRx;
using UnityEngine;
using VContainer;

namespace IsoRush.Utils
{
    public class CameraTracker : MonoBehaviour
    {
        [Inject]
        private GameState _gameState;

        [SerializeField]
        private Camera _camera;

        void Update()
        {
            transform.position =
                new Vector3(0, 0, _gameState.GameTime.Value * _gameState.ScrollSpeed.Value)
                + _gameState.CameraPosition.Value;

            _camera.orthographicSize = _gameState.CameraSize.Value;
        }
    }
}
