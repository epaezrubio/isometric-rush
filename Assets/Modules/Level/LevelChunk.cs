using System.Collections.Generic;
using System.Runtime.InteropServices;
using UniRx;
using UnityEngine;

namespace IsoRush.Level
{
    public class LevelChunk : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> _chunks = new List<GameObject>();

        private GameObject _currentChunk = null;

        public ReactiveProperty<int> index = new ReactiveProperty<int>(0);

        public int regularIndex;

        void Start()
        {
            index
                .Subscribe(value =>
                {
                    if (_currentChunk != null)
                    {
                        Destroy(_currentChunk);
                    }

                    GameObject nextChunk = _chunks[0];

                    if (Random.Range(0, 10) <= 1)
                    {
                        nextChunk = _chunks[value % _chunks.Count];
                    }

                    // TODO: Replace with object pool
                    _currentChunk = Instantiate(nextChunk, transform);
                })
                .AddTo(this);
        }
    }
}
