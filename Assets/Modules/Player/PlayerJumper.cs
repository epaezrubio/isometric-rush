using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

namespace IsoRush.Player
{
    public class PlayerJumper : MonoBehaviour
    {
        public enum SideJumpDirection{
            Left,
            Right,
        }

        [SerializeField]
        private float _jumpHeight = 2f;

        [SerializeField]
        private float _jumpRotation = 90f;

        [SerializeField]
        private float _sideJumpDistance = 3f;

        public async Task Jump()
        {
            var jumpTween = transform.DOJump(transform.position, _jumpHeight, 1, 0.6f);
            var rotateTween = transform.DORotateQuaternion(
                transform.rotation * Quaternion.AngleAxis(_jumpRotation, transform.forward),
                0.6f
            );

            await Task.WhenAll(
                jumpTween.AsyncWaitForCompletion(),
                rotateTween.AsyncWaitForCompletion()
            );
        }

        public async Task JumpSideways(SideJumpDirection direction)
        {
            var leftJump = direction == SideJumpDirection.Left;

            var jumpTween = transform.DOJump(
                transform.position
                    + new Vector3(0, 0, leftJump ? -_sideJumpDistance : _sideJumpDistance),
                _jumpHeight,
                1,
                0.6f
            );

            var rotateTween = transform.DORotateQuaternion(
                transform.rotation
                    * Quaternion.AngleAxis(
                        _jumpRotation,
                        leftJump ? -transform.right : transform.right
                    ),
                0.6f
            );

            await Task.WhenAll(
                jumpTween.AsyncWaitForCompletion(),
                rotateTween.AsyncWaitForCompletion()
            );
        }
    }
}
