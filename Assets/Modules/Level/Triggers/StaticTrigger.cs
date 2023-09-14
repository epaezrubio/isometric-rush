using System.Collections;
using System.Collections.Generic;
using IsoRush.State;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace IsoRush.Level.Triggers
{
    public class StaticTrigger : MonoBehaviour
    {
        [Inject]
        private GameState _gameState;

        [SerializeField]
        private Collider _collider;

        [SerializeField]
        public UnityEvent OnTrigger = new UnityEvent();

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
                    OnTrigger.Invoke();
                })
                .AddTo(this);
        }
    }
}
