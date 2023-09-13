using System.Linq;
using FSM;
using IsoRush.Managers;
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
        private AudioManager _audioManager;

        [Inject]
        private PlayerController _playerController;

        [Inject]
        private PhysicsPlayerMover _playerMover;

        public GameStateMachine()
        {
            AddState(
                GameStateStates.Gameplay,
                new State<string>(
                    onEnter: state =>
                    {
                        _audioManager.Play();
                        _playerController?.EnableControls();
                    },
                    onLogic: state =>
                    {
                        _gameState.GameTime.Value += _gameState.GameSpeed.Value * Time.fixedDeltaTime;
                    },
                    onExit: state =>
                    {
                        _audioManager.Stop();
                        _playerController?.DisableControls();
                    }
                )
            );

            AddState(
                GameStateStates.RestoringCheckpoint,
                new State<string>(onEnter: state =>
                {
                    float gameTime =_gameState.CheckpointGameTime.Value;

                    _gameState.GameTime.Value = gameTime;

                    _audioManager.SetMusicTime(gameTime);
                    _audioManager.FadeIn();
                    _playerMover.ResetPositionTo(_gameState.CheckpointPosition.Value);

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
                        _audioManager.FadeOut();

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
