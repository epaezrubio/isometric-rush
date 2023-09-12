using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace IsoRush.Player
{
    public class GroundedDetector : MonoBehaviour
    {
        private int _grounds = 0;

        public bool ForceUngrounded = false;

        public bool IsGrounded
        {
            get { return !ForceUngrounded && _grounds > 0; }
        }

        void Start()
        {
            Collider collider = GetComponent<Collider>();

            collider
                .OnTriggerEnterAsObservable()
                .Subscribe(_ =>
                {
                    _grounds++;
                });

            collider
                .OnTriggerExitAsObservable()
                .Subscribe(_ =>
                {
                    _grounds--;

                    if (_grounds == 0)
                    {
                        ForceUngrounded = false;
                    }
                });
        }
    }
}
