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
    public class GameStateMachine
        : StateMachine,
            ITriggerer,
            IInitializable,
            ITickable,
            IFixedTickable
    {
        [Inject]
        private GameState _gameState;

        [Inject]
        private AudioManager _audioManager;

        [Inject]
        private PlayerController _playerController;

        [Inject]
        private PhysicsPlayerMover _playerMover;

        [Inject]
        private CameraTracker _cameraTracker;

        public GameStateMachine()
        {
            AddState(
                GameStateStates.Gameplay,
                new State<string>(
                    onEnter: state =>
                    {
                        _cameraTracker.xOverride = false;
                        _audioManager?.Play();
                        _audioManager?.FadeIn();
                        _audioManager?.SetMusicTime(_gameState.GameTime.Value);
                        _playerMover?.DisableRagDoll();
                        _playerController?.EnableControls();
                        _gameState.GameSpeed.Value = 1;
                    },
                    onExit: state =>
                    {
                        _playerController?.DisableControls();
                    }
                )
            );

            AddState(
                GameStateStates.CrashStoppingGameplay,
                new State<string>(
                    onEnter: state =>
                    {
                        _audioManager.FadeOut();
                        _playerMover.EnableRagDoll();
                    },
                    onLogic: state =>
                    {
                        _gameState.GameSpeed.Value = Mathf.MoveTowards(
                            _gameState.GameSpeed.Value,
                            0,
                            0.8f * Time.fixedDeltaTime
                        );
                    }
                )
            );

            AddState(
                GameStateStates.RestoringCheckpointTime,
                new State<string>(
                    onEnter: state =>
                    {
                        _cameraTracker.xOverride = true;
                        _cameraTracker.trackedXOverride = _cameraTracker.transform.position.x;
                    },
                    onLogic: state =>
                    {
                        _cameraTracker.trackedXOverride = Mathf.MoveTowards(
                            _cameraTracker.trackedXOverride,
                            _gameState.CheckpointPosition.Value.x,
                            10f * Time.fixedDeltaTime
                        );

                        _gameState.GameTime.Value = Mathf.MoveTowards(
                            _gameState.GameTime.Value,
                            _gameState.CheckpointGameTime.Value,
                            5f * Time.fixedDeltaTime
                        );
                    }
                )
            );

            AddState(
                GameStateStates.RestoringCheckpoint,
                new State<string>(onEnter: state =>
                {
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
                    GameStateStates.CrashStoppingGameplay,
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

            AddTransition(
                new Transition(
                    GameStateStates.CrashStoppingGameplay,
                    GameStateStates.RestoringCheckpointTime,
                    _ =>
                    {
                        return _gameState.GameSpeed.Value == 0;
                    }
                )
            );

            AddTransition(
                new Transition(
                    GameStateStates.RestoringCheckpointTime,
                    GameStateStates.RestoringCheckpoint,
                    _ =>
                    {
                        return _gameState.GameTime.Value == _gameState.CheckpointGameTime.Value;
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
