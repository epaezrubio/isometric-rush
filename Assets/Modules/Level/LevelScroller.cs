using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace IsoRush.Level
{
    public class LevelScroller : MonoBehaviour
    {
        [SerializeField]
        [Min(1)]
        private float _length = 20;

        [SerializeField]
        [Min(0.1f)]
        private float _gridSize = 1;

        [SerializeField]
        [Min(0)]
        private float _scroll = 0;

        [SerializeField]
        private float _scrollSpeed = 10f;

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
                instance.index = i;

                _scrollElements.Add(instance);
            }
        }

        void Update()
        {
            _scroll+= Time.deltaTime * _scrollSpeed;

            int elementsCount = _scrollElements.Count;
            int scrollIndex = (int)(_scroll / _gridSize);

            for (int i = 0; i < elementsCount; i++)
            {
                LevelChunk element = _scrollElements[i];

                int scrollLoops = (scrollIndex - element.regularIndex) / elementsCount;

                element.index = scrollLoops * elementsCount + element.regularIndex;

                float offsetScroll =
                    (scrollIndex - element.index) * _gridSize + _scroll % _gridSize;

                element.transform.localPosition = new Vector3(
                    offsetScroll - _length * 0.5f,
                    element.transform.localPosition.y,
                    element.transform.localPosition.z
                );
            }

            // _scroll += Time.deltaTime * 10;
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

            float offset = _scroll % _gridSize;
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
