using IsoRush.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using static IsoRush.Player.PhysicsPlayerMover;

namespace IsoRush.Level.Jumpers
{
    public class LevelTriggerListener : MonoBehaviour
    {
        [SerializeField]
        private Collider _collider;

        [SerializeField]
        private SideJumpDirection _direction;

        [SerializeField]
        private AudioSource _fx;

        void Start()
        {
            _collider
                .OnTriggerEnterAsObservable()
                .Where(collider => collider.CompareTag("Player"))
                .Subscribe(collider =>
                {
                    collider.GetComponent<PhysicsPlayerMover>().JumpSideways(_direction);
                    _fx.Play();
                })
                .AddTo(this);
        }
    }
}
