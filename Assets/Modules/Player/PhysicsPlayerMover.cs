using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using VContainer;
using IsoRush.State;

namespace IsoRush.Player
{
    public class PhysicsPlayerMover : MonoBehaviour
    {
        public enum SideJumpDirection
        {
            Left,
            Right,
        }

        [Inject]
        private GameState _gameState;

        [SerializeField]
        private Transform _geometry;

        [SerializeField]
        private GroundedDetector _groundDetector;

        [SerializeField]
        private float _jumpForce = 2f;

        [SerializeField]
        private float _jumpRotation = 90f;

        [SerializeField]
        private float _sideJumpDistance = 5f;

        [SerializeField]
        private Rigidbody _rb;

        private float _targetX = 0;
        private Quaternion _targetRotation;

        void Start()
        {
            _targetRotation = _geometry.rotation;
        }

        void Update()
        {
            _geometry.rotation = Quaternion.RotateTowards(_geometry.rotation, _targetRotation, 2f);
        }

        void FixedUpdate()
        {
            float x = Mathf.MoveTowards(_rb.position.x, _targetX, 0.25f);

            _rb.MovePosition(
                new Vector3(
                    x,
                    _rb.position.y,
                    _gameState.GameTime.Value * _gameState.ScrollSpeed.Value
                )
            );
        }

        public void Jump(bool force = false)
        {
            if (!force && !_groundDetector.IsGrounded)
            {
                return;
            }

            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _groundDetector.ForceUngrounded = true;

            // _targetRotation *= Quaternion.AngleAxis(_jumpRotation, transform.right);
        }

        public void JumpSideways(SideJumpDirection direction)
        {
            var leftJump = direction == SideJumpDirection.Left;

            _targetX += leftJump ? -_sideJumpDistance : _sideJumpDistance;
            _targetRotation *= Quaternion.AngleAxis(
                _jumpRotation,
                leftJump ? transform.forward : -transform.forward
            );

            Jump(true);
        }
    }
}
