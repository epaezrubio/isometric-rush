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

        void Start()
        {
            _gameState.CameraSize
                .Subscribe(cameraSize =>
                {
                    _camera.orthographicSize = cameraSize;
                })
                .AddTo(this);

            _gameState.CameraPosition
                .Subscribe(cameraPosition =>
                {
                    transform.position = cameraPosition;
                })
                .AddTo(this);
        }
    }
}
