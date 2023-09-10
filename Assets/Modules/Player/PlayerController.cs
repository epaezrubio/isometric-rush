using System.Threading.Tasks;
using IsoRush.Input;
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
        [SerializeField]
        InputActionReference jumpAction;

        PlayerJumper jumper;

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
            if (context.interaction is TapInteraction)
            {
                jumpAction.action.Disable();
                await jumper.Jump();
                jumpAction.action.Enable();
            }
            else if (context.interaction is SlowTapInteraction)
            {
                jumpAction.action.Disable();
                await jumper.SuperJump();
                jumpAction.action.Enable();
            }
        }

        public void EnableControls()
        {
            jumpAction.action.Enable();
            jumpAction.action.performed += OnJumpPressed;
        }

        public void DisableControls()
        {
            jumpAction.action.Disable();
            jumpAction.action.performed -= OnJumpPressed;
        }

        void OnDestroy()
        {
            jumpAction.action.Disable();
            jumpAction.action.performed -= OnJumpPressed;
        }
    }
}
