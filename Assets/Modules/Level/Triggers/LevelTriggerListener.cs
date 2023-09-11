using System;
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
    public class LevelTriggerListener : MonoBehaviour
    {
        [Inject]
        private GameState _gameState;

        [SerializeField]
        public string TriggerId;

        [SerializeField]
        public int TriggerTarget = 1;

        [SerializeField]
        public UnityEvent OnTriggerTargetReached = new UnityEvent();

        [SerializeField]
        public UnityEvent OnTriggerTargetUnreached = new UnityEvent();

        void Start()
        {
            var replaceObservable = _gameState.Triggers.ObserveReplace();
            var addObservable = _gameState.Triggers
                .ObserveAdd()
                .Select(addEvent =>
                {
                    var replaceEvent = new DictionaryReplaceEvent<string, int>(
                        addEvent.Key,
                        0,
                        addEvent.Value
                    );

                    return replaceEvent;
                });

            Observable
                .Merge(replaceObservable, addObservable)
                .Select(update =>
                {
                    return update.NewValue == TriggerTarget;
                })
                .Subscribe(targetReached =>
                {
                    if (targetReached)
                    {
                        OnTriggerTargetReached.Invoke();
                    }
                    else
                    {
                        OnTriggerTargetUnreached.Invoke();
                    }
                })
                .AddTo(this);
        }
    }
}
