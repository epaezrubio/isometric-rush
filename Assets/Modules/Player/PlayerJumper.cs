using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

namespace IsoRush.Player
{
    public class PlayerJumper : MonoBehaviour
    {
        [SerializeField]
        private float _jumpHeight = 2f;

        [SerializeField]
        private float _jumpRotation = 90f;

        [SerializeField]
        private float _superJumpHeight = 4f;

        [SerializeField]
        private float _superJumpRotation = 90f;

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

        public async Task SuperJump()
        {
            var jumpTween = transform.DOJump(transform.position, _superJumpHeight, 1, 1f);
            var rotateTween = transform.DORotateQuaternion(
                transform.rotation * Quaternion.AngleAxis(_superJumpRotation, transform.forward),
                1f
            );

            await Task.WhenAll(
                jumpTween.AsyncWaitForCompletion(),
                rotateTween.AsyncWaitForCompletion()
            );
        }
    }
}
