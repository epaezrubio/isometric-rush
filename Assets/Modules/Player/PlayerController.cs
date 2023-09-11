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
        private InputActionReference zoomAction;

        private PlayerJumper jumper;

        void Awake()
        {
            jumper = GetComponent<PlayerJumper>();
        }

        private void OnJumpPressed(CallbackContext context)
        {
            _ = OnJumpPressedAsync(context);
        }

        private async Task OnJumpPressedAsync(CallbackContext context)
        {
            jumpAction.action.Disable();
            await jumper.Jump();
            jumpAction.action.Enable();
        }

        public void EnableControls()
        {
            jumpAction.action.Enable();
            jumpAction.action.performed += OnJumpPressed;

            zoomAction.action.Enable();
        }

        public void DisableControls()
        {
            jumpAction.action.Disable();
            jumpAction.action.performed -= OnJumpPressed;

            zoomAction.action.Disable();
        }

        void Update()
        {
            float zoomActionValue = zoomAction.action.ReadValue<float>();

            _gameState.CameraPositionTarget.Value = new Vector3(-10 * zoomActionValue, 0, 0);
            _gameState.CameraSizeTarget.Value = 10 + 10 * zoomActionValue;
        }

        void OnDestroy()
        {
            jumpAction.action.Disable();
            jumpAction.action.performed -= OnJumpPressed;
        }
    }
}
