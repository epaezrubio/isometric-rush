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
                        _gameState.GameTime.Value += _gameState.ScrollSpeed.Value * Time.deltaTime;
                    },
                    onExit: state =>
                    {
                        _playerController?.DisableControls();
                    }
                )
            );

            AddState(
                GameStateStates.RestoringCheckpoint,
                new State<string>(onLogic: state =>
                {
                    Debug.Log("Restoring Checkpoint");
                })
            );

            AddState(
                GameStateStates.GameOver,
                new State<string>(onLogic: state =>
                {
                    Debug.Log("GameOver");
                })
            );

            AddTriggerTransition(
                GameStateEvents.OnGameOverTrigger,
                new TransitionBase<string>(GameStateStates.Gameplay, GameStateStates.GameOver)
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
