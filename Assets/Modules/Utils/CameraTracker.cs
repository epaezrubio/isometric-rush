using System.Collections;
using System.Collections.Generic;
using IsoRush.Player;
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

        [SerializeField]
        private PhysicsPlayerMover _playerTransform;

        public bool xOverride = false;

        public float trackedXOverride = 0;

        void FixedUpdate()
        {
            float dampedX = xOverride ? trackedXOverride : _playerTransform.transform.position.x;

            transform.position =
                new Vector3(dampedX, 0, _gameState.GameTime.Value * _gameState.ScrollSpeed.Value)
                + _gameState.CameraPosition.Value;

            _camera.orthographicSize = _gameState.CameraSize.Value;
        }
    }
}
