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

        private Dictionary<int, Checkpoint> _checkpointsDict = new Dictionary<int, Checkpoint>();

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

        private async Task SpawnCheckpoint(int gameTime)
        {
            Checkpoint checkpoint = Instantiate(_checkpointPrefab, transform);
            await checkpoint.Spawn();

            _checkpointsDict[gameTime] = checkpoint;
        }

        private async Task DespawnCheckpoint(int gameTime)
        {
            Checkpoint checkpoint = _checkpointsDict[gameTime];
            await checkpoint.Despawn();

            _checkpointsDict.Remove(gameTime);
        }
    }
}
