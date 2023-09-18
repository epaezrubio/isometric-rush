using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using VContainer;
using IsoRush.State;

namespace IsoRush.Level.Triggers
{
    public class LevelCompletedTrigger : MonoBehaviour
    {
        [Inject]
        private GameStateMachine _stateMachine;

        [SerializeField]
        private Collider _collider;

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
                    _stateMachine.Trigger(GameStateEvents.OnGameCompleted);
                })
                .AddTo(this);
        }
    }
}
