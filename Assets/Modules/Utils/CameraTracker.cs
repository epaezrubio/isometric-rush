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

        void Update()
        {
            // var playerXDampling =
            //     Mathf.MoveTowards(transform.position.x,, 0.5f)
            //     * Time.deltaTime
            //     * 50f;

            transform.position =
                new Vector3(
                     _playerTransform.transform.position.x,
                    0,
                    _gameState.GameTime.Value * _gameState.ScrollSpeed.Value
                ) + _gameState.CameraPosition.Value;

            _camera.orthographicSize = _gameState.CameraSize.Value;
        }
    }
}
