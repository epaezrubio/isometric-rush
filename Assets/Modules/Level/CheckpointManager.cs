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

        void Start()
        {
            _gameState.Checkpoints
                .ObserveAdd()
                .Subscribe(
                    checkpoint =>
                    {
                        SpawnCheckpoint(checkpoint.Value);
                    }
                )
                .AddTo(this);

            _gameState.Checkpoints
                .ObserveRemove()
                .Subscribe(
                    checkpoint =>
                    {
                        DespawnCheckpoint(checkpoint.Value);
                    }
                )
                .AddTo(this);
        }

        private void SpawnCheckpoint(int gameTime) {
            Debug.Log($"New checkpoint at {gameTime}");
        }

        private void DespawnCheckpoint(int gameTime) {
            Debug.Log($"Removed checkpoint at {gameTime}");
        }
    }
}
