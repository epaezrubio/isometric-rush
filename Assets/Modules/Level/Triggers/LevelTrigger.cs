using System.Collections;
using System.Collections.Generic;
using IsoRush.State;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VContainer;

namespace IsoRush.Level.Triggers
{
    public class LevelTrigger : MonoBehaviour
    {
        [Inject]
        private GameState _gameState;

        [SerializeField]
        private Collider _collider;

        [SerializeField]
        public string TriggerId;

        void Start()
        {
            _collider
                .OnTriggerEnterAsObservable()
                .Where(collider =>
                {
                    return collider.CompareTag("Player");
                })
                .Subscribe(collider =>
                {
                    _gameState.Triggers[TriggerId] = _gameState.Triggers.ContainsKey(TriggerId)
                        ? _gameState.Triggers[TriggerId] + 1
                        : 1;
                })
                .AddTo(this);
        }
    }
}
