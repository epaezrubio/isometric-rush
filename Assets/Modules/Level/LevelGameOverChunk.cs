using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace IsoRush.Level
{
    public class LevelGameOverChunk : MonoBehaviour
    {
        [SerializeField]
        private Collider _gameOverCollider;

        void Start()
        {
            _gameOverCollider
                .OnTriggerEnterAsObservable()
                .Where(other =>
                {
                    return other.CompareTag("Player");
                })
                .Subscribe(_ =>
                {
                    LevelScroller.instance.Scroll = 0;
                });
        }
    }
}
