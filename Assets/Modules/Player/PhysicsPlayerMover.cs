using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using VContainer;
using IsoRush.State;
using System;

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
        private float _sideJumpDistance = 5f;

        [SerializeField]
        private Rigidbody _rb;

        private float _targetX = 0;

        void Update()
        {
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
        }

        public void JumpSideways(SideJumpDirection direction)
        {
            var leftJump = direction == SideJumpDirection.Left;

            _targetX += leftJump ? -_sideJumpDistance : _sideJumpDistance;

            Jump(true);
        }

        public void ResetPositionTo(Vector3 value)
        {
            _targetX = value.x;

            _rb.position = value;
            _rb.velocity = Vector3.zero;

            _groundDetector.ForceUngrounded = false;
        }
    }
}
