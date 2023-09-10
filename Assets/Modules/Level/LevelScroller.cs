using System.Collections.Generic;
using IsoRush.State;
using UnityEngine;
using VContainer;

namespace IsoRush.Level
{
    public class LevelScroller : MonoBehaviour
    {
        [Inject]
        private GameState _gameState;

        [SerializeField]
        [Min(1)]
        private float _length = 20;

        [SerializeField]
        [Min(0.1f)]
        private float _gridSize = 1;

        [SerializeField]
        private LevelChunk _chunkPrefab;

        private List<LevelChunk> _scrollElements = new List<LevelChunk>();

        void Start()
        {
            _scrollElements.Clear();

            for (int i = 0; i < _length / _gridSize; i++)
            {
                LevelChunk instance = Instantiate(_chunkPrefab, transform);
                instance.regularIndex = i;
                instance.index.Value = i;

                _scrollElements.Add(instance);
            }
        }

        void Update()
        {
            if (_gameState == null) {
                return;
            }

            float gameTime = _gameState.GameTime.Value;

            int elementsCount = _scrollElements.Count;
            int scrollIndex = (int)(gameTime / _gridSize);

            for (int i = 0; i < elementsCount; i++)
            {
                LevelChunk element = _scrollElements[i];

                int scrollLoops = (scrollIndex - element.regularIndex) / elementsCount;
                element.index.Value = scrollLoops * elementsCount + element.regularIndex;

                float offsetScroll =
                    (scrollIndex - element.index.Value) * _gridSize + gameTime % _gridSize;

                element.transform.localPosition = new Vector3(
                    offsetScroll - _length * 0.5f,
                    element.transform.localPosition.y,
                    element.transform.localPosition.z
                );
            }
        }

        void OnDrawGizmos()
        {
            float width = 50f;

            Gizmos.DrawLine(
                transform.position + new Vector3(_length * 0.5f, 0, -width * 0.5f),
                transform.position + new Vector3(_length * 0.5f, 0, width * 0.5f)
            );
            Gizmos.DrawLine(
                transform.position + new Vector3(_length * 0.5f, 0, width * 0.5f),
                transform.position + new Vector3(-_length * 0.5f, 0, width * 0.5f)
            );
            Gizmos.DrawLine(
                transform.position + new Vector3(-_length * 0.5f, 0, width * 0.5f),
                transform.position + new Vector3(-_length * 0.5f, 0, -width * 0.5f)
            );
            Gizmos.DrawLine(
                transform.position + new Vector3(-_length * 0.5f, 0, -width * 0.5f),
                transform.position + new Vector3(_length * 0.5f, 0, -width * 0.5f)
            );

            float gameTime = _gameState?.GameTime.Value ?? 0;
            float offset = gameTime % _gridSize;
            float start = -_length / _gridSize / 2 + offset;
            float end = _length / _gridSize / 2 + offset;

            for (float i = start; i < end; i += _gridSize)
            {
                Gizmos.DrawLine(
                    transform.position + new Vector3(i * _gridSize, 0, -width * 0.5f),
                    transform.position + new Vector3(i * _gridSize, 0, width * 0.5f)
                );
            }
        }
    }
}
