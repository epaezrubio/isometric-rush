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
    public class GameStateMachine : StateMachine, ITriggerer, IInitializable, ITickable, IFixedTickable
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
                        _gameState.GameTime.Value += _gameState.GameSpeed.Value * Time.fixedDeltaTime;
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
                    _gameState.GameTime.Value = _gameState.CheckpointGameTime.Value;

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
                        if (_gameState.GameDifficulty.Value == GameDifficulty.Normal)
                        {
                            return false;
                        }

                        return _gameState.CheckpointsCount.Value == 0;
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
                        if (_gameState.GameDifficulty.Value == GameDifficulty.Normal)
                        {
                            return true;
                        }

                        return _gameState.CheckpointsCount.Value > 0;
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
            // OnLogic();
        }

        public void FixedTick()
        {
            OnLogic();
        }

    }
}
