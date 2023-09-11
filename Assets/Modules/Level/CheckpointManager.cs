using System.Collections.Generic;
using System.Threading.Tasks;
using IsoRush.State;
using UniRx;
using UnityEngine;
using VContainer;

namespace IsoRush.Level
{
    public class CheckpointManager : MonoBehaviour
    {
        [Inject]
        private GameState _gameState;

        [SerializeField]
        private Checkpoint _checkpointPrefab;

        private Dictionary<float, Checkpoint> _checkpointsDict = new Dictionary<float, Checkpoint>();

        void Start()
        {
            _gameState.Checkpoints
                .ObserveAdd()
                .Subscribe(checkpoint =>
                {
                    _ = SpawnCheckpoint(checkpoint.Value);
                })
                .AddTo(this);

            _gameState.Checkpoints
                .ObserveRemove()
                .Subscribe(checkpoint =>
                {
                    _ = DespawnCheckpoint(checkpoint.Value);
                })
                .AddTo(this);
        }

        private async Task SpawnCheckpoint(float gameTime)
        {
            Checkpoint checkpoint = Instantiate(_checkpointPrefab, transform);
            await checkpoint.Spawn();

            _checkpointsDict[gameTime] = checkpoint;
        }

        private async Task DespawnCheckpoint(float gameTime)
        {
            Checkpoint checkpoint = _checkpointsDict[gameTime];
            await checkpoint.Despawn();

            _checkpointsDict.Remove(gameTime);
        }
    }
}
