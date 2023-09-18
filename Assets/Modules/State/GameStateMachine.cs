using System.Linq;
using FSM;
using IsoRush.Managers;
using IsoRush.Player;
using IsoRush.UI;
using IsoRush.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        private PhysicsPlayerMover _physicsPlayerMover;

        [Inject]
        private PlayerAnimator _playerAnimator;

        [Inject]
        private PhysicsPlayerMover _playerMover;

        [Inject]
        private PlayerFX _playerFX;

        [Inject]
        private CameraTracker _cameraTracker;

        [Inject]
        private SceneFader _sceneFader;

        public GameStateMachine()
        {
            AddState(
                GameStateStates.Init,
                new State<string>(onEnter: state =>
                {
                    // _sceneFader.FadeOut();
                })
            );

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
                        _playerFX.PlayDestroySound();
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

                        _audioManager.StartWarpSound();
                    },
                    onLogic: state =>
                    {
                        _cameraTracker.trackedXOverride = Mathf.MoveTowards(
                            _cameraTracker.trackedXOverride,
                            _gameState.CheckpointPosition.Value.x,
                            22f * Time.fixedDeltaTime
                        );

                        _gameState.GameTime.Value = Mathf.MoveTowards(
                            _gameState.GameTime.Value,
                            _gameState.CheckpointGameTime.Value,
                            5f * Time.fixedDeltaTime
                        );
                    },
                    onExit: state =>
                    {
                        _audioManager.StopWarpSound();
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
                GameStateStates.OutroAnimation,
                new State<string>(
                    onEnter: state =>
                    {
                        _gameState.GameSpeed.Value = 0;
                        _gameState.CameraCinematic.Value = true;

                        _physicsPlayerMover.DisablePhysics();
                        _playerAnimator.GetComponent<Animator>().SetBool("OutroAnimation", true);
                    },
                    onLogic: state =>
                    {
                        if (state.timer.Elapsed > 10f)
                        {
                            Trigger(GameStateEvents.OnExit);
                        }
                        else if (state.timer.Elapsed > 5f)
                        {
                            _sceneFader.FadeIn();
                            _audioManager.FadeOut(0.3f);
                        }
                    },
                    onExit: state =>
                    {
                        _gameState.GameSpeed.Value = 1;
                        _gameState.CameraCinematic.Value = false;

                        _physicsPlayerMover.EnablePhysics();
                        _playerAnimator.GetComponent<Animator>().SetBool("OutroAnimation", false);

                        // main menu
                        SceneManager.LoadScene(0);
                    }
                )
            );

            AddTransition(new Transition(GameStateStates.Init, GameStateStates.Gameplay));

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

            AddTriggerTransition(
                GameStateEvents.OnGameCompleted,
                new Transition(GameStateStates.Gameplay, GameStateStates.OutroAnimation)
            );

            AddTriggerTransition(
                GameStateEvents.OnExit,
                new Transition(GameStateStates.OutroAnimation, GameStateStates.Exit)
            );

            SetStartState(GameStateStates.Init);
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
