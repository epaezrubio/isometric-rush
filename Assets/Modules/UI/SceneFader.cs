using UnityEngine;
using UnityEngine.UI;

namespace IsoRush.UI
{
    public class SceneFader : MonoBehaviour
    {
        [SerializeField]
        private Image _panel;

        [SerializeField]
        private float _targetAlpha = 0;

        [SerializeField]
        private bool _toggleOnStart = false;

        private float _alpha = 0;

        private void Start()
        {
            _alpha = _targetAlpha;

            if (_toggleOnStart)
            {
                Toggle();
            }
        }

        private void Update()
        {
            _panel.color = new Color(0, 0, 0, _alpha);

            _alpha = Mathf.MoveTowards(_alpha, _targetAlpha, 0.3f * Time.deltaTime);
        }

        public void FadeIn()
        {
            _targetAlpha = 1f;
        }

        public void FadeOut()
        {
            _targetAlpha = 0f;
        }

        public void Toggle()
        {
            if (_targetAlpha > 0)
            {
                _targetAlpha = 0;
            }
            else
            {
                _targetAlpha = 1;
            }
        }
    }
}
