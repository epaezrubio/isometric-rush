using IsoRush.State;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VContainer;

namespace IsoRush.Level
{
    public class Checkpoint : MonoBehaviour
    {
        [Inject]
        private GameState _gameState;

        [SerializeField]
        private Collider _collider;

        [SerializeField]
        private int _checkpointsCount = -1;

        void Start()
        {
            _collider
                .OnTriggerEnterAsObservable()
                .Where(collider => collider.CompareTag("Player"))
                .Subscribe(collider =>
                {
                    _gameState.CheckpointPosition.Value = transform.parent.position;
                    _gameState.CheckpointGameTime.Value = _gameState.GameTime.Value;
                    _gameState.CheckpointsCount.Value = _checkpointsCount;
                })
                .AddTo(this);
        }
    }
}
