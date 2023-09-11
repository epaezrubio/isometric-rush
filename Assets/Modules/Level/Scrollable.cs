using IsoRush.State;
using UniRx;
using UnityEngine;
using VContainer;

namespace IsoRush.Level
{
    public class Scrollable : MonoBehaviour
    {
        [Inject]
        protected GameState gameState;

        [SerializeField]
        protected float speedMultiplier = 1;

        void Start()
        {
            gameState.GameTime
                .Subscribe(gameTime =>
                {
                    SetScroll(gameTime * gameState.ScrollSpeed.Value * speedMultiplier);
                })
                .AddTo(this);
        }

        protected virtual void SetScroll(float scroll) { }
    }
}
