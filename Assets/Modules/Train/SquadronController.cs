using System.Collections.Generic;
using System.Linq;
using IsoRush.State;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using VContainer;

namespace IsoRush.Train
{
    public class SquadronController : MonoBehaviour
    {
        [Inject]
        private GameState _gameState;

        [SerializeField]
        private float _startTime = 0f;

        [SerializeField]
        private float _duration = 10f;

        [SerializeField]
        private float _distance = 10f;

        void Update()
        {
            float gameTime = _gameState.GameTime.Value;

            if (gameTime < _startTime || gameTime > _startTime + _duration)
            {
                return;
            }

            float offset = math.remap(_startTime, _startTime + _duration, 0, 1, gameTime);

            transform.localPosition = _distance * offset * transform.forward;
        }
    }
}
