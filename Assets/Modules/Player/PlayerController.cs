using System.Threading.Tasks;
using IsoRush.State;
using UnityEngine;
using UnityEngine.InputSystem;
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

        private PhysicsPlayerMover jumper;

        void Awake()
        {
            jumper = GetComponent<PhysicsPlayerMover>();
        }

        private void OnJumpPressed(CallbackContext context)
        {
            _ = OnJumpPressedAsync(context);
        }

        private async Task OnJumpPressedAsync(CallbackContext context)
        {
            jumpAction.action.Disable();
            jumper.Jump();
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

            _gameState.CameraPositionTarget.Value = new Vector3(0, 7 * zoomActionValue, 10 * zoomActionValue);
            _gameState.CameraSizeTarget.Value = 10 + 10 * zoomActionValue;
        }

        void OnDestroy()
        {
            jumpAction.action.Disable();
            jumpAction.action.performed -= OnJumpPressed;
        }
    }
}
