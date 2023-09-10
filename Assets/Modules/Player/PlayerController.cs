using System;
using System.Threading.Tasks;
using IsoRush.Input;
using IsoRush.State;
using IsoRush.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using VContainer;
using static UnityEngine.InputSystem.InputAction;

namespace IsoRush.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Inject]
        private GameState _gameState;

        [SerializeField]
        private InputActionReference jumpAction;

        [SerializeField]
        private InputActionReference addCheckpointAction;

        private PlayerJumper jumper;

        private bool _isJumping = false;

        private bool _canPlaceCheckpoint
        {
            get
            {
                if (_isJumping)
                {
                    return false;
                }

                if (_gameState == null)
                {
                    return false;
                }

                if (_gameState.InventoryCheckpoints.Value == 0)
                {
                    return false;
                }

                return true;
            }
        }

        void Awake()
        {
            jumper = GetComponent<PlayerJumper>();
        }

        private void OnJumpPressed(CallbackContext context)
        {
            _ = OnJumpPressedAsync(context);
        }

        private void OnAddCheckPoint(CallbackContext context)
        {
            if (!_canPlaceCheckpoint)
            {
                return;
            }

            _gameState.InventoryCheckpoints.Value--;
            _gameState.Checkpoints.Add((int)_gameState.GameTime.Value);
        }

        private async Task OnJumpPressedAsync(CallbackContext context)
        {
            if (context.interaction is TapInteraction)
            {
                _isJumping = true;
                jumpAction.action.Disable();
                await jumper.Jump();
                jumpAction.action.Enable();
                _isJumping = false;
            }
            else if (context.interaction is SlowTapInteraction)
            {
                _isJumping = true;
                jumpAction.action.Disable();
                await jumper.SuperJump();
                jumpAction.action.Enable();
                _isJumping = false;
            }
        }

        public void EnableControls()
        {
            jumpAction.action.Enable();
            jumpAction.action.performed += OnJumpPressed;

            addCheckpointAction.action.Enable();
            addCheckpointAction.action.performed += OnAddCheckPoint;
        }

        public void DisableControls()
        {
            jumpAction.action.Disable();
            jumpAction.action.performed -= OnJumpPressed;

            addCheckpointAction.action.Disable();
            addCheckpointAction.action.performed -= OnAddCheckPoint;
        }

        void OnDestroy()
        {
            jumpAction.action.Disable();
            jumpAction.action.performed -= OnJumpPressed;

            addCheckpointAction.action.Disable();
            addCheckpointAction.action.performed -= OnAddCheckPoint;
        }
    }
}
