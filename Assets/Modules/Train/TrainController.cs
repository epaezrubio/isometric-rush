using System.Collections.Generic;
using System.Linq;
using IsoRush.State;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using VContainer;

namespace IsoRush.Train
{
    public class TrainController : MonoBehaviour
    {
        [Inject]
        private GameState _gameState;

        [SerializeField]
        private SplineContainer _splineComponent;

        [SerializeField]
        private float _startTime = 0f;

        [SerializeField]
        private float _duration = 10f;

        [SerializeField]
        private float _chunksSize = 1f;

        private List<Transform> _trainChunks;

        void Start()
        {
            _trainChunks = new List<Transform>();

            foreach (Transform child in transform)
            {
                _trainChunks.Add(child);

                child.localPosition = Vector3.zero;
                child.localRotation = Quaternion.identity;
            }
        }

        void Update()
        {
            float gameTime = _gameState.GameTime.Value;

            if (gameTime < _startTime || gameTime > _startTime + _duration)
            {
                return;
            }

            float offset = math.remap(_startTime, _startTime + _duration, 0, 1, gameTime);
            float chunkRatio = _chunksSize / _splineComponent.Spline.GetLength();

            for (int i = 0; i < _trainChunks.Count; i++)
            {
                float chunkOffset = offset - i * chunkRatio;

                Vector3 position = _splineComponent.Spline.EvaluatePosition(chunkOffset);
                Vector3 tangent = _splineComponent.Spline.EvaluateTangent(chunkOffset);
                Quaternion rotation = Quaternion.LookRotation(tangent);

                _trainChunks[i].localPosition = position;
                _trainChunks[i].localRotation = rotation;
            }
        }
    }
}
