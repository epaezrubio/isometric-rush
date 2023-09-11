using IsoRush.State;
using IsoRush.Utils;
using UnityEngine;
using VContainer;
using UniRx;
using UniRx.Triggers;
using IsoRush.Level;

namespace IsoRush.Player
{
    public class PlayerCollider : MonoBehaviour
    {
        [Inject]
        private GameState _gameState;

        [Inject]
        private ITriggerer _triggerer;

        [SerializeField]
        private Collider _playerCollider;

        void Start()
        {
            var triggerObservable = _playerCollider.OnTriggerEnterAsObservable();

            triggerObservable
                .Where(_ =>
                {
                    return _gameState != null && _triggerer != null;
                })
                .Where(other =>
                {
                    return other.gameObject.GetComponent<LevelGameOverChunk>() != null;
                })
                .Subscribe(_ =>
                {
                    _triggerer.Trigger(GameStateEvents.OnGameOverTrigger);
                })
                .AddTo(this);
        }
    }
}
