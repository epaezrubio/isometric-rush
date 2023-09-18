using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using VContainer;
using IsoRush.State;
using System;
using System.Collections.Generic;

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
        private Animator _animator;

        [SerializeField]
        private PlayerFX _playerFX;

        [SerializeField]
        private float _jumpForce = 3f;

        [SerializeField]
        private float _sideJumpDistance = 5f;

        [SerializeField]
        private Rigidbody _rb;

        public float targetX = 0;

        private bool _ragDoll = false;

        void Update() { }

        void FixedUpdate()
        {
            if (_ragDoll)
            {
                return;
            }

            float x = Mathf.MoveTowards(_rb.position.x, targetX, 0.25f);

            _rb.MovePosition(
                new Vector3(
                    x,
                    _rb.position.y,
                    _gameState.GameTime.Value * _gameState.ScrollSpeed.Value
                )
            );
        }

        public void Jump(bool force = false, bool animate = true)
        {
            if (!force && !_groundDetector.IsGrounded)
            {
                return;
            }

            _rb.velocity = Vector3.up * _jumpForce;
            _groundDetector.ForceUngrounded = true;
            _playerFX.PlayJumpSound();

            if (animate)
            {
                _animator.Play("Jump Animation");
            }
        }

        public void JumpSideways(SideJumpDirection direction)
        {
            var leftJump = direction == SideJumpDirection.Left;

            targetX += leftJump ? -_sideJumpDistance : _sideJumpDistance;

            _animator.Play(leftJump ? "Left Side Jump Animation" : "Right Side Jump Animation");

            Jump(true, false);
        }

        public void ResetPositionTo(Vector3 value)
        {
            targetX = value.x;

            _rb.position = value;
            _rb.velocity = Vector3.zero;

            _groundDetector.ForceUngrounded = false;
        }

        public void EnableRagDoll()
        {
            _rb.velocity = _rb.velocity + transform.forward * 8;
            _rb.constraints = RigidbodyConstraints.None;

            _ragDoll = true;
        }

        public void DisableRagDoll()
        {
            _ragDoll = false;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            _rb.rotation = Quaternion.identity;
        }

        public void EnablePhysics()
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;
        }

        public void DisablePhysics()
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
        }
    }
}
