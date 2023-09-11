using System.Linq;
using FSM;
using IsoRush.Player;
using IsoRush.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Debug = UnityEngine.Debug;

namespace IsoRush.State
{
    public class GameStateMachine : StateMachine, ITriggerer, IInitializable, ITickable
    {
        [Inject]
        private GameState _gameState;

        [Inject]
        private PlayerController _playerController;

        public GameStateMachine()
        {
            AddState(
                GameStateStates.Gameplay,
                new State<string>(
                    onEnter: state =>
                    {
                        _playerController?.EnableControls();
                    },
                    onLogic: state =>
                    {
                        _gameState.GameTime.Value += _gameState.GameSpeed.Value * Time.deltaTime;
                    },
                    onExit: state =>
                    {
                        _playerController?.DisableControls();
                    }
                )
            );

            AddState(
                GameStateStates.RestoringCheckpoint,
                new State<string>(onEnter: state =>
                {
                    var checkpoint = _gameState.Checkpoints.Last();
                    _gameState.GameTime.Value = checkpoint;

                    _gameState.Checkpoints.Remove(checkpoint);

                    Trigger(GameStateEvents.OnResumeFromCheckpoint);
                })
            );

            AddState(
                GameStateStates.GameOver,
                new State<string>(onEnter: state =>
                {
                    Debug.Log("Game Over");
                })
            );

            AddTriggerTransition(
                GameStateEvents.OnGameOverTrigger,
                new Transition(
                    GameStateStates.Gameplay,
                    GameStateStates.GameOver,
                    _ =>
                    {
                        return _gameState.Checkpoints.Count == 0;
                    }
                )
            );

            AddTriggerTransition(
                GameStateEvents.OnGameOverTrigger,
                new Transition(
                    GameStateStates.Gameplay,
                    GameStateStates.RestoringCheckpoint,
                    _ =>
                    {
                        return _gameState.Checkpoints.Count > 0;
                    }
                )
            );

            AddTriggerTransition(
                GameStateEvents.OnResumeFromCheckpoint,
                new Transition(GameStateStates.RestoringCheckpoint, GameStateStates.Gameplay)
            );

            SetStartState(GameStateStates.Gameplay);
        }

        public void Initialize()
        {
            Init();
        }

        public void Tick()
        {
            OnLogic();
        }
    }
}
